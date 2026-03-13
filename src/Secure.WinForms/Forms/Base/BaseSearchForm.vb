Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Text.Json
Imports DevExpress.Data
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout
Imports DevExpress.XtraPrinting
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Base
    ''' <summary>
    ''' Formulario base reutilizable para busqueda/listado con paginacion y filtros.
    ''' Autor: Mario Gomez.
    ''' </summary>
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class BaseSearchForm
        Inherits XtraForm

        Protected Class SearchPageRequest
            Public Property Page As Integer
            Public Property Size As Integer
            Public Property SortBy As String
            Public Property SortDirection As String
            Public Property Filter As String
            Public Property FilterField As String
            Public Property IdTenant As Long?
            Public Property IdEmpresa As Long?
        End Class

        Protected Class SearchPageResult
            Public Property Page As Integer
            Public Property Size As Integer
            Public Property Total As Integer
            Public Property Data As DataTable
        End Class

        Private Class PersistedLayoutState
            Public Property v As Integer
            Public Property p As Integer
            Public Property f As String
            Public Property ff As String
            Public Property cols As List(Of PersistedLayoutColumnState)
        End Class

        Private Class PersistedLayoutColumnState
            Public Property n As String
            Public Property i As Integer
            Public Property w As Integer
            Public Property vis As Boolean
            Public Property s As String
        End Class

        Protected Ribbon As RibbonControl
        Protected Grid As GridControl
        Protected View As GridView
        Protected MainLayout As LayoutControl
        Protected TxtFiltro As TextEdit
        Protected CmbCampoFiltro As ComboBoxEdit
        Protected SpnTamanoPagina As SpinEdit

        Private _statusBar As RibbonStatusBar
        Private _statusInfo As BarStaticItem
        Private _statusPaginacion As BarStaticItem
        Private ReadOnly _apiClient As ApiClient
        Private ReadOnly _sessionContext As UserSessionContext
        Private _btnNuevo As BarButtonItem
        Private _btnEditar As BarButtonItem
        Private _btnRefrescar As BarButtonItem
        Private _btnPrimero As BarButtonItem
        Private _btnAnterior As BarButtonItem
        Private _btnSiguiente As BarButtonItem
        Private _btnUltimo As BarButtonItem
        Private _btnGuardarVista As BarButtonItem
        Private _btnRestaurarVista As BarButtonItem
        Private _btnExportar As BarButtonItem
        Private _repoCampoFiltro As RepositoryItemComboBox
        Private _repoBusqueda As RepositoryItemSearchControl
        Private _repoTamanoPagina As RepositoryItemSpinEdit
        Private _barCampoFiltro As BarEditItem
        Private _barBusqueda As BarEditItem
        Private _barBuscar As BarButtonItem
        Private _barLimpiar As BarButtonItem
        Private _barTamanoPagina As BarEditItem
        Private _barPaginaAnterior As BarButtonItem
        Private _barPaginaSiguiente As BarButtonItem
        Private _barResumenPagina As BarStaticItem
        Private _rpInicio As RibbonPage
        Private _rpgOperaciones As RibbonPageGroup
        Private _rpgBusqueda As RibbonPageGroup
        Private _rpgPaginacion As RibbonPageGroup
        Private _rpgNavegacion As RibbonPageGroup
        Private _rpgVistaReporte As RibbonPageGroup
        Private _layoutRoot As LayoutControlGroup
        Private _layoutGridItem As LayoutControlItem
        Private _searchDebounceTimer As Timer

        Private _sourceData As DataTable
        Private _currentPage As Integer
        Private _totalPages As Integer
        Private _totalRows As Integer
        Private _isRefreshingGrid As Boolean
        Private _isFirstLoadDone As Boolean
        Private _isSyncingRibbon As Boolean

        Public Sub New()
            Me.New(Nothing, Nothing)
        End Sub

        Protected Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            _apiClient = apiClient
            _sessionContext = If(sessionContext, UserSessionContext.CrearDiseno())

            InitializeComponent()
            _searchDebounceTimer = New Timer()
            _currentPage = 1
            _totalPages = 1
            _totalRows = 0
            _isFirstLoadDone = False
            _isSyncingRibbon = False

            Text = BuildFormTitle()
            Width = 1220
            Height = 760
            StartPosition = FormStartPosition.CenterParent
            AutoScaleMode = AutoScaleMode.Dpi

            ConfigureGrid()
            ConfigureRibbon()
            ConfigureLayout()
            ConfigureStatus()

            SpnTamanoPagina.Properties.IsFloatValue = False
            SpnTamanoPagina.Properties.MinValue = 5
            SpnTamanoPagina.Properties.MaxValue = 500
            SpnTamanoPagina.Properties.Increment = 5
            SpnTamanoPagina.EditValue = 25

            ConfigureColumns(View)
            PopulateFilterFieldsFromGrid()
            SyncRibbonFromState()

            _searchDebounceTimer.Interval = 400
            AddHandler _searchDebounceTimer.Tick, AddressOf OnSearchDebounceTick

            AddHandler Shown, AddressOf OnFirstShown
            AddHandler FormClosing, AddressOf OnFormClosingPersistLayout
        End Sub

        Public ReadOnly Property ModuleRibbon As RibbonControl
            Get
                Return Ribbon
            End Get
        End Property

        Public Overridable ReadOnly Property ModuleTitle As String
            Get
                Return BuildFormTitle()
            End Get
        End Property

        Public Overridable Sub SetRibbonHostMode(ByVal hosted As Boolean)
            Ribbon.Visible = Not hosted
        End Sub

        Protected Overridable ReadOnly Property UsaPaginacionServidor As Boolean
            Get
                Return False
            End Get
        End Property

        Protected ReadOnly Property ApiClient As ApiClient
            Get
                Return _apiClient
            End Get
        End Property

        Protected ReadOnly Property SessionContext As UserSessionContext
            Get
                Return _sessionContext
            End Get
        End Property

        Private Sub ConfigureRibbon()
            Ribbon.Dock = DockStyle.Top
            Ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            Ribbon.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden
            Ribbon.MdiMergeStyle = RibbonMdiMergeStyle.Always

            _barCampoFiltro.Edit = _repoCampoFiltro

            _barBusqueda.Edit = _repoBusqueda

            _barTamanoPagina.Edit = _repoTamanoPagina

            _repoCampoFiltro.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _repoBusqueda.NullValuePrompt = "Buscar en datos..."
            _repoBusqueda.NullValuePromptShowForEmptyValue = True
            _repoBusqueda.Client = Grid
            _repoTamanoPagina.IsFloatValue = False
            _repoTamanoPagina.MinValue = 5
            _repoTamanoPagina.MaxValue = 500
            _repoTamanoPagina.Increment = 5

            If Not Ribbon.RepositoryItems.Contains(_repoCampoFiltro) Then Ribbon.RepositoryItems.Add(_repoCampoFiltro)
            If Not Ribbon.RepositoryItems.Contains(_repoBusqueda) Then Ribbon.RepositoryItems.Add(_repoBusqueda)
            If Not Ribbon.RepositoryItems.Contains(_repoTamanoPagina) Then Ribbon.RepositoryItems.Add(_repoTamanoPagina)

            AssignIcon(_btnNuevo, "Actions.Add")
            AssignIcon(_btnEditar, "Edit.Edit")
            AssignIcon(_btnRefrescar, "Actions.Refresh")
            AssignIcon(_btnPrimero, "Navigation.First|Arrows.ArrowUp")
            AssignIcon(_btnAnterior, "Navigation.Back|Navigation.Previous|Arrows.ArrowLeft")
            AssignIcon(_btnSiguiente, "Navigation.Forward|Navigation.Next|Arrows.ArrowRight")
            AssignIcon(_btnUltimo, "Navigation.Last|Arrows.ArrowDown")
            AssignIcon(_btnGuardarVista, "Save.Save")
            AssignIcon(_btnRestaurarVista, "Actions.Reset|Actions.Undo")
            AssignIcon(_btnExportar, "Export.ExportToXLSX|Export.ExportTo")
            AssignIcon(_barBuscar, "Find.Find|Actions.Search")
            AssignIcon(_barLimpiar, "Actions.Clear")
            AssignIcon(_barPaginaAnterior, "Navigation.Back|Navigation.Previous|Arrows.ArrowLeft")
            AssignIcon(_barPaginaSiguiente, "Navigation.Forward|Navigation.Next|Arrows.ArrowRight")

            _barCampoFiltro.ImageOptions.Image = Nothing
            _barCampoFiltro.ImageOptions.SvgImage = Nothing
            _barBusqueda.ImageOptions.Image = Nothing
            _barBusqueda.ImageOptions.SvgImage = Nothing
            _barTamanoPagina.ImageOptions.Image = Nothing
            _barTamanoPagina.ImageOptions.SvgImage = Nothing

            ApplyRibbonButtonVisual(_btnNuevo)
            ApplyRibbonButtonVisual(_btnEditar)
            ApplyRibbonButtonVisual(_btnRefrescar)
            ApplyRibbonButtonVisual(_btnPrimero)
            ApplyRibbonButtonVisual(_btnAnterior)
            ApplyRibbonButtonVisual(_btnSiguiente)
            ApplyRibbonButtonVisual(_btnUltimo)
            ApplyRibbonButtonVisual(_btnGuardarVista)
            ApplyRibbonButtonVisual(_btnRestaurarVista)
            ApplyRibbonButtonVisual(_btnExportar)
            ApplyRibbonButtonVisual(_barBuscar)
            ApplyRibbonButtonVisual(_barLimpiar)
            ApplyRibbonButtonVisual(_barPaginaAnterior)
            ApplyRibbonButtonVisual(_barPaginaSiguiente)
            ApplyRibbonEditVisual(_barCampoFiltro)
            ApplyRibbonEditVisual(_barBusqueda)
            ApplyRibbonEditVisual(_barTamanoPagina)

            AddHandler _btnNuevo.ItemClick, AddressOf OnNuevoClick
            AddHandler _btnEditar.ItemClick, AddressOf OnEditarClick
            AddHandler _btnRefrescar.ItemClick, AddressOf OnRefrescarClick
            AddHandler _btnPrimero.ItemClick, AddressOf OnPrimerRegistroClick
            AddHandler _btnAnterior.ItemClick, AddressOf OnRegistroAnteriorClick
            AddHandler _btnSiguiente.ItemClick, AddressOf OnRegistroSiguienteClick
            AddHandler _btnUltimo.ItemClick, AddressOf OnUltimoRegistroClick
            AddHandler _btnGuardarVista.ItemClick, AddressOf OnGuardarVistaClick
            AddHandler _btnRestaurarVista.ItemClick, AddressOf OnRestaurarVistaClick
            AddHandler _btnExportar.ItemClick, AddressOf OnExportarClick
            AddHandler _barBuscar.ItemClick, AddressOf OnBuscarRibbonClick
            AddHandler _barLimpiar.ItemClick, AddressOf OnLimpiarRibbonClick
            AddHandler _barPaginaAnterior.ItemClick, AddressOf OnPaginaAnteriorRibbonClick
            AddHandler _barPaginaSiguiente.ItemClick, AddressOf OnPaginaSiguienteRibbonClick
            AddHandler _barCampoFiltro.EditValueChanged, AddressOf OnCampoFiltroRibbonChanged
            AddHandler _barBusqueda.EditValueChanged, AddressOf OnTextoBusquedaRibbonChanged
            AddHandler _barTamanoPagina.EditValueChanged, AddressOf OnTamanoPaginaRibbonChanged
            _barTamanoPagina.EditValue = 25
        End Sub

        Private Sub ConfigureGrid()
            Grid.Dock = DockStyle.Fill
            Grid.MainView = View
            Grid.UseEmbeddedNavigator = False

            View.OptionsBehavior.Editable = False
            View.OptionsBehavior.AllowIncrementalSearch = True
            View.OptionsView.ShowGroupPanel = True
            View.OptionsView.ColumnAutoWidth = False
            View.OptionsView.ShowAutoFilterRow = False
            View.OptionsSelection.EnableAppearanceFocusedCell = False
            View.FocusRectStyle = DrawFocusRectStyle.RowFocus
            View.OptionsCustomization.AllowSort = True
            View.OptionsCustomization.AllowFilter = True
            View.OptionsFind.AllowFindPanel = False
            View.OptionsFind.AlwaysVisible = False
            View.OptionsFind.ShowFindButton = False
            View.OptionsFind.FindNullPrompt = "Buscar en datos..."

            AddHandler View.EndSorting, AddressOf OnGridEndSorting
        End Sub

        Private Sub ConfigureLayout()
            MainLayout.Dock = DockStyle.Fill
            If MainLayout.Root Is Nothing Then
                MainLayout.Root = New LayoutControlGroup() With {
                    .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                    .GroupBordersVisible = False
                }
            End If

            If Grid.Parent IsNot MainLayout Then
                MainLayout.Controls.Add(Grid)
            End If

            Dim rootGroup = TryCast(MainLayout.Root, LayoutControlGroup)
            If rootGroup IsNot Nothing AndAlso rootGroup.Items.Count = 0 Then
                Dim item = rootGroup.AddItem(String.Empty, Grid)
                item.TextVisible = False
            End If
        End Sub

        Private Sub ConfigureStatus()
            _statusBar.Dock = DockStyle.Bottom
            _statusBar.Ribbon = Ribbon
            _statusInfo.Caption = $"Origen: {BuildEndpoint()}"
            _statusPaginacion.Caption = "Registros: 0"
            _statusBar.ItemLinks.Clear()
            _statusBar.ItemLinks.Add(_statusInfo)
            _statusBar.ItemLinks.Add(_statusPaginacion)
        End Sub

        Private Shared Function NormalizeFilterFieldDisplay(ByVal rawValue As String) As String
            If String.IsNullOrWhiteSpace(rawValue) OrElse String.Equals(rawValue, "*", StringComparison.Ordinal) Then
                Return "(Todos)"
            End If

            Return rawValue
        End Function

        Private Sub SyncRibbonFromState()
            _isSyncingRibbon = True
            Try
                Dim selectedField = If(CmbCampoFiltro.SelectedItem IsNot Nothing, CmbCampoFiltro.SelectedItem.ToString(), "(Todos)")
                _barCampoFiltro.EditValue = NormalizeFilterFieldDisplay(selectedField)
                _barBusqueda.EditValue = TxtFiltro.Text
                _barTamanoPagina.EditValue = GetPageSize()
            Finally
                _isSyncingRibbon = False
            End Try
        End Sub

        Private Sub AssignIcon(ByVal item As BarItem, ByVal iconKeys As String)
            If item Is Nothing OrElse String.IsNullOrWhiteSpace(iconKeys) Then Return

            Dim selectedSvg As SvgImage = Nothing
            For Each candidate In iconKeys.Split("|"c, StringSplitOptions.RemoveEmptyEntries)
                Try
                    Dim svg = TryCast(IconService.GetIcon(candidate.Trim()), SvgImage)
                    If svg IsNot Nothing Then
                        selectedSvg = svg
                        Exit For
                    End If
                Catch
                    ' En diseno el cache de imagenes puede no resolver algunas claves; usar fallback.
                End Try
            Next

            Dim selectedBitmap As Image = Nothing
            If selectedSvg Is Nothing Then
                selectedBitmap = BuildFallbackBitmap(iconKeys)
            End If

            If TypeOf item Is BarButtonItem Then
                Dim button = DirectCast(item, BarButtonItem)
                If selectedSvg IsNot Nothing Then
                    button.ImageOptions.SvgImage = selectedSvg
                    button.ImageOptions.SvgImageSize = New Size(16, 16)
                ElseIf selectedBitmap IsNot Nothing Then
                    button.ImageOptions.Image = selectedBitmap
                End If
            ElseIf TypeOf item Is BarEditItem Then
                Dim editor = DirectCast(item, BarEditItem)
                If selectedSvg IsNot Nothing Then
                    editor.ImageOptions.SvgImage = selectedSvg
                    editor.ImageOptions.SvgImageSize = New Size(16, 16)
                ElseIf selectedBitmap IsNot Nothing Then
                    editor.ImageOptions.Image = selectedBitmap
                End If
            End If
        End Sub

        Private Shared Function BuildFallbackBitmap(ByVal iconKeys As String) As Image
            Dim normalized = If(iconKeys, String.Empty).ToLowerInvariant()
            Dim action = ResolveFallbackAction(normalized)
            Dim bmp As New Bitmap(16, 16)

            Using g = Graphics.FromImage(bmp)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.Clear(Color.Transparent)

                Select Case action
                    Case "add"
                        DrawPlusIcon(g, Color.ForestGreen)
                    Case "edit"
                        DrawPencilIcon(g, Color.DarkOrange)
                    Case "refresh"
                        DrawRefreshIcon(g, Color.DodgerBlue)
                    Case "close"
                        DrawCloseIcon(g, Color.Crimson)
                    Case "save"
                        DrawSaveIcon(g, Color.SteelBlue)
                    Case "export"
                        DrawExportIcon(g, Color.SeaGreen)
                    Case "reset"
                        DrawUndoIcon(g, Color.DarkGoldenrod)
                    Case "search"
                        DrawSearchIcon(g, Color.MidnightBlue)
                    Case "filter"
                        DrawFilterIcon(g, Color.Purple)
                    Case "first"
                        DrawFirstIcon(g, Color.DimGray)
                    Case "previous"
                        DrawArrowIcon(g, True, Color.DimGray)
                    Case "next"
                        DrawArrowIcon(g, False, Color.DimGray)
                    Case "last"
                        DrawLastIcon(g, Color.DimGray)
                    Case "page"
                        DrawPageIcon(g, Color.SlateBlue)
                    Case Else
                        DrawDotIcon(g, Color.Gray)
                End Select
            End Using

            Return bmp
        End Function

        Private Shared Function ResolveFallbackAction(ByVal normalizedKeys As String) As String
            If normalizedKeys.Contains("add") Then Return "add"
            If normalizedKeys.Contains("edit") Then Return "edit"
            If normalizedKeys.Contains("refresh") Then Return "refresh"
            If normalizedKeys.Contains("close") OrElse normalizedKeys.Contains("cancel") Then Return "close"
            If normalizedKeys.Contains("save") Then Return "save"
            If normalizedKeys.Contains("export") Then Return "export"
            If normalizedKeys.Contains("reset") OrElse normalizedKeys.Contains("undo") OrElse normalizedKeys.Contains("clear") Then Return "reset"
            If normalizedKeys.Contains("filter") Then Return "filter"
            If normalizedKeys.Contains("find") OrElse normalizedKeys.Contains("search") Then Return "search"
            If normalizedKeys.Contains("first") Then Return "first"
            If normalizedKeys.Contains("last") Then Return "last"
            If normalizedKeys.Contains("back") OrElse normalizedKeys.Contains("previous") Then Return "previous"
            If normalizedKeys.Contains("next") OrElse normalizedKeys.Contains("forward") Then Return "next"
            If normalizedKeys.Contains("page") OrElse normalizedKeys.Contains("print") Then Return "page"
            Return "unknown"
        End Function

        Private Shared Sub DrawPlusIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                g.DrawLine(p, 8, 3, 8, 13)
                g.DrawLine(p, 3, 8, 13, 8)
            End Using
        End Sub

        Private Shared Sub DrawPencilIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                g.DrawLine(p, 3, 12, 10, 5)
                g.DrawLine(p, 5, 13, 12, 6)
            End Using
            Using b As New SolidBrush(color)
                g.FillPolygon(b, New Point() {New Point(11, 4), New Point(13, 2), New Point(14, 5)})
            End Using
        End Sub

        Private Shared Sub DrawRefreshIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                g.DrawArc(p, 3, 3, 10, 10, 35, 280)
                g.DrawLine(p, 10, 2, 13, 2)
                g.DrawLine(p, 13, 2, 13, 5)
            End Using
        End Sub

        Private Shared Sub DrawCloseIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.4F)
                g.DrawLine(p, 3, 3, 13, 13)
                g.DrawLine(p, 13, 3, 3, 13)
            End Using
        End Sub

        Private Shared Sub DrawSaveIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 1.8F)
                g.DrawRectangle(p, 3, 3, 10, 10)
                g.DrawLine(p, 5, 3, 11, 3)
                g.DrawRectangle(p, 5, 8, 6, 5)
            End Using
            Using b As New SolidBrush(color)
                g.FillRectangle(b, 5, 5, 5, 2)
            End Using
        End Sub

        Private Shared Sub DrawExportIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 1.8F)
                g.DrawRectangle(p, 2, 4, 8, 8)
                g.DrawLine(p, 8, 8, 14, 8)
                g.DrawLine(p, 11, 5, 14, 8)
                g.DrawLine(p, 11, 11, 14, 8)
            End Using
        End Sub

        Private Shared Sub DrawUndoIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                g.DrawArc(p, 3, 4, 10, 8, 200, 250)
                g.DrawLine(p, 3, 6, 6, 4)
                g.DrawLine(p, 3, 6, 6, 8)
            End Using
        End Sub

        Private Shared Sub DrawSearchIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                g.DrawEllipse(p, 3, 3, 7, 7)
                g.DrawLine(p, 9, 9, 13, 13)
            End Using
        End Sub

        Private Shared Sub DrawFilterIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 1.8F)
                g.DrawLine(p, 2, 3, 14, 3)
                g.DrawLine(p, 2, 3, 8, 8)
                g.DrawLine(p, 14, 3, 8, 8)
                g.DrawLine(p, 8, 8, 8, 13)
            End Using
        End Sub

        Private Shared Sub DrawArrowIcon(ByVal g As Graphics, ByVal left As Boolean, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                If left Then
                    g.DrawLine(p, 11, 3, 5, 8)
                    g.DrawLine(p, 5, 8, 11, 13)
                Else
                    g.DrawLine(p, 5, 3, 11, 8)
                    g.DrawLine(p, 11, 8, 5, 13)
                End If
            End Using
        End Sub

        Private Shared Sub DrawFirstIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                g.DrawLine(p, 4, 3, 4, 13)
            End Using
            DrawArrowIcon(g, True, color)
        End Sub

        Private Shared Sub DrawLastIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 2.0F)
                g.DrawLine(p, 12, 3, 12, 13)
            End Using
            DrawArrowIcon(g, False, color)
        End Sub

        Private Shared Sub DrawPageIcon(ByVal g As Graphics, ByVal color As Color)
            Using p As New Pen(color, 1.8F)
                g.DrawRectangle(p, 3, 2, 10, 12)
                g.DrawLine(p, 6, 5, 10, 5)
                g.DrawLine(p, 6, 8, 10, 8)
                g.DrawLine(p, 6, 11, 10, 11)
            End Using
        End Sub

        Private Shared Sub DrawDotIcon(ByVal g As Graphics, ByVal color As Color)
            Using b As New SolidBrush(color)
                g.FillEllipse(b, 6, 6, 4, 4)
            End Using
        End Sub

        Private Shared Sub ApplyRibbonButtonVisual(ByVal item As BarButtonItem)
            item.PaintStyle = BarItemPaintStyle.CaptionGlyph
            item.RibbonStyle = RibbonItemStyles.SmallWithText Or RibbonItemStyles.Large
        End Sub

        Private Shared Sub ApplyRibbonEditVisual(ByVal item As BarEditItem)
            item.PaintStyle = BarItemPaintStyle.CaptionGlyph
            item.RibbonStyle = RibbonItemStyles.SmallWithText Or RibbonItemStyles.Large
        End Sub

        Protected Overridable Function BuildFormTitle() As String
            Return "Busqueda"
        End Function

        Protected Overridable Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
        End Sub

        Protected Overridable Function BuildSampleData() As DataTable
            Return New DataTable()
        End Function

        Protected Overridable Function CargarPaginaServidor(ByVal request As SearchPageRequest) As SearchPageResult
            Return Nothing
        End Function

        Protected Overridable Sub OnNuevoClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            XtraMessageBox.Show(Me, $"Nuevo registro en {BuildEndpoint()}", "Operacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Protected Overridable Sub OnEditarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            XtraMessageBox.Show(Me, $"Editar registro en {BuildEndpoint()}", "Operacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Protected Overridable Sub OnRefrescarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            LoadGridData(False)
        End Sub

        Protected Overridable Sub OnCerrarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Close()
        End Sub

        Protected Overridable Function BuildEndpoint() As String
            Return "api/v1/base/search/listar"
        End Function

        Private Sub OnFirstShown(ByVal sender As Object, ByVal e As EventArgs)
            If _isFirstLoadDone Then Return
            _isFirstLoadDone = True

            If IsDesignerHosted() Then Return

            RestoreLayoutFromApi(False, False)
            LoadGridData(False)
        End Sub

        Private Sub LoadGridData(ByVal resetFilters As Boolean)
            If UsaPaginacionServidor AndAlso CanUseServerApi() Then
                Try
                    LoadServerGrid(resetFilters)
                    _statusInfo.Caption = $"Origen: {BuildEndpoint()} | API"
                    Return
                Catch
                    _statusInfo.Caption = $"Origen: {BuildEndpoint()} | Fallback local por error API"
                End Try
            End If

            LoadLocalGrid(resetFilters)
        End Sub

        Private Sub LoadServerGrid(ByVal resetFilters As Boolean)
            If resetFilters Then _currentPage = 1

            Dim request As SearchPageRequest = BuildServerRequest()
            Dim result = CargarPaginaServidor(request)
            If result Is Nothing OrElse result.Data Is Nothing Then
                Throw New InvalidOperationException("No se recibio resultado paginado desde el servidor.")
            End If

            Dim pageSize = Math.Max(5, result.Size)
            _currentPage = If(result.Page > 0, result.Page, request.Page)
            _totalRows = Math.Max(0, result.Total)
            _totalPages = Math.Max(1, CInt(Math.Ceiling(_totalRows / CDbl(pageSize))))
            If _currentPage > _totalPages Then _currentPage = _totalPages

            _isRefreshingGrid = True
            Grid.DataSource = result.Data
            _isRefreshingGrid = False

            If CmbCampoFiltro.Properties.Items.Count = 0 Then PopulateFilterFieldsFromGrid()
            UpdatePagingSummary(result.Data.Rows.Count)
            If View.RowCount > 0 Then View.FocusedRowHandle = 0
        End Sub

        Private Sub LoadLocalGrid(ByVal resetFilters As Boolean)
            _sourceData = BuildSampleData()
            If _sourceData Is Nothing Then _sourceData = New DataTable()

            If resetFilters Then
                PopulateFilterFieldsFromData(_sourceData)
                _currentPage = 1
            End If

            ApplyLocalFilteringAndPaging(resetFilters)
        End Sub

        Private Function BuildServerRequest() As SearchPageRequest
            Dim selectedField = GetSelectedFilterField()
            Dim filterField = If(selectedField = "*", String.Empty, selectedField)

            Dim sortBy As String = String.Empty
            Dim sortDirection As String = "ASC"
            If View.SortInfo.Count > 0 Then
                Dim sortItem As GridColumnSortInfo = View.SortInfo(0)
                sortBy = sortItem.Column.FieldName
                sortDirection = If(sortItem.SortOrder = ColumnSortOrder.Descending, "DESC", "ASC")
            End If

            Return New SearchPageRequest With {
                .Page = Math.Max(1, _currentPage),
                .Size = GetPageSize(),
                .SortBy = sortBy,
                .SortDirection = sortDirection,
                .Filter = TxtFiltro.Text.Trim(),
                .FilterField = filterField,
                .IdTenant = _sessionContext.IdTenant,
                .IdEmpresa = _sessionContext.IdEmpresa
            }
        End Function

        Private Sub PopulateFilterFieldsFromData(ByVal source As DataTable)
            CmbCampoFiltro.Properties.Items.Clear()
            CmbCampoFiltro.Properties.Items.Add("(Todos)")
            For Each column As DataColumn In source.Columns
                CmbCampoFiltro.Properties.Items.Add(column.ColumnName)
            Next
            CmbCampoFiltro.SelectedIndex = 0

            _repoCampoFiltro.Items.Clear()
            For Each item In CmbCampoFiltro.Properties.Items
                _repoCampoFiltro.Items.Add(item)
            Next

            SyncRibbonFromState()
        End Sub

        Private Sub PopulateFilterFieldsFromGrid()
            CmbCampoFiltro.Properties.Items.Clear()
            CmbCampoFiltro.Properties.Items.Add("(Todos)")
            For Each column As GridColumn In View.Columns
                If Not String.IsNullOrWhiteSpace(column.FieldName) Then
                    CmbCampoFiltro.Properties.Items.Add(column.FieldName)
                End If
            Next
            If CmbCampoFiltro.Properties.Items.Count > 0 Then CmbCampoFiltro.SelectedIndex = 0

            _repoCampoFiltro.Items.Clear()
            For Each item In CmbCampoFiltro.Properties.Items
                _repoCampoFiltro.Items.Add(item)
            Next

            SyncRibbonFromState()
        End Sub

        Private Sub OnBuscarRibbonClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OnApplyFilterClick(sender, EventArgs.Empty)
        End Sub

        Private Sub OnLimpiarRibbonClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OnClearFilterClick(sender, EventArgs.Empty)
        End Sub

        Private Sub OnPaginaAnteriorRibbonClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OnPrevPageClick(sender, EventArgs.Empty)
        End Sub

        Private Sub OnPaginaSiguienteRibbonClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OnNextPageClick(sender, EventArgs.Empty)
        End Sub

        Private Sub OnCampoFiltroRibbonChanged(ByVal sender As Object, ByVal e As EventArgs)
            If _isSyncingRibbon Then Return
            If CmbCampoFiltro.Properties.Items.Count = 0 Then Return

            Dim value = NormalizeFilterFieldDisplay(Convert.ToString(_barCampoFiltro.EditValue))
            If Not CmbCampoFiltro.Properties.Items.Contains(value) Then
                value = "(Todos)"
            End If

            CmbCampoFiltro.SelectedItem = value
            _currentPage = 1
            LoadGridData(False)
        End Sub

        Private Sub OnTextoBusquedaRibbonChanged(ByVal sender As Object, ByVal e As EventArgs)
            TxtFiltro.Text = Convert.ToString(_barBusqueda.EditValue)
            If _isSyncingRibbon Then Return

            _searchDebounceTimer.Stop()
            _searchDebounceTimer.Start()
        End Sub

        Private Sub OnTamanoPaginaRibbonChanged(ByVal sender As Object, ByVal e As EventArgs)
            If _isSyncingRibbon Then Return

            Dim parsedSize As Integer
            If Not Integer.TryParse(Convert.ToString(_barTamanoPagina.EditValue), parsedSize) Then
                parsedSize = GetPageSize()
            End If

            parsedSize = Math.Max(5, parsedSize)
            SpnTamanoPagina.EditValue = parsedSize
            _currentPage = 1
            LoadGridData(False)
        End Sub

        Private Sub OnSearchDebounceTick(ByVal sender As Object, ByVal e As EventArgs)
            _searchDebounceTimer.Stop()
            If _isSyncingRibbon Then Return

            _currentPage = 1
            LoadGridData(False)
        End Sub

        Private Sub OnApplyFilterClick(ByVal sender As Object, ByVal e As EventArgs)
            _searchDebounceTimer.Stop()
            TxtFiltro.Text = Convert.ToString(_barBusqueda.EditValue)

            If UsaPaginacionServidor Then
                LoadGridData(True)
            Else
                ApplyLocalFilteringAndPaging(True)
            End If
        End Sub

        Private Sub OnClearFilterClick(ByVal sender As Object, ByVal e As EventArgs)
            TxtFiltro.Text = String.Empty
            If CmbCampoFiltro.Properties.Items.Count > 0 Then CmbCampoFiltro.SelectedIndex = 0
            SyncRibbonFromState()
            _searchDebounceTimer.Stop()

            If UsaPaginacionServidor Then
                LoadGridData(True)
            Else
                ApplyLocalFilteringAndPaging(True)
            End If
        End Sub

        Private Sub OnPrevPageClick(ByVal sender As Object, ByVal e As EventArgs)
            If _currentPage <= 1 Then Return
            _currentPage -= 1
            LoadGridData(False)
        End Sub

        Private Sub OnNextPageClick(ByVal sender As Object, ByVal e As EventArgs)
            If _currentPage >= _totalPages Then Return
            _currentPage += 1
            LoadGridData(False)
        End Sub

        Private Sub OnPageSizeChanged(ByVal sender As Object, ByVal e As EventArgs)
            _currentPage = 1
            LoadGridData(False)
        End Sub

        Private Sub OnTxtFiltroKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                _currentPage = 1
                LoadGridData(False)
            End If
        End Sub

        Private Sub OnGridEndSorting(ByVal sender As Object, ByVal e As EventArgs)
            If _isRefreshingGrid Then Return
            LoadGridData(False)
        End Sub

        Private Sub OnPrimerRegistroClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _totalRows = 0 Then Return
            _currentPage = 1
            LoadGridData(False)
            If View.RowCount > 0 Then View.FocusedRowHandle = 0
        End Sub

        Private Sub OnUltimoRegistroClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _totalRows = 0 Then Return
            _currentPage = _totalPages
            LoadGridData(False)
            If View.RowCount > 0 Then View.FocusedRowHandle = View.RowCount - 1
        End Sub

        Private Sub OnRegistroAnteriorClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If View.RowCount = 0 Then Return
            Dim currentRow = View.FocusedRowHandle
            If currentRow > 0 Then
                View.FocusedRowHandle = currentRow - 1
                Return
            End If

            If _currentPage > 1 Then
                _currentPage -= 1
                LoadGridData(False)
                If View.RowCount > 0 Then View.FocusedRowHandle = View.RowCount - 1
            End If
        End Sub

        Private Sub OnRegistroSiguienteClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If View.RowCount = 0 Then Return
            Dim currentRow = View.FocusedRowHandle
            If currentRow < View.RowCount - 1 Then
                View.FocusedRowHandle = currentRow + 1
                Return
            End If

            If _currentPage < _totalPages Then
                _currentPage += 1
                LoadGridData(False)
                If View.RowCount > 0 Then View.FocusedRowHandle = 0
            End If
        End Sub

        Private Sub ApplyLocalFilteringAndPaging(ByVal resetPage As Boolean)
            If _sourceData Is Nothing Then _sourceData = New DataTable()
            If resetPage Then _currentPage = 1

            Dim filteredData = FilterData(_sourceData)
            ApplySort(filteredData)

            _totalRows = filteredData.Rows.Count
            Dim pageSize = GetPageSize()
            _totalPages = Math.Max(1, CInt(Math.Ceiling(_totalRows / CDbl(pageSize))))
            If _currentPage > _totalPages Then _currentPage = _totalPages

            Dim pageData = GetPageData(filteredData, _currentPage, pageSize)
            _isRefreshingGrid = True
            Grid.DataSource = pageData
            _isRefreshingGrid = False

            UpdatePagingSummary(pageData.Rows.Count)
            If View.RowCount > 0 Then View.FocusedRowHandle = 0
        End Sub

        Private Function FilterData(ByVal source As DataTable) As DataTable
            If source.Rows.Count = 0 OrElse String.IsNullOrWhiteSpace(TxtFiltro.Text) Then Return source.Copy()

            Dim selectedField = GetSelectedFilterField()
            Dim filterValue = EscapeLikeValue(TxtFiltro.Text.Trim())
            Dim dv As New DataView(source)

            If selectedField = "*" Then
                Dim expressions As New List(Of String)()
                For Each column As DataColumn In source.Columns
                    expressions.Add($"Convert([{EscapeColumnName(column.ColumnName)}], 'System.String') LIKE '%{filterValue}%'")
                Next
                dv.RowFilter = String.Join(" OR ", expressions)
            Else
                dv.RowFilter = $"Convert([{EscapeColumnName(selectedField)}], 'System.String') LIKE '%{filterValue}%'"
            End If

            Return dv.ToTable()
        End Function

        Private Sub ApplySort(ByVal table As DataTable)
            If table.Rows.Count = 0 Then Return
            Dim sortExpression = BuildSortExpression()
            If String.IsNullOrWhiteSpace(sortExpression) Then Return

            Dim dv As New DataView(table) With {.Sort = sortExpression}
            Dim sorted = dv.ToTable()
            table.Clear()
            For Each row As DataRow In sorted.Rows
                table.ImportRow(row)
            Next
        End Sub

        Private Function BuildSortExpression() As String
            If View.SortInfo.Count = 0 Then Return String.Empty

            Dim parts As New List(Of String)()
            For Each sortItem As GridColumnSortInfo In View.SortInfo
                Dim direction = If(sortItem.SortOrder = ColumnSortOrder.Descending, "DESC", "ASC")
                parts.Add($"[{EscapeColumnName(sortItem.Column.FieldName)}] {direction}")
            Next
            Return String.Join(", ", parts)
        End Function

        Private Function GetPageData(ByVal source As DataTable, ByVal page As Integer, ByVal pageSize As Integer) As DataTable
            Dim result = source.Clone()
            If source.Rows.Count = 0 Then Return result

            Dim startIndex = (page - 1) * pageSize
            Dim endIndex = Math.Min(startIndex + pageSize - 1, source.Rows.Count - 1)
            If startIndex > source.Rows.Count - 1 Then Return result

            For index = startIndex To endIndex
                result.ImportRow(source.Rows(index))
            Next

            Return result
        End Function

        Private Sub UpdatePagingSummary(ByVal rowsInPage As Integer)
            Dim startNumber = If(rowsInPage = 0, 0, ((_currentPage - 1) * GetPageSize()) + 1)
            Dim endNumber = If(rowsInPage = 0, 0, startNumber + rowsInPage - 1)

            _barResumenPagina.Caption = $"Pag {_currentPage}/{_totalPages}"
            _statusPaginacion.Caption = $"Registros: {_totalRows} | Mostrando {startNumber}-{endNumber}"

            _barPaginaAnterior.Enabled = _currentPage > 1
            _barPaginaSiguiente.Enabled = _currentPage < _totalPages
        End Sub

        Private Function GetSelectedFilterField() As String
            Dim selected = If(CmbCampoFiltro.SelectedItem IsNot Nothing, CmbCampoFiltro.SelectedItem.ToString(), "(Todos)")
            Return If(String.Equals(selected, "(Todos)", StringComparison.Ordinal), "*", selected)
        End Function

        Private Function GetPageSize() As Integer
            Dim pageSize = Convert.ToInt32(SpnTamanoPagina.Value)
            Return Math.Max(5, pageSize)
        End Function
        Private Sub OnGuardarVistaClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            SaveLayoutToApi(True)
        End Sub

        Private Sub OnRestaurarVistaClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            RestoreLayoutFromApi(True, True)
        End Sub

        Private Sub OnExportarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Dim options = PromptExportOptions()
            If options Is Nothing Then Return
            ExecuteExport(options)
        End Sub

        Private Function PromptExportOptions() As GridExportOptions
            Using frm As New XtraForm()
                frm.Text = "Opciones de exportacion"
                frm.StartPosition = FormStartPosition.CenterParent
                frm.FormBorderStyle = FormBorderStyle.FixedDialog
                frm.MaximizeBox = False
                frm.MinimizeBox = False
                frm.ShowInTaskbar = False
                frm.Width = 520
                frm.Height = 280

                Dim lblNombre As New LabelControl() With {.Text = "Nombre del reporte", .Location = New Point(20, 22)}
                Dim txtNombre As New TextEdit() With {.Location = New Point(20, 44), .Width = 460, .Text = BuildFormTitle()}

                Dim rgFormatos As New RadioGroup() With {.Location = New Point(20, 84), .Width = 460, .Height = 112}
                rgFormatos.Properties.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem(CInt(GridExportFormat.Excel2003), "Microsoft Excel 2003"))
                rgFormatos.Properties.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem(CInt(GridExportFormat.Excel2007Plus), "Microsoft Excel 2007+"))
                rgFormatos.Properties.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem(CInt(GridExportFormat.Word), "Microsoft Word"))
                rgFormatos.Properties.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem(CInt(GridExportFormat.TextoPlano), "Texto Plano"))
                rgFormatos.Properties.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem(CInt(GridExportFormat.Pdf), "Acrobat Reader (PDF)"))
                rgFormatos.Properties.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem(CInt(GridExportFormat.VistaPrevia), "Vista previa"))
                rgFormatos.EditValue = CInt(GridExportFormat.VistaPrevia)

                Dim btnAceptar As New SimpleButton() With {.Text = "Aceptar", .DialogResult = DialogResult.OK, .Location = New Point(292, 210), .Width = 90}
                Dim btnCancelar As New SimpleButton() With {.Text = "Cancelar", .DialogResult = DialogResult.Cancel, .Location = New Point(390, 210), .Width = 90}

                frm.Controls.Add(lblNombre)
                frm.Controls.Add(txtNombre)
                frm.Controls.Add(rgFormatos)
                frm.Controls.Add(btnAceptar)
                frm.Controls.Add(btnCancelar)
                frm.AcceptButton = btnAceptar
                frm.CancelButton = btnCancelar

                If frm.ShowDialog(Me) <> DialogResult.OK Then Return Nothing

                Dim formatValue = Convert.ToInt32(rgFormatos.EditValue)
                Return New GridExportOptions With {
                    .NombreReporte = If(String.IsNullOrWhiteSpace(txtNombre.Text), BuildFormTitle(), txtNombre.Text.Trim()),
                    .Formato = CType(formatValue, GridExportFormat)
                }
            End Using
        End Function

        Private Sub ExecuteExport(ByVal options As GridExportOptions)
            Try
                Using link = CreatePrintableLink(options.NombreReporte)
                    If options.Formato = GridExportFormat.VistaPrevia Then
                        link.ShowRibbonPreviewDialog(DevExpress.LookAndFeel.UserLookAndFeel.Default)
                        Return
                    End If

                    Using dialog As New SaveFileDialog()
                        dialog.Title = "Guardar exportacion"
                        dialog.FileName = SanitizeFileName(options.NombreReporte)
                        dialog.OverwritePrompt = True

                        Select Case options.Formato
                            Case GridExportFormat.Excel2003
                                dialog.Filter = "Microsoft Excel 2003 (*.xls)|*.xls"
                                dialog.DefaultExt = "xls"
                            Case GridExportFormat.Excel2007Plus
                                dialog.Filter = "Microsoft Excel 2007+ (*.xlsx)|*.xlsx"
                                dialog.DefaultExt = "xlsx"
                            Case GridExportFormat.Word
                                dialog.Filter = "Microsoft Word (*.docx)|*.docx"
                                dialog.DefaultExt = "docx"
                            Case GridExportFormat.TextoPlano
                                dialog.Filter = "Texto plano (*.txt)|*.txt"
                                dialog.DefaultExt = "txt"
                            Case GridExportFormat.Pdf
                                dialog.Filter = "Acrobat Reader (*.pdf)|*.pdf"
                                dialog.DefaultExt = "pdf"
                        End Select

                        If dialog.ShowDialog(Me) <> DialogResult.OK Then Return

                        Select Case options.Formato
                            Case GridExportFormat.Excel2003
                                link.ExportToXls(dialog.FileName)
                            Case GridExportFormat.Excel2007Plus
                                link.ExportToXlsx(dialog.FileName)
                            Case GridExportFormat.Word
                                link.ExportToDocx(dialog.FileName)
                            Case GridExportFormat.TextoPlano
                                link.ExportToText(dialog.FileName)
                            Case GridExportFormat.Pdf
                                link.ExportToPdf(dialog.FileName)
                        End Select
                    End Using
                End Using

                XtraMessageBox.Show(Me, "Exportacion completada.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Function CreatePrintableLink(ByVal reportTitle As String) As PrintableComponentLink
            Dim printingSystem As New PrintingSystem()
            Dim link As New PrintableComponentLink(printingSystem) With {
                .Component = Grid,
                .Landscape = True,
                .PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4
            }

            Dim headerFooter As New PageHeaderFooter()
            headerFooter.Header.Content.AddRange(New String() {
                AppSettingsProvider.GetReportCompanyName(),
                reportTitle,
                String.Empty
            })
            headerFooter.Footer.Content.AddRange(New String() {
                "Fecha y Hora: [Date Printed] [Time Printed]",
                $"Usuario: {_sessionContext.Usuario}",
                "Paginas: [Page # of Pages #]"
            })
            link.PageHeaderFooter = headerFooter

            AddHandler link.CreateMarginalHeaderArea,
                Sub(sender, args)
                    Dim logoPath = AppSettingsProvider.GetReportLogoPath()
                    If String.IsNullOrWhiteSpace(logoPath) OrElse Not File.Exists(logoPath) Then Return
                    Using logoImage As Image = Image.FromFile(logoPath)
                        args.Graph.DrawImage(logoImage, New RectangleF(8.0F, 8.0F, 64.0F, 40.0F))
                    End Using
                End Sub

            link.CreateDocument()
            Return link
        End Function

        Private Sub SaveLayoutToApi(ByVal notify As Boolean)
            If Not CanPersistLayoutToApi() Then Return

            Dim payload = BuildLayoutPayloadWithFallback()
            If String.IsNullOrWhiteSpace(payload) Then
                If notify Then
                    XtraMessageBox.Show(Me, "No fue posible serializar el layout dentro del limite de almacenamiento en BD.", "Vista", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
                Return
            End If

            Dim request As New GuardarLayoutUiRequestDto With {
                .IdUsuario = _sessionContext.IdUsuario.Value,
                .IdTenant = _sessionContext.IdTenant.Value,
                .IdEmpresa = _sessionContext.IdEmpresa,
                .CodigoLayout = BuildEndpoint(),
                .LayoutPayload = payload
            }

            Try
                _apiClient.PostAsync(Of GuardarLayoutUiRequestDto, GuardarLayoutUiResponseDto)("api/v1/seguridad/layout_ui/crear", request).GetAwaiter().GetResult()
                If notify Then XtraMessageBox.Show(Me, "Vista guardada correctamente.", "Vista", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch
                If notify Then XtraMessageBox.Show(Me, "No fue posible guardar la vista en la base de datos.", "Vista", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub

        Private Sub RestoreLayoutFromApi(ByVal reloadAfterApply As Boolean, ByVal notify As Boolean)
            If Not CanPersistLayoutToApi() Then Return

            Dim endpointBuilder As New StringBuilder()
            endpointBuilder.Append("api/v1/seguridad/layout_ui?")
            endpointBuilder.Append("idUsuario=").Append(_sessionContext.IdUsuario.Value)
            endpointBuilder.Append("&idTenant=").Append(_sessionContext.IdTenant.Value)
            If _sessionContext.IdEmpresa.HasValue Then endpointBuilder.Append("&idEmpresa=").Append(_sessionContext.IdEmpresa.Value)
            endpointBuilder.Append("&codigoLayout=").Append(Uri.EscapeDataString(BuildEndpoint()))

            Try
                Dim response = _apiClient.GetAsync(Of ObtenerLayoutUiResponseDto)(endpointBuilder.ToString()).GetAwaiter().GetResult()
                If response Is Nothing OrElse Not response.Encontrado OrElse String.IsNullOrWhiteSpace(response.LayoutPayload) Then
                    If notify Then XtraMessageBox.Show(Me, "No existe una vista guardada para este modulo.", "Vista", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                Dim state = DeserializeLayoutState(response.LayoutPayload)
                If state Is Nothing Then
                    If notify Then XtraMessageBox.Show(Me, "La vista guardada no pudo deserializarse.", "Vista", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ApplyLayoutState(state)
                If reloadAfterApply Then
                    _currentPage = 1
                    LoadGridData(False)
                End If

                If notify Then XtraMessageBox.Show(Me, "Vista restaurada.", "Vista", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch
                If notify Then XtraMessageBox.Show(Me, "No fue posible recuperar la vista desde la base de datos.", "Vista", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub
        Private Sub OnFormClosingPersistLayout(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            If IsDesignerHosted() Then Return
            SaveLayoutToApi(False)
        End Sub

        Private Function CanPersistLayoutToApi() As Boolean
            Return _apiClient IsNot Nothing AndAlso _sessionContext IsNot Nothing AndAlso _sessionContext.TieneIdentidadValida AndAlso Not IsDesignerHosted()
        End Function

        Private Function CanUseServerApi() As Boolean
            Return _apiClient IsNot Nothing AndAlso Not IsDesignerHosted()
        End Function

        Private Function BuildLayoutPayloadWithFallback() As String
            Dim attempts As New List(Of KeyValuePair(Of Boolean, Boolean)) From {
                New KeyValuePair(Of Boolean, Boolean)(True, True),
                New KeyValuePair(Of Boolean, Boolean)(False, True),
                New KeyValuePair(Of Boolean, Boolean)(False, False)
            }

            For Each attempt In attempts
                Dim state = BuildLayoutState(attempt.Key, attempt.Value)
                Dim payload = SerializeLayoutState(state)
                If Not String.IsNullOrWhiteSpace(payload) AndAlso payload.Length <= 300 Then Return payload
            Next

            Return String.Empty
        End Function

        Private Function BuildLayoutState(ByVal includeWidths As Boolean, ByVal includeFilter As Boolean) As PersistedLayoutState
            Dim state As New PersistedLayoutState With {
                .v = 1,
                .p = GetPageSize(),
                .f = If(includeFilter, TxtFiltro.Text.Trim(), String.Empty),
                .ff = GetSelectedFilterField(),
                .cols = New List(Of PersistedLayoutColumnState)()
            }

            For Each column As GridColumn In View.Columns
                If String.IsNullOrWhiteSpace(column.FieldName) Then Continue For

                state.cols.Add(New PersistedLayoutColumnState With {
                    .n = column.FieldName,
                    .i = column.VisibleIndex,
                    .w = If(includeWidths, column.Width, 0),
                    .vis = column.Visible,
                    .s = GetSortCode(column.SortOrder)
                })
            Next

            Return state
        End Function

        Private Shared Function SerializeLayoutState(ByVal state As PersistedLayoutState) As String
            Dim json = JsonSerializer.Serialize(state)
            Dim rawBytes = Encoding.UTF8.GetBytes(json)

            Using output As New MemoryStream()
                Using gzip As New GZipStream(output, CompressionMode.Compress)
                    gzip.Write(rawBytes, 0, rawBytes.Length)
                End Using

                Return Convert.ToBase64String(output.ToArray())
            End Using
        End Function

        Private Shared Function DeserializeLayoutState(ByVal payload As String) As PersistedLayoutState
            Try
                Dim compressed = Convert.FromBase64String(payload)
                Using input As New MemoryStream(compressed)
                    Using gzip As New GZipStream(input, CompressionMode.Decompress)
                        Using decompressed As New MemoryStream()
                            gzip.CopyTo(decompressed)
                            Dim json = Encoding.UTF8.GetString(decompressed.ToArray())
                            Return JsonSerializer.Deserialize(Of PersistedLayoutState)(json)
                        End Using
                    End Using
                End Using
            Catch
                Return Nothing
            End Try
        End Function

        Private Sub ApplyLayoutState(ByVal state As PersistedLayoutState)
            If state Is Nothing Then Return

            If state.p >= 5 Then SpnTamanoPagina.EditValue = state.p
            TxtFiltro.Text = If(state.f, String.Empty)

            If Not String.IsNullOrWhiteSpace(state.ff) Then
                Dim normalizedField = If(state.ff = "*", "(Todos)", state.ff)
                If CmbCampoFiltro.Properties.Items.Contains(normalizedField) Then
                    CmbCampoFiltro.SelectedItem = normalizedField
                End If
            End If

            View.BeginUpdate()
            View.BeginSort()
            Try
                View.ClearSorting()

                For Each colState In state.cols
                    Dim col = View.Columns.ColumnByFieldName(colState.n)
                    If col Is Nothing Then Continue For

                    col.Visible = colState.vis
                    If col.Visible Then col.VisibleIndex = Math.Max(0, colState.i)
                    If colState.w > 0 Then col.Width = colState.w

                    Select Case colState.s
                        Case "A"
                            col.SortOrder = ColumnSortOrder.Ascending
                        Case "D"
                            col.SortOrder = ColumnSortOrder.Descending
                        Case Else
                            col.SortOrder = ColumnSortOrder.None
                    End Select
                Next
            Finally
                View.EndSort()
                View.EndUpdate()
            End Try

            SyncRibbonFromState()
        End Sub

        Private Shared Function GetSortCode(ByVal sortOrder As ColumnSortOrder) As String
            Select Case sortOrder
                Case ColumnSortOrder.Ascending
                    Return "A"
                Case ColumnSortOrder.Descending
                    Return "D"
                Case Else
                    Return "N"
            End Select
        End Function

        Private Shared Function SanitizeFileName(ByVal name As String) As String
            Dim invalid = Path.GetInvalidFileNameChars()
            Dim safeName = name
            For Each ch In invalid
                safeName = safeName.Replace(ch, "_"c)
            Next
            Return safeName
        End Function

        Private Shared Function EscapeLikeValue(ByVal rawValue As String) As String
            Return rawValue.
                Replace("'", "''").
                Replace("[", "[[]").
                Replace("%", "[%]").
                Replace("*", "[*]").
                Replace("_", "[_]")
        End Function

        Private Shared Function EscapeColumnName(ByVal columnName As String) As String
            Return columnName.Replace("]", "]]")
        End Function

        Private Shared Function IsDesignerHosted() As Boolean
            Return LicenseManager.UsageMode = LicenseUsageMode.Designtime
        End Function


    End Class
End Namespace


