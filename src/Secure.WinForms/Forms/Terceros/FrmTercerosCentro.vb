
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout
Imports DevExpress.XtraTab
Imports Secure.Platform.Contracts.Dtos.Tercero
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Terceros
    ''' <summary>
    ''' Centro unificado de mantenimiento para terceros y sus datos relacionados.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmTercerosCentro
        Inherits XtraForm

        Private NotInheritable Class IdResponse
            Public Property Id As Long
        End Class

        Private NotInheritable Class ComboIntItem
            Public Property Id As Integer
            Public Property Nombre As String = String.Empty

            Public Overrides Function ToString() As String
                Return Nombre
            End Function
        End Class

        Private ReadOnly _apiClient As ApiClient
        Private ReadOnly _sessionContext As UserSessionContext

        Private ReadOnly _ribbon As RibbonControl
        Private ReadOnly _statusBar As RibbonStatusBar
        Private ReadOnly _statusInfo As BarStaticItem

        Private ReadOnly _btnSeccionGeneral As BarButtonItem
        Private ReadOnly _btnSeccionIdentificaciones As BarButtonItem
        Private ReadOnly _btnSeccionDirecciones As BarButtonItem
        Private ReadOnly _btnSeccionContactos As BarButtonItem
        Private ReadOnly _btnSeccionCuentas As BarButtonItem
        Private ReadOnly _btnSeccionRoles As BarButtonItem

        Private ReadOnly _btnNuevo As BarButtonItem
        Private ReadOnly _btnGuardar As BarButtonItem
        Private ReadOnly _btnRefrescar As BarButtonItem
        Private ReadOnly _btnDesactivar As BarButtonItem
        Private ReadOnly _btnLimpiar As BarButtonItem

        Private ReadOnly _mainLayout As LayoutControl
        Private ReadOnly _splitMain As SplitContainerControl

        Private ReadOnly _txtBuscar As TextEdit
        Private ReadOnly _gridTerceros As GridControl
        Private ReadOnly _viewTerceros As GridView

        Private ReadOnly _editorLayout As LayoutControl
        Private ReadOnly _txtCodigo As TextEdit
        Private ReadOnly _cmbTipoPersona As ComboBoxEdit
        Private ReadOnly _txtNombre As TextEdit
        Private ReadOnly _txtSegundoNombre As TextEdit
        Private ReadOnly _txtApellido As TextEdit
        Private ReadOnly _txtSegundoApellido As TextEdit
        Private ReadOnly _txtRazonSocial As TextEdit
        Private ReadOnly _txtNombreComercial As TextEdit
        Private ReadOnly _dteFechaNacimiento As DateEdit
        Private ReadOnly _dteFechaConstitucion As DateEdit
        Private ReadOnly _chkActivo As CheckEdit

        Private ReadOnly _tabsDetalle As XtraTabControl
        Private ReadOnly _tabIdentificaciones As XtraTabPage
        Private ReadOnly _tabDirecciones As XtraTabPage
        Private ReadOnly _tabContactos As XtraTabPage
        Private ReadOnly _tabCuentas As XtraTabPage
        Private ReadOnly _tabRoles As XtraTabPage

        Private ReadOnly _gridIdentificaciones As GridControl
        Private ReadOnly _viewIdentificaciones As GridView
        Private ReadOnly _gridDirecciones As GridControl
        Private ReadOnly _viewDirecciones As GridView
        Private ReadOnly _gridContactos As GridControl
        Private ReadOnly _viewContactos As GridView
        Private ReadOnly _gridCuentas As GridControl
        Private ReadOnly _viewCuentas As GridView
        Private ReadOnly _gridRoles As GridControl
        Private ReadOnly _viewRoles As GridView

        Private ReadOnly _tblTerceros As DataTable
        Private ReadOnly _tblIdentificaciones As DataTable
        Private ReadOnly _tblDirecciones As DataTable
        Private ReadOnly _tblContactos As DataTable
        Private ReadOnly _tblCuentas As DataTable
        Private ReadOnly _tblRoles As DataTable

        Private _tiposPersona As List(Of TipoPersonaDto)
        Private _terceros As List(Of TerceroDto)
        Private _identificaciones As List(Of IdentificacionTerceroDto)
        Private _direcciones As List(Of DireccionTerceroDto)
        Private _contactos As List(Of ContactoTerceroDto)
        Private _cuentas As List(Of CuentaBancariaTerceroDto)
        Private _roles As List(Of TerceroRolDto)

        Private _editingTerceroId As Long?
        Private _firstLoadDone As Boolean
        Private _loadingUi As Boolean

        Public Sub New()
            Me.New(New ApiClient(AppSettingsProvider.GetApiBaseUrl()), UserSessionContext.CrearDiseno())
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            _apiClient = apiClient
            _sessionContext = If(sessionContext, UserSessionContext.CrearDiseno())

            _ribbon = New RibbonControl()
            _statusBar = New RibbonStatusBar()
            _statusInfo = New BarStaticItem()

            _btnSeccionGeneral = New BarButtonItem() With {.Caption = "Datos generales"}
            _btnSeccionIdentificaciones = New BarButtonItem() With {.Caption = "Identificaciones"}
            _btnSeccionDirecciones = New BarButtonItem() With {.Caption = "Direcciones"}
            _btnSeccionContactos = New BarButtonItem() With {.Caption = "Contactos"}
            _btnSeccionCuentas = New BarButtonItem() With {.Caption = "Cuentas"}
            _btnSeccionRoles = New BarButtonItem() With {.Caption = "Roles"}

            _btnNuevo = New BarButtonItem() With {.Caption = "Nuevo"}
            _btnGuardar = New BarButtonItem() With {.Caption = "Guardar"}
            _btnRefrescar = New BarButtonItem() With {.Caption = "Refrescar"}
            _btnDesactivar = New BarButtonItem() With {.Caption = "Desactivar"}
            _btnLimpiar = New BarButtonItem() With {.Caption = "Limpiar"}

            _mainLayout = New LayoutControl()
            _splitMain = New SplitContainerControl()

            _txtBuscar = New TextEdit()
            _gridTerceros = New GridControl()
            _viewTerceros = New GridView()

            _editorLayout = New LayoutControl()
            _txtCodigo = New TextEdit()
            _cmbTipoPersona = New ComboBoxEdit()
            _txtNombre = New TextEdit()
            _txtSegundoNombre = New TextEdit()
            _txtApellido = New TextEdit()
            _txtSegundoApellido = New TextEdit()
            _txtRazonSocial = New TextEdit()
            _txtNombreComercial = New TextEdit()
            _dteFechaNacimiento = New DateEdit()
            _dteFechaConstitucion = New DateEdit()
            _chkActivo = New CheckEdit()

            _tabsDetalle = New XtraTabControl()
            _tabIdentificaciones = New XtraTabPage() With {.Text = "Identificaciones"}
            _tabDirecciones = New XtraTabPage() With {.Text = "Direcciones"}
            _tabContactos = New XtraTabPage() With {.Text = "Contactos"}
            _tabCuentas = New XtraTabPage() With {.Text = "Cuentas bancarias"}
            _tabRoles = New XtraTabPage() With {.Text = "Roles"}

            _gridIdentificaciones = New GridControl()
            _viewIdentificaciones = New GridView()
            _gridDirecciones = New GridControl()
            _viewDirecciones = New GridView()
            _gridContactos = New GridControl()
            _viewContactos = New GridView()
            _gridCuentas = New GridControl()
            _viewCuentas = New GridView()
            _gridRoles = New GridControl()
            _viewRoles = New GridView()

            _tblTerceros = CreateTercerosTable()
            _tblIdentificaciones = CreateIdentificacionesTable()
            _tblDirecciones = CreateDireccionesTable()
            _tblContactos = CreateContactosTable()
            _tblCuentas = CreateCuentasTable()
            _tblRoles = CreateRolesTable()

            _tiposPersona = New List(Of TipoPersonaDto)()
            _terceros = New List(Of TerceroDto)()
            _identificaciones = New List(Of IdentificacionTerceroDto)()
            _direcciones = New List(Of DireccionTerceroDto)()
            _contactos = New List(Of ContactoTerceroDto)()
            _cuentas = New List(Of CuentaBancariaTerceroDto)()
            _roles = New List(Of TerceroRolDto)()

            InitializeComponent()
            WireEvents()
        End Sub

        Public ReadOnly Property ModuleRibbon As RibbonControl
            Get
                Return _ribbon
            End Get
        End Property

        Public ReadOnly Property ModuleTitle As String
            Get
                Return "Centro de Terceros"
            End Get
        End Property

        Public Sub SetRibbonHostMode(ByVal hosted As Boolean)
            _ribbon.Visible = Not hosted
        End Sub

        Private Sub InitializeComponent()
            SuspendLayout()
            AutoScaleDimensions = New SizeF(96.0F, 96.0F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(1366, 860)
            FormBorderStyle = FormBorderStyle.None
            Name = "FrmTercerosCentro"
            Text = "Centro de Terceros"

            ConfigureRibbon()
            ConfigureBody()
            ConfigureStatusBar()

            Controls.Add(_mainLayout)
            Controls.Add(_statusBar)
            Controls.Add(_ribbon)

            ResumeLayout(False)
        End Sub
        Private Sub ConfigureRibbon()
            _ribbon.Dock = DockStyle.Top
            _ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            _ribbon.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden
            _ribbon.MdiMergeStyle = RibbonMdiMergeStyle.Always

            AssignIcon(_btnSeccionGeneral, "BusinessObjects.BOPerson")
            AssignIcon(_btnSeccionIdentificaciones, "BusinessObjects.BOValidation")
            AssignIcon(_btnSeccionDirecciones, "BusinessObjects.BOAddress")
            AssignIcon(_btnSeccionContactos, "BusinessObjects.BOContact")
            AssignIcon(_btnSeccionCuentas, "BusinessObjects.BOInvoice")
            AssignIcon(_btnSeccionRoles, "BusinessObjects.BORole")

            AssignIcon(_btnNuevo, "Actions.Add")
            AssignIcon(_btnGuardar, "Save.Save")
            AssignIcon(_btnRefrescar, "Actions.Refresh")
            AssignIcon(_btnDesactivar, "Actions.Cancel")
            AssignIcon(_btnLimpiar, "Actions.Clear")

            For Each item In New BarButtonItem() {
                _btnSeccionGeneral, _btnSeccionIdentificaciones, _btnSeccionDirecciones,
                _btnSeccionContactos, _btnSeccionCuentas, _btnSeccionRoles,
                _btnNuevo, _btnGuardar, _btnRefrescar, _btnDesactivar, _btnLimpiar
            }
                item.PaintStyle = BarItemPaintStyle.CaptionGlyph
            Next

            _ribbon.Items.AddRange(New BarItem() {
                _btnSeccionGeneral, _btnSeccionIdentificaciones, _btnSeccionDirecciones,
                _btnSeccionContactos, _btnSeccionCuentas, _btnSeccionRoles,
                _btnNuevo, _btnGuardar, _btnRefrescar, _btnDesactivar, _btnLimpiar,
                _statusInfo
            })

            Dim page As New RibbonPage("Inicio")
            Dim groupSecciones As New RibbonPageGroup("Secciones")
            groupSecciones.ItemLinks.Add(_btnSeccionGeneral)
            groupSecciones.ItemLinks.Add(_btnSeccionIdentificaciones)
            groupSecciones.ItemLinks.Add(_btnSeccionDirecciones)
            groupSecciones.ItemLinks.Add(_btnSeccionContactos)
            groupSecciones.ItemLinks.Add(_btnSeccionCuentas)
            groupSecciones.ItemLinks.Add(_btnSeccionRoles)

            Dim groupAcciones As New RibbonPageGroup("Acciones")
            groupAcciones.ItemLinks.Add(_btnNuevo)
            groupAcciones.ItemLinks.Add(_btnGuardar)
            groupAcciones.ItemLinks.Add(_btnRefrescar)
            groupAcciones.ItemLinks.Add(_btnDesactivar)
            groupAcciones.ItemLinks.Add(_btnLimpiar)

            page.Groups.Add(groupSecciones)
            page.Groups.Add(groupAcciones)
            _ribbon.Pages.Add(page)
        End Sub

        Private Sub ConfigureBody()
            _mainLayout.Dock = DockStyle.Fill
            _mainLayout.Root = New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }

            _splitMain.Dock = DockStyle.Fill
            _splitMain.SplitterPosition = 430
            _splitMain.FixedPanel = SplitFixedPanel.Panel1

            BuildLeftPanel()
            BuildRightPanel()

            _mainLayout.Controls.Add(_splitMain)
            Dim item = DirectCast(_mainLayout.Root, LayoutControlGroup).AddItem(String.Empty, _splitMain)
            item.TextVisible = False
        End Sub

        Private Sub BuildLeftPanel()
            Dim panelLeft As New PanelControl() With {
                .Dock = DockStyle.Fill,
                .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            }

            Dim searchPanel As New PanelControl() With {
                .Dock = DockStyle.Top,
                .Height = 44,
                .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            }

            Dim lblBuscar As New LabelControl() With {
                .Text = "Buscar tercero:",
                .Location = New Point(10, 14)
            }

            _txtBuscar.Location = New Point(102, 10)
            _txtBuscar.Width = 312
            _txtBuscar.Properties.NullValuePrompt = "Codigo, nombre, razon social..."
            _txtBuscar.Properties.NullValuePromptShowForEmptyValue = True

            searchPanel.Controls.Add(lblBuscar)
            searchPanel.Controls.Add(_txtBuscar)

            _gridTerceros.Dock = DockStyle.Fill
            _gridTerceros.MainView = _viewTerceros
            _gridTerceros.UseEmbeddedNavigator = True
            _gridTerceros.DataSource = _tblTerceros
            _gridTerceros.ViewCollection.Add(_viewTerceros)
            ConfigureTercerosView()

            panelLeft.Controls.Add(_gridTerceros)
            panelLeft.Controls.Add(searchPanel)
            _splitMain.Panel1.Controls.Add(panelLeft)
        End Sub

        Private Sub BuildRightPanel()
            Dim panelRight As New PanelControl() With {
                .Dock = DockStyle.Fill,
                .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            }

            _editorLayout.Dock = DockStyle.Fill
            panelRight.Controls.Add(_editorLayout)
            _splitMain.Panel2.Controls.Add(panelRight)

            _chkActivo.Properties.Caption = "Activo"
            _cmbTipoPersona.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor

            _dteFechaNacimiento.Properties.CalendarTimeProperties.Buttons.Clear()
            _dteFechaNacimiento.Properties.CalendarTimeProperties.Buttons.Add(New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo))
            _dteFechaConstitucion.Properties.CalendarTimeProperties.Buttons.Clear()
            _dteFechaConstitucion.Properties.CalendarTimeProperties.Buttons.Add(New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo))

            _editorLayout.Controls.Add(_txtCodigo)
            _editorLayout.Controls.Add(_cmbTipoPersona)
            _editorLayout.Controls.Add(_txtNombre)
            _editorLayout.Controls.Add(_txtSegundoNombre)
            _editorLayout.Controls.Add(_txtApellido)
            _editorLayout.Controls.Add(_txtSegundoApellido)
            _editorLayout.Controls.Add(_txtRazonSocial)
            _editorLayout.Controls.Add(_txtNombreComercial)
            _editorLayout.Controls.Add(_dteFechaNacimiento)
            _editorLayout.Controls.Add(_dteFechaConstitucion)
            _editorLayout.Controls.Add(_chkActivo)
            _editorLayout.Controls.Add(_tabsDetalle)

            ConfigureDetailTabs()

            Dim root As New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }
            _editorLayout.Root = root

            Dim info As New LabelControl() With {.Text = "Ficha unificada de tercero. Edite y guarde en una sola pantalla."}
            info.Appearance.ForeColor = Color.DimGray
            _editorLayout.Controls.Add(info)

            Dim iInfo = root.AddItem(String.Empty, info)
            iInfo.TextVisible = False
            iInfo.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 8, 10)

            Dim groupGeneral As LayoutControlGroup = root.AddGroup()
            groupGeneral.Text = "Datos generales"

            groupGeneral.AddItem("Codigo*", _txtCodigo)
            groupGeneral.AddItem("Tipo persona*", _cmbTipoPersona)
            groupGeneral.AddItem("Nombre", _txtNombre)
            groupGeneral.AddItem("Segundo nombre", _txtSegundoNombre)
            groupGeneral.AddItem("Apellido", _txtApellido)
            groupGeneral.AddItem("Segundo apellido", _txtSegundoApellido)
            groupGeneral.AddItem("Razon social", _txtRazonSocial)
            groupGeneral.AddItem("Nombre comercial", _txtNombreComercial)
            groupGeneral.AddItem("Fecha nacimiento", _dteFechaNacimiento)
            groupGeneral.AddItem("Fecha constitucion", _dteFechaConstitucion)

            Dim activeItem = groupGeneral.AddItem(String.Empty, _chkActivo)
            activeItem.TextVisible = False

            Dim tabItem = root.AddItem(String.Empty, _tabsDetalle)
            tabItem.TextVisible = False
            tabItem.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 8, 0)
        End Sub

        Private Sub ConfigureDetailTabs()
            _tabsDetalle.Dock = DockStyle.Fill
            _tabsDetalle.TabPages.AddRange(New XtraTabPage() {
                _tabIdentificaciones,
                _tabDirecciones,
                _tabContactos,
                _tabCuentas,
                _tabRoles
            })

            ConfigureChildGrid(_gridIdentificaciones, _viewIdentificaciones, _tblIdentificaciones)
            ConfigureChildGrid(_gridDirecciones, _viewDirecciones, _tblDirecciones)
            ConfigureChildGrid(_gridContactos, _viewContactos, _tblContactos)
            ConfigureChildGrid(_gridCuentas, _viewCuentas, _tblCuentas)
            ConfigureChildGrid(_gridRoles, _viewRoles, _tblRoles)

            _tabIdentificaciones.Controls.Add(_gridIdentificaciones)
            _tabDirecciones.Controls.Add(_gridDirecciones)
            _tabContactos.Controls.Add(_gridContactos)
            _tabCuentas.Controls.Add(_gridCuentas)
            _tabRoles.Controls.Add(_gridRoles)

            ConfigureIdentificacionesColumns()
            ConfigureDireccionesColumns()
            ConfigureContactosColumns()
            ConfigureCuentasColumns()
            ConfigureRolesColumns()
        End Sub

        Private Sub ConfigureChildGrid(ByVal grid As GridControl, ByVal view As GridView, ByVal source As DataTable)
            grid.Dock = DockStyle.Fill
            grid.MainView = view
            grid.UseEmbeddedNavigator = True
            grid.DataSource = source
            grid.ViewCollection.Add(view)

            view.OptionsView.ColumnAutoWidth = False
            view.OptionsView.ShowGroupPanel = False
            view.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
            view.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click
            view.OptionsNavigation.AutoFocusNewRow = True
            view.OptionsCustomization.AllowSort = True
            view.OptionsCustomization.AllowFilter = True
            view.OptionsSelection.EnableAppearanceFocusedCell = True
            view.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False
        End Sub

        Private Sub ConfigureTercerosView()
            _viewTerceros.OptionsBehavior.Editable = False
            _viewTerceros.OptionsView.ColumnAutoWidth = False
            _viewTerceros.OptionsView.ShowGroupPanel = False
            _viewTerceros.OptionsFind.AllowFindPanel = False

            _viewTerceros.Columns.Clear()
            _viewTerceros.Columns.AddVisible("id_tercero", "Id").Width = 70
            _viewTerceros.Columns.AddVisible("codigo", "Codigo").Width = 120
            _viewTerceros.Columns.AddVisible("tipo_persona", "Tipo persona").Width = 130
            _viewTerceros.Columns.AddVisible("nombre_principal", "Nombre principal").Width = 230
            _viewTerceros.Columns.AddVisible("activo", "Activo").Width = 70
        End Sub

        Private Sub ConfigureIdentificacionesColumns()
            _viewIdentificaciones.Columns.Clear()
            _viewIdentificaciones.Columns.AddVisible("id_identificacion_tercero", "Id").Width = 80
            _viewIdentificaciones.Columns.AddVisible("id_tipo_identificacion", "Tipo").Width = 110
            _viewIdentificaciones.Columns.AddVisible("numero_identificacion", "Numero").Width = 180
            _viewIdentificaciones.Columns.AddVisible("fecha_emision", "Emision").Width = 110
            _viewIdentificaciones.Columns.AddVisible("fecha_vencimiento", "Vencimiento").Width = 110
            _viewIdentificaciones.Columns.AddVisible("principal", "Principal").Width = 90
        End Sub

        Private Sub ConfigureDireccionesColumns()
            _viewDirecciones.Columns.Clear()
            _viewDirecciones.Columns.AddVisible("id_direccion_tercero", "Id").Width = 80
            _viewDirecciones.Columns.AddVisible("id_tipo_direccion", "Tipo").Width = 110
            _viewDirecciones.Columns.AddVisible("direccion_linea1", "Direccion 1").Width = 210
            _viewDirecciones.Columns.AddVisible("direccion_linea2", "Direccion 2").Width = 210
            _viewDirecciones.Columns.AddVisible("id_pais", "Pais").Width = 90
            _viewDirecciones.Columns.AddVisible("id_estado", "Estado").Width = 90
            _viewDirecciones.Columns.AddVisible("id_ciudad", "Ciudad").Width = 90
            _viewDirecciones.Columns.AddVisible("codigo_postal", "Postal").Width = 100
            _viewDirecciones.Columns.AddVisible("principal", "Principal").Width = 90
        End Sub

        Private Sub ConfigureContactosColumns()
            _viewContactos.Columns.Clear()
            _viewContactos.Columns.AddVisible("id_contacto_tercero", "Id").Width = 80
            _viewContactos.Columns.AddVisible("id_tipo_contacto", "Tipo").Width = 110
            _viewContactos.Columns.AddVisible("valor", "Valor").Width = 260
            _viewContactos.Columns.AddVisible("principal", "Principal").Width = 90
        End Sub

        Private Sub ConfigureCuentasColumns()
            _viewCuentas.Columns.Clear()
            _viewCuentas.Columns.AddVisible("id_cuenta_bancaria_tercero", "Id").Width = 80
            _viewCuentas.Columns.AddVisible("id_banco", "Banco").Width = 90
            _viewCuentas.Columns.AddVisible("numero_cuenta", "Numero cuenta").Width = 230
            _viewCuentas.Columns.AddVisible("id_moneda", "Moneda").Width = 90
            _viewCuentas.Columns.AddVisible("principal", "Principal").Width = 90
        End Sub

        Private Sub ConfigureRolesColumns()
            _viewRoles.Columns.Clear()
            _viewRoles.Columns.AddVisible("id_tercero_rol", "Id").Width = 80
            _viewRoles.Columns.AddVisible("id_rol_tercero", "Rol tercero").Width = 110
            _viewRoles.Columns.AddVisible("id_empresa", "Empresa").Width = 90
            _viewRoles.Columns.AddVisible("activo", "Activo").Width = 90
        End Sub

        Private Sub ConfigureStatusBar()
            _statusBar.Dock = DockStyle.Bottom
            _statusBar.Ribbon = _ribbon
            _statusInfo.Caption = "Centro de terceros listo."
            _statusBar.ItemLinks.Add(_statusInfo)
        End Sub

        Private Sub WireEvents()
            AddHandler Shown, AddressOf OnFormShown
            AddHandler _txtBuscar.EditValueChanged, AddressOf OnBuscarChanged
            AddHandler _viewTerceros.FocusedRowChanged, AddressOf OnTerceroFocusedRowChanged

            AddHandler _btnSeccionGeneral.ItemClick, AddressOf OnSeccionGeneralClick
            AddHandler _btnSeccionIdentificaciones.ItemClick, AddressOf OnSeccionIdentificacionesClick
            AddHandler _btnSeccionDirecciones.ItemClick, AddressOf OnSeccionDireccionesClick
            AddHandler _btnSeccionContactos.ItemClick, AddressOf OnSeccionContactosClick
            AddHandler _btnSeccionCuentas.ItemClick, AddressOf OnSeccionCuentasClick
            AddHandler _btnSeccionRoles.ItemClick, AddressOf OnSeccionRolesClick

            AddHandler _btnNuevo.ItemClick, AddressOf OnNuevoClick
            AddHandler _btnGuardar.ItemClick, AddressOf OnGuardarClick
            AddHandler _btnRefrescar.ItemClick, AddressOf OnRefrescarClick
            AddHandler _btnDesactivar.ItemClick, AddressOf OnDesactivarClick
            AddHandler _btnLimpiar.ItemClick, AddressOf OnLimpiarClick
        End Sub
        Private Sub OnFormShown(ByVal sender As Object, ByVal e As EventArgs)
            If _firstLoadDone Then Return
            _firstLoadDone = True
            ReloadAll(True)
        End Sub

        Private Sub ReloadAll(ByVal selectFirstIfNeeded As Boolean)
            Dim previousId = _editingTerceroId
            _loadingUi = True
            Try
                LoadCatalogos()
                LoadTerceros()
                LoadChildren()
                ConfigureDynamicIdLookups()
                BindTercerosTable()

                Dim targetId = previousId
                If Not targetId.HasValue AndAlso selectFirstIfNeeded AndAlso _tblTerceros.Rows.Count > 0 Then
                    targetId = Convert.ToInt64(_tblTerceros.Rows(0)("id_tercero"))
                End If

                SelectTercero(targetId)
                _statusInfo.Caption = $"Terceros: {_tblTerceros.Rows.Count}"
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Terceros", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                _loadingUi = False
            End Try
        End Sub

        Private Sub LoadCatalogos()
            Try
                Dim tipoPersonaResult = _apiClient.GetAsync(Of List(Of TipoPersonaDto))("api/v1/tercero/tipo_persona").GetAwaiter().GetResult()
                _tiposPersona = If(tipoPersonaResult, New List(Of TipoPersonaDto)())
            Catch
                _tiposPersona = New List(Of TipoPersonaDto)()
            End Try

            _cmbTipoPersona.Properties.Items.Clear()
            For Each item In _tiposPersona.OrderBy(Function(x) x.Nombre)
                _cmbTipoPersona.Properties.Items.Add(New ComboIntItem With {
                    .Id = item.IdTipoPersona.GetValueOrDefault(0),
                    .Nombre = If(String.IsNullOrWhiteSpace(item.Nombre), $"Tipo {item.IdTipoPersona}", item.Nombre)
                })
            Next

            If _cmbTipoPersona.Properties.Items.Count > 0 Then
                _cmbTipoPersona.SelectedIndex = 0
            End If
        End Sub

        Private Sub LoadTerceros()
            Dim result = _apiClient.GetAsync(Of List(Of TerceroDto))("api/v1/tercero/tercero").GetAwaiter().GetResult()
            _terceros = If(result, New List(Of TerceroDto)())
        End Sub

        Private Sub LoadChildren()
            _identificaciones = SafeGetList(Of IdentificacionTerceroDto)("api/v1/tercero/identificacion_tercero")
            _direcciones = SafeGetList(Of DireccionTerceroDto)("api/v1/tercero/direccion_tercero")
            _contactos = SafeGetList(Of ContactoTerceroDto)("api/v1/tercero/contacto_tercero")
            _cuentas = SafeGetList(Of CuentaBancariaTerceroDto)("api/v1/tercero/cuenta_bancaria_tercero")
            _roles = SafeGetList(Of TerceroRolDto)("api/v1/tercero/tercero_rol")
        End Sub

        Private Sub ConfigureDynamicIdLookups()
            ApplyIntegerLookup(_gridIdentificaciones, _viewIdentificaciones, "id_tipo_identificacion", _identificaciones.
                Select(Function(x) x.IdTipoIdentificacion.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())

            ApplyIntegerLookup(_gridDirecciones, _viewDirecciones, "id_tipo_direccion", _direcciones.
                Select(Function(x) x.IdTipoDireccion.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())
            ApplyIntegerLookup(_gridDirecciones, _viewDirecciones, "id_pais", _direcciones.
                Select(Function(x) x.IdPais.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())
            ApplyIntegerLookup(_gridDirecciones, _viewDirecciones, "id_estado", _direcciones.
                Select(Function(x) x.IdEstado.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())
            ApplyIntegerLookup(_gridDirecciones, _viewDirecciones, "id_ciudad", _direcciones.
                Select(Function(x) x.IdCiudad.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())

            ApplyIntegerLookup(_gridContactos, _viewContactos, "id_tipo_contacto", _contactos.
                Select(Function(x) x.IdTipoContacto.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())

            ApplyIntegerLookup(_gridCuentas, _viewCuentas, "id_banco", _cuentas.
                Select(Function(x) x.IdBanco.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())
            ApplyIntegerLookup(_gridCuentas, _viewCuentas, "id_moneda", _cuentas.
                Select(Function(x) x.IdMoneda.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())

            ApplyIntegerLookup(_gridRoles, _viewRoles, "id_rol_tercero", _roles.
                Select(Function(x) x.IdRolTercero.GetValueOrDefault(0)).
                Where(Function(id) id > 0).
                Distinct().
                OrderBy(Function(id) id).
                ToList())
        End Sub

        Private Shared Sub ApplyIntegerLookup(ByVal grid As GridControl, ByVal view As GridView, ByVal fieldName As String, ByVal values As IReadOnlyCollection(Of Integer))
            Dim col = view.Columns.ColumnByFieldName(fieldName)
            If col Is Nothing Then Return

            Dim repo As New RepositoryItemComboBox()
            repo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            repo.Items.Clear()

            If values IsNot Nothing Then
                For Each value In values
                    repo.Items.Add(value)
                Next
            End If

            grid.RepositoryItems.Add(repo)
            col.ColumnEdit = repo
        End Sub

        Private Function SafeGetList(Of T)(ByVal endpoint As String) As List(Of T)
            Try
                Dim result = _apiClient.GetAsync(Of List(Of T))(endpoint).GetAwaiter().GetResult()
                Return If(result, New List(Of T)())
            Catch
                Return New List(Of T)()
            End Try
        End Function

        Private Sub BindTercerosTable()
            _tblTerceros.Rows.Clear()

            For Each t In _terceros.OrderBy(Function(x) x.Codigo)
                Dim tipoNombre = ResolveTipoPersonaNombre(t.IdTipoPersona)
                Dim nombrePrincipal = BuildNombrePrincipal(t)
                _tblTerceros.Rows.Add(
                    t.IdTercero.GetValueOrDefault(0),
                    If(t.Codigo, String.Empty),
                    tipoNombre,
                    nombrePrincipal,
                    t.Activo.GetValueOrDefault(False))
            Next
        End Sub

        Private Sub SelectTercero(ByVal idTercero As Long?)
            If Not idTercero.HasValue Then
                ClearEditor()
                BindChildTables(Nothing)
                Return
            End If

            Dim foundHandle = GridControl.InvalidRowHandle
            For rowHandle = 0 To _viewTerceros.RowCount - 1
                Dim rowId = ToNullableLong(_viewTerceros.GetRowCellValue(rowHandle, "id_tercero"))
                If rowId.HasValue AndAlso rowId.Value = idTercero.Value Then
                    foundHandle = rowHandle
                    Exit For
                End If
            Next

            If foundHandle <> GridControl.InvalidRowHandle Then
                _viewTerceros.FocusedRowHandle = foundHandle
            End If

            Dim current = _terceros.FirstOrDefault(Function(x) x.IdTercero.HasValue AndAlso x.IdTercero.Value = idTercero.Value)
            If current Is Nothing Then
                ClearEditor()
                BindChildTables(Nothing)
                Return
            End If

            PopulateEditor(current)
            BindChildTables(current.IdTercero)
        End Sub

        Private Sub PopulateEditor(ByVal dto As TerceroDto)
            _editingTerceroId = dto.IdTercero
            _txtCodigo.Text = If(dto.Codigo, String.Empty)
            _txtNombre.Text = If(dto.Nombre, String.Empty)
            _txtSegundoNombre.Text = If(dto.SegundoNombre, String.Empty)
            _txtApellido.Text = If(dto.Apellido, String.Empty)
            _txtSegundoApellido.Text = If(dto.SegundoApellido, String.Empty)
            _txtRazonSocial.Text = If(dto.RazonSocial, String.Empty)
            _txtNombreComercial.Text = If(dto.NombreComercial, String.Empty)
            _dteFechaNacimiento.EditValue = dto.FechaNacimiento
            _dteFechaConstitucion.EditValue = dto.FechaConstitucion
            _chkActivo.Checked = dto.Activo.GetValueOrDefault(True)

            Dim selected = _cmbTipoPersona.Properties.Items.
                OfType(Of ComboIntItem)().
                FirstOrDefault(Function(x) x.Id = dto.IdTipoPersona.GetValueOrDefault(0))
            _cmbTipoPersona.SelectedItem = selected
        End Sub

        Private Sub BindChildTables(ByVal idTercero As Long?)
            _tblIdentificaciones.Rows.Clear()
            _tblDirecciones.Rows.Clear()
            _tblContactos.Rows.Clear()
            _tblCuentas.Rows.Clear()
            _tblRoles.Rows.Clear()

            If Not idTercero.HasValue Then Return

            For Each item In _identificaciones.Where(Function(x) x.IdTercero.GetValueOrDefault(0) = idTercero.Value)
                _tblIdentificaciones.Rows.Add(
                    item.IdIdentificacionTercero.GetValueOrDefault(0),
                    idTercero.Value,
                    item.IdTipoIdentificacion.GetValueOrDefault(0),
                    If(item.NumeroIdentificacion, String.Empty),
                    If(item.FechaEmision.HasValue, item.FechaEmision.Value, CType(DBNull.Value, Object)),
                    If(item.FechaVencimiento.HasValue, item.FechaVencimiento.Value, CType(DBNull.Value, Object)),
                    item.Principal.GetValueOrDefault(False))
            Next

            For Each item In _direcciones.Where(Function(x) x.IdTercero.GetValueOrDefault(0) = idTercero.Value)
                _tblDirecciones.Rows.Add(
                    item.IdDireccionTercero.GetValueOrDefault(0),
                    idTercero.Value,
                    item.IdTipoDireccion.GetValueOrDefault(0),
                    If(item.DireccionLinea1, String.Empty),
                    If(item.DireccionLinea2, String.Empty),
                    item.IdPais.GetValueOrDefault(0),
                    item.IdEstado.GetValueOrDefault(0),
                    item.IdCiudad.GetValueOrDefault(0),
                    If(item.CodigoPostal, String.Empty),
                    item.Principal.GetValueOrDefault(False))
            Next

            For Each item In _contactos.Where(Function(x) x.IdTercero.GetValueOrDefault(0) = idTercero.Value)
                _tblContactos.Rows.Add(
                    item.IdContactoTercero.GetValueOrDefault(0),
                    idTercero.Value,
                    item.IdTipoContacto.GetValueOrDefault(0),
                    If(item.Valor, String.Empty),
                    item.Principal.GetValueOrDefault(False))
            Next

            For Each item In _cuentas.Where(Function(x) x.IdTercero.GetValueOrDefault(0) = idTercero.Value)
                _tblCuentas.Rows.Add(
                    item.IdCuentaBancariaTercero.GetValueOrDefault(0),
                    idTercero.Value,
                    item.IdBanco.GetValueOrDefault(0),
                    If(item.NumeroCuenta, String.Empty),
                    item.IdMoneda.GetValueOrDefault(0),
                    item.Principal.GetValueOrDefault(False))
            Next

            For Each item In _roles.Where(Function(x) x.IdTercero.GetValueOrDefault(0) = idTercero.Value)
                _tblRoles.Rows.Add(
                    item.IdTerceroRol.GetValueOrDefault(0),
                    idTercero.Value,
                    item.IdRolTercero.GetValueOrDefault(0),
                    item.IdEmpresa.GetValueOrDefault(0),
                    item.Activo.GetValueOrDefault(True))
            Next
        End Sub

        Private Shared Function BuildNombrePrincipal(ByVal dto As TerceroDto) As String
            If Not String.IsNullOrWhiteSpace(dto.RazonSocial) Then
                Return dto.RazonSocial
            End If

            Dim fullName = String.Join(" ", New String() {
                If(dto.Nombre, String.Empty),
                If(dto.SegundoNombre, String.Empty),
                If(dto.Apellido, String.Empty),
                If(dto.SegundoApellido, String.Empty)
            }.Where(Function(part) Not String.IsNullOrWhiteSpace(part))).Trim()

            If Not String.IsNullOrWhiteSpace(fullName) Then
                Return fullName
            End If

            If Not String.IsNullOrWhiteSpace(dto.NombreComercial) Then
                Return dto.NombreComercial
            End If

            Return String.Empty
        End Function

        Private Function ResolveTipoPersonaNombre(ByVal idTipoPersona As Integer?) As String
            If Not idTipoPersona.HasValue Then Return String.Empty
            Dim value = _tiposPersona.FirstOrDefault(Function(x) x.IdTipoPersona.GetValueOrDefault(0) = idTipoPersona.Value)
            If value Is Nothing Then Return $"Id {idTipoPersona.Value}"
            Return If(String.IsNullOrWhiteSpace(value.Nombre), $"Id {idTipoPersona.Value}", value.Nombre)
        End Function

        Private Sub OnBuscarChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim rawFilter = If(_txtBuscar.Text, String.Empty).Trim()
            If String.IsNullOrWhiteSpace(rawFilter) Then
                _viewTerceros.FindFilterText = String.Empty
                Return
            End If

            _viewTerceros.FindFilterText = rawFilter
        End Sub

        Private Sub OnTerceroFocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
            If _loadingUi Then Return
            Dim id = ToNullableLong(_viewTerceros.GetRowCellValue(_viewTerceros.FocusedRowHandle, "id_tercero"))
            If Not id.HasValue Then Return

            Dim dto = _terceros.FirstOrDefault(Function(x) x.IdTercero.GetValueOrDefault(0) = id.Value)
            If dto Is Nothing Then Return

            PopulateEditor(dto)
            BindChildTables(id)
        End Sub

        Private Sub OnSeccionGeneralClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            _txtCodigo.Focus()
        End Sub

        Private Sub OnSeccionIdentificacionesClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            _tabsDetalle.SelectedTabPage = _tabIdentificaciones
        End Sub

        Private Sub OnSeccionDireccionesClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            _tabsDetalle.SelectedTabPage = _tabDirecciones
        End Sub

        Private Sub OnSeccionContactosClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            _tabsDetalle.SelectedTabPage = _tabContactos
        End Sub

        Private Sub OnSeccionCuentasClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            _tabsDetalle.SelectedTabPage = _tabCuentas
        End Sub

        Private Sub OnSeccionRolesClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            _tabsDetalle.SelectedTabPage = _tabRoles
        End Sub
        Private Sub OnNuevoClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            ClearEditor()
            BindChildTables(Nothing)
            _txtCodigo.Focus()
            _statusInfo.Caption = "Nuevo tercero listo para capturar."
        End Sub

        Private Sub OnGuardarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Try
                _viewIdentificaciones.CloseEditor()
                _viewIdentificaciones.UpdateCurrentRow()
                _viewDirecciones.CloseEditor()
                _viewDirecciones.UpdateCurrentRow()
                _viewContactos.CloseEditor()
                _viewContactos.UpdateCurrentRow()
                _viewCuentas.CloseEditor()
                _viewCuentas.UpdateCurrentRow()
                _viewRoles.CloseEditor()
                _viewRoles.UpdateCurrentRow()

                Dim dto = BuildTerceroFromEditor()
                Dim savedId As Long

                If _editingTerceroId.HasValue AndAlso _editingTerceroId.Value > 0 Then
                    dto.IdTercero = _editingTerceroId
                    _apiClient.PutAsync($"api/v1/tercero/tercero/actualizar/{_editingTerceroId.Value}", dto).GetAwaiter().GetResult()
                    savedId = _editingTerceroId.Value
                Else
                    Dim created = _apiClient.PostAsync(Of TerceroDto, IdResponse)("api/v1/tercero/tercero/crear", dto).GetAwaiter().GetResult()
                    savedId = If(created IsNot Nothing AndAlso created.Id > 0, created.Id, 0)
                    If savedId <= 0 Then
                        Throw New InvalidOperationException("La API no devolvio id de tercero creado.")
                    End If
                End If

                _editingTerceroId = savedId
                SaveChildRows(savedId)

                ReloadAll(False)
                SelectTercero(savedId)
                _statusInfo.Caption = $"Tercero {savedId} guardado correctamente."
                XtraMessageBox.Show(Me, "Cambios guardados correctamente.", "Terceros", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub SaveChildRows(ByVal idTercero As Long)
            SaveIdentificaciones(idTercero)
            SaveDirecciones(idTercero)
            SaveContactos(idTercero)
            SaveCuentas(idTercero)
            SaveRoles(idTercero)
        End Sub

        Private Sub SaveIdentificaciones(ByVal idTercero As Long)
            For Each row As DataRow In _tblIdentificaciones.Rows
                If row.RowState = DataRowState.Deleted Then Continue For

                Dim numero = Convert.ToString(row("numero_identificacion")).Trim()
                If String.IsNullOrWhiteSpace(numero) Then Continue For

                Dim dto As New IdentificacionTerceroDto With {
                    .IdTercero = idTercero,
                    .IdTipoIdentificacion = ToNullableInt(row("id_tipo_identificacion")),
                    .NumeroIdentificacion = numero,
                    .FechaEmision = ToNullableDate(row("fecha_emision")),
                    .FechaVencimiento = ToNullableDate(row("fecha_vencimiento")),
                    .Principal = ToNullableBool(row("principal"))
                }

                Dim id = ToNullableLong(row("id_identificacion_tercero"))
                If id.HasValue AndAlso id.Value > 0 Then
                    dto.IdIdentificacionTercero = id
                    _apiClient.PutAsync($"api/v1/tercero/identificacion_tercero/actualizar/{id.Value}", dto).GetAwaiter().GetResult()
                Else
                    Dim created = _apiClient.PostAsync(Of IdentificacionTerceroDto, IdResponse)("api/v1/tercero/identificacion_tercero/crear", dto).GetAwaiter().GetResult()
                    If created IsNot Nothing Then
                        row("id_identificacion_tercero") = created.Id
                    End If
                End If
            Next
        End Sub

        Private Sub SaveDirecciones(ByVal idTercero As Long)
            For Each row As DataRow In _tblDirecciones.Rows
                If row.RowState = DataRowState.Deleted Then Continue For

                Dim direccion1 = Convert.ToString(row("direccion_linea1")).Trim()
                If String.IsNullOrWhiteSpace(direccion1) Then Continue For

                Dim dto As New DireccionTerceroDto With {
                    .IdTercero = idTercero,
                    .IdTipoDireccion = ToNullableInt(row("id_tipo_direccion")),
                    .DireccionLinea1 = direccion1,
                    .DireccionLinea2 = Convert.ToString(row("direccion_linea2")),
                    .IdPais = ToNullableInt(row("id_pais")),
                    .IdEstado = ToNullableInt(row("id_estado")),
                    .IdCiudad = ToNullableInt(row("id_ciudad")),
                    .CodigoPostal = Convert.ToString(row("codigo_postal")),
                    .Principal = ToNullableBool(row("principal"))
                }

                Dim id = ToNullableLong(row("id_direccion_tercero"))
                If id.HasValue AndAlso id.Value > 0 Then
                    dto.IdDireccionTercero = id
                    _apiClient.PutAsync($"api/v1/tercero/direccion_tercero/actualizar/{id.Value}", dto).GetAwaiter().GetResult()
                Else
                    Dim created = _apiClient.PostAsync(Of DireccionTerceroDto, IdResponse)("api/v1/tercero/direccion_tercero/crear", dto).GetAwaiter().GetResult()
                    If created IsNot Nothing Then
                        row("id_direccion_tercero") = created.Id
                    End If
                End If
            Next
        End Sub

        Private Sub SaveContactos(ByVal idTercero As Long)
            For Each row As DataRow In _tblContactos.Rows
                If row.RowState = DataRowState.Deleted Then Continue For

                Dim valor = Convert.ToString(row("valor")).Trim()
                If String.IsNullOrWhiteSpace(valor) Then Continue For

                Dim dto As New ContactoTerceroDto With {
                    .IdTercero = idTercero,
                    .IdTipoContacto = ToNullableInt(row("id_tipo_contacto")),
                    .Valor = valor,
                    .Principal = ToNullableBool(row("principal"))
                }

                Dim id = ToNullableLong(row("id_contacto_tercero"))
                If id.HasValue AndAlso id.Value > 0 Then
                    dto.IdContactoTercero = id
                    _apiClient.PutAsync($"api/v1/tercero/contacto_tercero/actualizar/{id.Value}", dto).GetAwaiter().GetResult()
                Else
                    Dim created = _apiClient.PostAsync(Of ContactoTerceroDto, IdResponse)("api/v1/tercero/contacto_tercero/crear", dto).GetAwaiter().GetResult()
                    If created IsNot Nothing Then
                        row("id_contacto_tercero") = created.Id
                    End If
                End If
            Next
        End Sub

        Private Sub SaveCuentas(ByVal idTercero As Long)
            For Each row As DataRow In _tblCuentas.Rows
                If row.RowState = DataRowState.Deleted Then Continue For

                Dim numero = Convert.ToString(row("numero_cuenta")).Trim()
                If String.IsNullOrWhiteSpace(numero) Then Continue For

                Dim dto As New CuentaBancariaTerceroDto With {
                    .IdTercero = idTercero,
                    .IdBanco = ToNullableInt(row("id_banco")),
                    .NumeroCuenta = numero,
                    .IdMoneda = ToNullableInt(row("id_moneda")),
                    .Principal = ToNullableBool(row("principal"))
                }

                Dim id = ToNullableLong(row("id_cuenta_bancaria_tercero"))
                If id.HasValue AndAlso id.Value > 0 Then
                    dto.IdCuentaBancariaTercero = id
                    _apiClient.PutAsync($"api/v1/tercero/cuenta_bancaria_tercero/actualizar/{id.Value}", dto).GetAwaiter().GetResult()
                Else
                    Dim created = _apiClient.PostAsync(Of CuentaBancariaTerceroDto, IdResponse)("api/v1/tercero/cuenta_bancaria_tercero/crear", dto).GetAwaiter().GetResult()
                    If created IsNot Nothing Then
                        row("id_cuenta_bancaria_tercero") = created.Id
                    End If
                End If
            Next
        End Sub

        Private Sub SaveRoles(ByVal idTercero As Long)
            For Each row As DataRow In _tblRoles.Rows
                If row.RowState = DataRowState.Deleted Then Continue For

                Dim idRol = ToNullableInt(row("id_rol_tercero"))
                If Not idRol.HasValue OrElse idRol.Value <= 0 Then Continue For

                Dim dto As New TerceroRolDto With {
                    .IdTercero = idTercero,
                    .IdRolTercero = idRol,
                    .IdEmpresa = ToNullableLong(row("id_empresa")),
                    .Activo = ToNullableBool(row("activo"))
                }

                Dim id = ToNullableLong(row("id_tercero_rol"))
                If id.HasValue AndAlso id.Value > 0 Then
                    dto.IdTerceroRol = id
                    _apiClient.PutAsync($"api/v1/tercero/tercero_rol/actualizar/{id.Value}", dto).GetAwaiter().GetResult()
                Else
                    Dim created = _apiClient.PostAsync(Of TerceroRolDto, IdResponse)("api/v1/tercero/tercero_rol/crear", dto).GetAwaiter().GetResult()
                    If created IsNot Nothing Then
                        row("id_tercero_rol") = created.Id
                    End If
                End If
            Next
        End Sub

        Private Function BuildTerceroFromEditor() As TerceroDto
            If String.IsNullOrWhiteSpace(_txtCodigo.Text) Then
                Throw New InvalidOperationException("El codigo del tercero es obligatorio.")
            End If

            Dim selectedTipo = TryCast(_cmbTipoPersona.SelectedItem, ComboIntItem)
            If selectedTipo Is Nothing OrElse selectedTipo.Id <= 0 Then
                Throw New InvalidOperationException("Debe seleccionar un tipo de persona valido.")
            End If

            Dim dto As New TerceroDto With {
                .Codigo = _txtCodigo.Text.Trim(),
                .IdTipoPersona = selectedTipo.Id,
                .Nombre = NullIfEmpty(_txtNombre.Text),
                .SegundoNombre = NullIfEmpty(_txtSegundoNombre.Text),
                .Apellido = NullIfEmpty(_txtApellido.Text),
                .SegundoApellido = NullIfEmpty(_txtSegundoApellido.Text),
                .RazonSocial = NullIfEmpty(_txtRazonSocial.Text),
                .NombreComercial = NullIfEmpty(_txtNombreComercial.Text),
                .FechaNacimiento = ToNullableDate(_dteFechaNacimiento.EditValue),
                .FechaConstitucion = ToNullableDate(_dteFechaConstitucion.EditValue),
                .Activo = _chkActivo.Checked,
                .CreadoPor = If(_sessionContext.IdUsuario.HasValue, CInt(_sessionContext.IdUsuario.Value), CType(Nothing, Integer?)),
                .CreadoUtc = DateTime.UtcNow
            }

            Return dto
        End Function

        Private Sub OnRefrescarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            ReloadAll(True)
        End Sub

        Private Sub OnDesactivarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If Not _editingTerceroId.HasValue OrElse _editingTerceroId.Value <= 0 Then
                XtraMessageBox.Show(Me, "Seleccione un tercero para desactivar.", "Terceros", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If XtraMessageBox.Show(Me, "Desea desactivar el tercero seleccionado?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            Try
                _apiClient.DeleteAsync($"api/v1/tercero/tercero/desactivar/{_editingTerceroId.Value}").GetAwaiter().GetResult()
                ReloadAll(True)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub OnLimpiarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            ClearEditor()
            BindChildTables(Nothing)
        End Sub

        Private Sub ClearEditor()
            _editingTerceroId = Nothing
            _txtCodigo.Text = String.Empty
            _txtNombre.Text = String.Empty
            _txtSegundoNombre.Text = String.Empty
            _txtApellido.Text = String.Empty
            _txtSegundoApellido.Text = String.Empty
            _txtRazonSocial.Text = String.Empty
            _txtNombreComercial.Text = String.Empty
            _dteFechaNacimiento.EditValue = Nothing
            _dteFechaConstitucion.EditValue = Nothing
            _chkActivo.Checked = True

            If _cmbTipoPersona.Properties.Items.Count > 0 Then
                _cmbTipoPersona.SelectedIndex = 0
            Else
                _cmbTipoPersona.SelectedIndex = -1
            End If
        End Sub

        Private Shared Function NullIfEmpty(ByVal value As String) As String
            If String.IsNullOrWhiteSpace(value) Then Return Nothing
            Return value.Trim()
        End Function

        Private Shared Sub AssignIcon(ByVal item As BarButtonItem, ByVal iconKey As String)
            Dim svg = TryCast(IconService.GetIcon(iconKey), SvgImage)
            If svg Is Nothing Then Return
            item.ImageOptions.SvgImage = svg
        End Sub
        Private Shared Function CreateTercerosTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("codigo", GetType(String))
            table.Columns.Add("tipo_persona", GetType(String))
            table.Columns.Add("nombre_principal", GetType(String))
            table.Columns.Add("activo", GetType(Boolean))
            Return table
        End Function

        Private Shared Function CreateIdentificacionesTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_identificacion_tercero", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_tipo_identificacion", GetType(Integer))
            table.Columns.Add("numero_identificacion", GetType(String))
            table.Columns.Add("fecha_emision", GetType(Date))
            table.Columns.Add("fecha_vencimiento", GetType(Date))
            table.Columns.Add("principal", GetType(Boolean))
            Return table
        End Function

        Private Shared Function CreateDireccionesTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_direccion_tercero", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_tipo_direccion", GetType(Integer))
            table.Columns.Add("direccion_linea1", GetType(String))
            table.Columns.Add("direccion_linea2", GetType(String))
            table.Columns.Add("id_pais", GetType(Integer))
            table.Columns.Add("id_estado", GetType(Integer))
            table.Columns.Add("id_ciudad", GetType(Integer))
            table.Columns.Add("codigo_postal", GetType(String))
            table.Columns.Add("principal", GetType(Boolean))
            Return table
        End Function

        Private Shared Function CreateContactosTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_contacto_tercero", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_tipo_contacto", GetType(Integer))
            table.Columns.Add("valor", GetType(String))
            table.Columns.Add("principal", GetType(Boolean))
            Return table
        End Function

        Private Shared Function CreateCuentasTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_cuenta_bancaria_tercero", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_banco", GetType(Integer))
            table.Columns.Add("numero_cuenta", GetType(String))
            table.Columns.Add("id_moneda", GetType(Integer))
            table.Columns.Add("principal", GetType(Boolean))
            Return table
        End Function

        Private Shared Function CreateRolesTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_tercero_rol", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_rol_tercero", GetType(Integer))
            table.Columns.Add("id_empresa", GetType(Long))
            table.Columns.Add("activo", GetType(Boolean))
            Return table
        End Function

        Private Shared Function ToNullableLong(ByVal raw As Object) As Long?
            If raw Is Nothing OrElse raw Is DBNull.Value Then Return Nothing
            Dim value As Long
            If Long.TryParse(raw.ToString(), value) Then Return value
            Return Nothing
        End Function

        Private Shared Function ToNullableInt(ByVal raw As Object) As Integer?
            If raw Is Nothing OrElse raw Is DBNull.Value Then Return Nothing
            Dim value As Integer
            If Integer.TryParse(raw.ToString(), value) Then Return value
            Return Nothing
        End Function

        Private Shared Function ToNullableDate(ByVal raw As Object) As Date?
            If raw Is Nothing OrElse raw Is DBNull.Value Then Return Nothing
            Dim value As DateTime
            If DateTime.TryParse(raw.ToString(), value) Then Return value.Date
            Return Nothing
        End Function

        Private Shared Function ToNullableBool(ByVal raw As Object) As Boolean?
            If raw Is Nothing OrElse raw Is DBNull.Value Then Return Nothing
            Dim value As Boolean
            If Boolean.TryParse(raw.ToString(), value) Then Return value
            Return Nothing
        End Function
    End Class
End Namespace

