Imports System.Linq
Imports System.ComponentModel
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout
Imports Secure.Platform.Contracts.Dtos.Catalogo
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Seguridad
    ''' <summary>
    ''' Centro administrativo IAM con experiencia unificada para usuarios, roles y asignaciones.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmIamAdminCenter
        Inherits XtraForm

        Private NotInheritable Class ComboLongItem
            Public Property Id As Long
            Public Property Nombre As String = String.Empty

            Public Overrides Function ToString() As String
                Return Nombre
            End Function
        End Class

        Private NotInheritable Class ComboShortItem
            Public Property Id As Short
            Public Property Nombre As String = String.Empty

            Public Overrides Function ToString() As String
                Return Nombre
            End Function
        End Class

        Private NotInheritable Class AsignacionGridRow
            Public Property IdAsignacionRolUsuario As Long
            Public Property Usuario As String = String.Empty
            Public Property Rol As String = String.Empty
            Public Property Alcance As String = String.Empty
            Public Property FechaInicioUtc As DateTime?
            Public Property FechaFinUtc As DateTime?
            Public Property Activo As Boolean
        End Class

        Private ReadOnly _apiClient As ApiClient
        Private ReadOnly _sessionContext As UserSessionContext

        Private ReadOnly _ribbon As RibbonControl
        Private ReadOnly _statusBar As RibbonStatusBar
        Private ReadOnly _statusInfo As BarStaticItem
        Private ReadOnly _statusModulo As BarStaticItem
        Private ReadOnly _btnTabUsuarios As BarButtonItem
        Private ReadOnly _btnTabRoles As BarButtonItem
        Private ReadOnly _btnTabAsignaciones As BarButtonItem
        Private ReadOnly _btnNuevo As BarButtonItem
        Private ReadOnly _btnGuardar As BarButtonItem
        Private ReadOnly _btnRefrescar As BarButtonItem
        Private ReadOnly _btnDesactivar As BarButtonItem
        Private ReadOnly _btnLimpiar As BarButtonItem

        Private ReadOnly _mainLayout As LayoutControl
        Private ReadOnly _tabs As TabControl

        Private ReadOnly _splitUsuarios As SplitContainerControl
        Private ReadOnly _txtBuscarUsuarios As TextEdit
        Private ReadOnly _gridUsuarios As GridControl
        Private ReadOnly _viewUsuarios As GridView
        Private ReadOnly _txtUsuarioCodigo As TextEdit
        Private ReadOnly _txtUsuarioLogin As TextEdit
        Private ReadOnly _txtUsuarioNombre As TextEdit
        Private ReadOnly _txtUsuarioApellido As TextEdit
        Private ReadOnly _txtUsuarioNombreMostrar As TextEdit
        Private ReadOnly _txtUsuarioCorreo As TextEdit
        Private ReadOnly _cmbUsuarioEstado As ComboBoxEdit
        Private ReadOnly _chkUsuarioActivo As CheckEdit
        Private ReadOnly _chkUsuarioMfa As CheckEdit
        Private ReadOnly _chkUsuarioCambioClave As CheckEdit

        Private ReadOnly _splitRoles As SplitContainerControl
        Private ReadOnly _txtBuscarRoles As TextEdit
        Private ReadOnly _gridRoles As GridControl
        Private ReadOnly _viewRoles As GridView
        Private ReadOnly _txtRolCodigo As TextEdit
        Private ReadOnly _txtRolNombre As TextEdit
        Private ReadOnly _memoRolDescripcion As MemoEdit
        Private ReadOnly _chkRolActivo As CheckEdit
        Private ReadOnly _chkRolSistema As CheckEdit

        Private ReadOnly _splitAsignaciones As SplitContainerControl
        Private ReadOnly _txtBuscarAsignaciones As TextEdit
        Private ReadOnly _gridAsignaciones As GridControl
        Private ReadOnly _viewAsignaciones As GridView
        Private ReadOnly _cmbAsignUsuario As ComboBoxEdit
        Private ReadOnly _cmbAsignRol As ComboBoxEdit
        Private ReadOnly _cmbAsignAlcance As ComboBoxEdit
        Private ReadOnly _dteAsignInicio As DateEdit
        Private ReadOnly _dteAsignFin As DateEdit
        Private ReadOnly _chkAsignActivo As CheckEdit

        Private _usuarios As List(Of UsuarioDto)
        Private _roles As List(Of RolDto)
        Private _asignaciones As List(Of AsignacionRolUsuarioDto)
        Private _estadosUsuario As List(Of EstadoUsuarioDto)
        Private _alcances As List(Of AlcanceAsignacionDto)

        Private _editingUsuarioId As Long?
        Private _editingRolId As Long?
        Private _editingAsignacionId As Long?
        Private _usuarioCreadoUtc As DateTime?
        Private _rolCreadoUtc As DateTime?
        Private _asignacionCreadoUtc As DateTime?
        Private _firstLoadDone As Boolean
        Private _loadingEditors As Boolean
        Private _busy As Boolean

        Public Sub New()
            Me.New(New ApiClient(AppSettingsProvider.GetApiBaseUrl()), UserSessionContext.CrearDiseno())
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            _apiClient = apiClient
            _sessionContext = If(sessionContext, UserSessionContext.CrearDiseno())

            _ribbon = New RibbonControl()
            _statusBar = New RibbonStatusBar()
            _statusInfo = New BarStaticItem()
            _statusModulo = New BarStaticItem()
            _btnTabUsuarios = New BarButtonItem() With {.Caption = "Usuarios"}
            _btnTabRoles = New BarButtonItem() With {.Caption = "Roles"}
            _btnTabAsignaciones = New BarButtonItem() With {.Caption = "Asignaciones"}
            _btnNuevo = New BarButtonItem() With {.Caption = "Nuevo"}
            _btnGuardar = New BarButtonItem() With {.Caption = "Guardar"}
            _btnRefrescar = New BarButtonItem() With {.Caption = "Refrescar"}
            _btnDesactivar = New BarButtonItem() With {.Caption = "Desactivar"}
            _btnLimpiar = New BarButtonItem() With {.Caption = "Limpiar editor"}

            _mainLayout = New LayoutControl()
            _tabs = New TabControl()

            _splitUsuarios = New SplitContainerControl()
            _txtBuscarUsuarios = New TextEdit()
            _gridUsuarios = New GridControl()
            _viewUsuarios = New GridView()
            _txtUsuarioCodigo = New TextEdit()
            _txtUsuarioLogin = New TextEdit()
            _txtUsuarioNombre = New TextEdit()
            _txtUsuarioApellido = New TextEdit()
            _txtUsuarioNombreMostrar = New TextEdit()
            _txtUsuarioCorreo = New TextEdit()
            _cmbUsuarioEstado = New ComboBoxEdit()
            _chkUsuarioActivo = New CheckEdit()
            _chkUsuarioMfa = New CheckEdit()
            _chkUsuarioCambioClave = New CheckEdit()

            _splitRoles = New SplitContainerControl()
            _txtBuscarRoles = New TextEdit()
            _gridRoles = New GridControl()
            _viewRoles = New GridView()
            _txtRolCodigo = New TextEdit()
            _txtRolNombre = New TextEdit()
            _memoRolDescripcion = New MemoEdit()
            _chkRolActivo = New CheckEdit()
            _chkRolSistema = New CheckEdit()

            _splitAsignaciones = New SplitContainerControl()
            _txtBuscarAsignaciones = New TextEdit()
            _gridAsignaciones = New GridControl()
            _viewAsignaciones = New GridView()
            _cmbAsignUsuario = New ComboBoxEdit()
            _cmbAsignRol = New ComboBoxEdit()
            _cmbAsignAlcance = New ComboBoxEdit()
            _dteAsignInicio = New DateEdit()
            _dteAsignFin = New DateEdit()
            _chkAsignActivo = New CheckEdit()

            _usuarios = New List(Of UsuarioDto)()
            _roles = New List(Of RolDto)()
            _asignaciones = New List(Of AsignacionRolUsuarioDto)()
            _estadosUsuario = New List(Of EstadoUsuarioDto)()
            _alcances = New List(Of AlcanceAsignacionDto)()

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
                Return "Centro IAM"
            End Get
        End Property

        Public Sub SetRibbonHostMode(ByVal hosted As Boolean)
            _ribbon.Visible = Not hosted
        End Sub

        Private Sub InitializeComponent()
            Text = "Centro IAM"
            FormBorderStyle = FormBorderStyle.None
            Dock = DockStyle.Fill
            AutoScaleMode = AutoScaleMode.Dpi

            ConfigureRibbon()
            ConfigureBody()
            ConfigureStatusBar()

            Controls.Add(_mainLayout)
            Controls.Add(_statusBar)
            Controls.Add(_ribbon)
        End Sub

        Private Sub ConfigureRibbon()
            _ribbon.Dock = DockStyle.Top
            _ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            _ribbon.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden
            _ribbon.MdiMergeStyle = RibbonMdiMergeStyle.Always

            AssignIcon(_btnTabUsuarios, "BusinessObjects.BOUser")
            AssignIcon(_btnTabRoles, "BusinessObjects.BORole")
            AssignIcon(_btnTabAsignaciones, "BusinessObjects.BOPosition")
            AssignIcon(_btnNuevo, "Actions.Add")
            AssignIcon(_btnGuardar, "Save.Save")
            AssignIcon(_btnRefrescar, "Actions.Refresh")
            AssignIcon(_btnDesactivar, "Actions.Cancel")
            AssignIcon(_btnLimpiar, "Actions.Clear")
            _btnTabUsuarios.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabRoles.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabAsignaciones.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnNuevo.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnGuardar.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnRefrescar.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnDesactivar.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnLimpiar.PaintStyle = BarItemPaintStyle.CaptionGlyph

            _ribbon.Items.AddRange(New BarItem() {
                _btnTabUsuarios, _btnTabRoles, _btnTabAsignaciones,
                _btnNuevo, _btnGuardar, _btnRefrescar, _btnDesactivar, _btnLimpiar,
                _statusInfo, _statusModulo
            })

            Dim page As New RibbonPage("Inicio")
            page.Name = "rpInicio"
            Dim groupSecciones As New RibbonPageGroup("Secciones")
            groupSecciones.ItemLinks.Add(_btnTabUsuarios)
            groupSecciones.ItemLinks.Add(_btnTabRoles)
            groupSecciones.ItemLinks.Add(_btnTabAsignaciones)

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

            _tabs.Dock = DockStyle.Fill
            _tabs.Padding = New Point(16, 6)

            Dim tabUsuarios As New TabPage("Usuarios")
            Dim tabRoles As New TabPage("Roles")
            Dim tabAsignaciones As New TabPage("Asignaciones")

            BuildUsuariosTab(tabUsuarios)
            BuildRolesTab(tabRoles)
            BuildAsignacionesTab(tabAsignaciones)

            _tabs.TabPages.Add(tabUsuarios)
            _tabs.TabPages.Add(tabRoles)
            _tabs.TabPages.Add(tabAsignaciones)

            _mainLayout.Controls.Add(_tabs)
            Dim item = DirectCast(_mainLayout.Root, LayoutControlGroup).AddItem(String.Empty, _tabs)
            item.TextVisible = False
        End Sub

        Private Sub ConfigureStatusBar()
            _statusBar.Dock = DockStyle.Bottom
            _statusBar.Ribbon = _ribbon
            _statusInfo.Caption = "Centro IAM listo."
            _statusModulo.Caption = "Seccion: Usuarios"

            _statusBar.ItemLinks.Add(_statusModulo)
            _statusBar.ItemLinks.Add(_statusInfo)
        End Sub
        Private Sub BuildUsuariosTab(ByVal tab As TabPage)
            _splitUsuarios.Dock = DockStyle.Fill
            _splitUsuarios.SplitterPosition = 640
            tab.Controls.Add(_splitUsuarios)

            Dim panelLeft As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitUsuarios.Panel1.Controls.Add(panelLeft)

            Dim pnlSearch As New PanelControl() With {.Dock = DockStyle.Top, .Height = 42, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar usuario:", .Location = New Point(8, 14)}
            _txtBuscarUsuarios.Location = New Point(96, 11)
            _txtBuscarUsuarios.Width = 320
            _txtBuscarUsuarios.Properties.NullValuePrompt = "Login, nombre o correo..."
            _txtBuscarUsuarios.Properties.NullValuePromptShowForEmptyValue = True
            pnlSearch.Controls.Add(lblSearch)
            pnlSearch.Controls.Add(_txtBuscarUsuarios)

            _gridUsuarios.Dock = DockStyle.Fill
            _gridUsuarios.MainView = _viewUsuarios
            _gridUsuarios.UseEmbeddedNavigator = True
            ConfigureGridView(_viewUsuarios)
            _gridUsuarios.DataSource = _usuarios

            panelLeft.Controls.Add(_gridUsuarios)
            panelLeft.Controls.Add(pnlSearch)

            Dim panelRight As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitUsuarios.Panel2.Controls.Add(panelRight)

            Dim layout As New LayoutControl() With {.Dock = DockStyle.Fill}
            panelRight.Controls.Add(layout)

            Dim lblInfo As New LabelControl() With {.Text = "Edicion contextual de usuario (sin abrir ventanas adicionales)."}
            lblInfo.Appearance.ForeColor = Color.DimGray

            _chkUsuarioActivo.Properties.Caption = "Activo"
            _chkUsuarioMfa.Properties.Caption = "MFA habilitado"
            _chkUsuarioCambioClave.Properties.Caption = "Requiere cambio de clave"
            _cmbUsuarioEstado.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor

            layout.Controls.Add(lblInfo)
            layout.Controls.Add(_txtUsuarioCodigo)
            layout.Controls.Add(_txtUsuarioLogin)
            layout.Controls.Add(_txtUsuarioNombre)
            layout.Controls.Add(_txtUsuarioApellido)
            layout.Controls.Add(_txtUsuarioNombreMostrar)
            layout.Controls.Add(_txtUsuarioCorreo)
            layout.Controls.Add(_cmbUsuarioEstado)
            layout.Controls.Add(_chkUsuarioActivo)
            layout.Controls.Add(_chkUsuarioMfa)
            layout.Controls.Add(_chkUsuarioCambioClave)

            Dim root As New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }
            layout.Root = root

            Dim iInfo = root.AddItem(String.Empty, lblInfo)
            iInfo.TextVisible = False
            iInfo.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 8, 10)

            root.AddItem("Codigo", _txtUsuarioCodigo)
            root.AddItem("Login*", _txtUsuarioLogin)
            root.AddItem("Nombre*", _txtUsuarioNombre)
            root.AddItem("Apellido", _txtUsuarioApellido)
            root.AddItem("Nombre mostrar*", _txtUsuarioNombreMostrar)
            root.AddItem("Correo", _txtUsuarioCorreo)
            root.AddItem("Estado*", _cmbUsuarioEstado)

            Dim iActivo = root.AddItem(String.Empty, _chkUsuarioActivo)
            iActivo.TextVisible = False
            Dim iMfa = root.AddItem(String.Empty, _chkUsuarioMfa)
            iMfa.TextVisible = False
            Dim iCambio = root.AddItem(String.Empty, _chkUsuarioCambioClave)
            iCambio.TextVisible = False

            ConfigureUsuarioColumns()
        End Sub

        Private Sub BuildRolesTab(ByVal tab As TabPage)
            _splitRoles.Dock = DockStyle.Fill
            _splitRoles.SplitterPosition = 620
            tab.Controls.Add(_splitRoles)

            Dim panelLeft As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitRoles.Panel1.Controls.Add(panelLeft)

            Dim pnlSearch As New PanelControl() With {.Dock = DockStyle.Top, .Height = 42, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar rol:", .Location = New Point(8, 14)}
            _txtBuscarRoles.Location = New Point(74, 11)
            _txtBuscarRoles.Width = 320
            _txtBuscarRoles.Properties.NullValuePrompt = "Codigo o nombre..."
            _txtBuscarRoles.Properties.NullValuePromptShowForEmptyValue = True
            pnlSearch.Controls.Add(lblSearch)
            pnlSearch.Controls.Add(_txtBuscarRoles)

            _gridRoles.Dock = DockStyle.Fill
            _gridRoles.MainView = _viewRoles
            _gridRoles.UseEmbeddedNavigator = True
            ConfigureGridView(_viewRoles)
            _gridRoles.DataSource = _roles

            panelLeft.Controls.Add(_gridRoles)
            panelLeft.Controls.Add(pnlSearch)

            Dim panelRight As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitRoles.Panel2.Controls.Add(panelRight)

            Dim layout As New LayoutControl() With {.Dock = DockStyle.Fill}
            panelRight.Controls.Add(layout)

            Dim lblInfo As New LabelControl() With {.Text = "Catalogo de roles por tenant y estado de activacion."}
            lblInfo.Appearance.ForeColor = Color.DimGray
            _chkRolActivo.Properties.Caption = "Activo"
            _chkRolSistema.Properties.Caption = "Rol de sistema"

            layout.Controls.Add(lblInfo)
            layout.Controls.Add(_txtRolCodigo)
            layout.Controls.Add(_txtRolNombre)
            layout.Controls.Add(_memoRolDescripcion)
            layout.Controls.Add(_chkRolActivo)
            layout.Controls.Add(_chkRolSistema)

            Dim root As New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }
            layout.Root = root

            Dim iInfo = root.AddItem(String.Empty, lblInfo)
            iInfo.TextVisible = False
            iInfo.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 8, 10)

            root.AddItem("Codigo*", _txtRolCodigo)
            root.AddItem("Nombre*", _txtRolNombre)
            root.AddItem("Descripcion", _memoRolDescripcion)

            Dim iActivo = root.AddItem(String.Empty, _chkRolActivo)
            iActivo.TextVisible = False
            Dim iSistema = root.AddItem(String.Empty, _chkRolSistema)
            iSistema.TextVisible = False

            ConfigureRolColumns()
        End Sub

        Private Sub BuildAsignacionesTab(ByVal tab As TabPage)
            _splitAsignaciones.Dock = DockStyle.Fill
            _splitAsignaciones.SplitterPosition = 700
            tab.Controls.Add(_splitAsignaciones)

            Dim panelLeft As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitAsignaciones.Panel1.Controls.Add(panelLeft)

            Dim pnlSearch As New PanelControl() With {.Dock = DockStyle.Top, .Height = 42, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar asignacion:", .Location = New Point(8, 14)}
            _txtBuscarAsignaciones.Location = New Point(106, 11)
            _txtBuscarAsignaciones.Width = 340
            _txtBuscarAsignaciones.Properties.NullValuePrompt = "Usuario, rol o alcance..."
            _txtBuscarAsignaciones.Properties.NullValuePromptShowForEmptyValue = True
            pnlSearch.Controls.Add(lblSearch)
            pnlSearch.Controls.Add(_txtBuscarAsignaciones)

            _gridAsignaciones.Dock = DockStyle.Fill
            _gridAsignaciones.MainView = _viewAsignaciones
            _gridAsignaciones.UseEmbeddedNavigator = True
            ConfigureGridView(_viewAsignaciones)

            panelLeft.Controls.Add(_gridAsignaciones)
            panelLeft.Controls.Add(pnlSearch)

            Dim panelRight As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitAsignaciones.Panel2.Controls.Add(panelRight)

            Dim layout As New LayoutControl() With {.Dock = DockStyle.Fill}
            panelRight.Controls.Add(layout)

            Dim lblInfo As New LabelControl() With {.Text = "Asignacion de roles sin salir de la pantalla principal."}
            lblInfo.Appearance.ForeColor = Color.DimGray
            _chkAsignActivo.Properties.Caption = "Activo"
            _cmbAsignUsuario.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _cmbAsignRol.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _cmbAsignAlcance.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _dteAsignInicio.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            _dteAsignInicio.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm"
            _dteAsignInicio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            _dteAsignInicio.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm"
            _dteAsignInicio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            _dteAsignFin.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            _dteAsignFin.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm"
            _dteAsignFin.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            _dteAsignFin.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm"
            _dteAsignFin.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime

            layout.Controls.Add(lblInfo)
            layout.Controls.Add(_cmbAsignUsuario)
            layout.Controls.Add(_cmbAsignRol)
            layout.Controls.Add(_cmbAsignAlcance)
            layout.Controls.Add(_dteAsignInicio)
            layout.Controls.Add(_dteAsignFin)
            layout.Controls.Add(_chkAsignActivo)

            Dim root As New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }
            layout.Root = root

            Dim iInfo = root.AddItem(String.Empty, lblInfo)
            iInfo.TextVisible = False
            iInfo.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 8, 10)

            root.AddItem("Usuario*", _cmbAsignUsuario)
            root.AddItem("Rol*", _cmbAsignRol)
            root.AddItem("Alcance*", _cmbAsignAlcance)
            root.AddItem("Inicio UTC*", _dteAsignInicio)
            root.AddItem("Fin UTC", _dteAsignFin)

            Dim iActivo = root.AddItem(String.Empty, _chkAsignActivo)
            iActivo.TextVisible = False

            ConfigureAsignacionColumns()
        End Sub

        Private Sub ConfigureGridView(ByVal view As GridView)
            view.OptionsBehavior.Editable = False
            view.OptionsView.ColumnAutoWidth = False
            view.OptionsView.ShowGroupPanel = False
            view.OptionsView.ShowAutoFilterRow = True
            view.OptionsSelection.EnableAppearanceFocusedCell = False
            view.FocusRectStyle = DrawFocusRectStyle.RowFocus
            view.OptionsCustomization.AllowSort = True
            view.OptionsCustomization.AllowFilter = True
        End Sub

        Private Sub ConfigureUsuarioColumns()
            _viewUsuarios.Columns.Clear()
            _viewUsuarios.Columns.AddVisible(NameOf(UsuarioDto.IdUsuario), "Id").Width = 70
            _viewUsuarios.Columns.AddVisible(NameOf(UsuarioDto.LoginPrincipal), "Login").Width = 160
            _viewUsuarios.Columns.AddVisible(NameOf(UsuarioDto.NombreMostrar), "Nombre").Width = 220
            _viewUsuarios.Columns.AddVisible(NameOf(UsuarioDto.CorreoElectronico), "Correo").Width = 240
            _viewUsuarios.Columns.AddVisible(NameOf(UsuarioDto.IdEstadoUsuario), "EstadoId").Width = 90
            _viewUsuarios.Columns.AddVisible(NameOf(UsuarioDto.Activo), "Activo").Width = 80
        End Sub

        Private Sub ConfigureRolColumns()
            _viewRoles.Columns.Clear()
            _viewRoles.Columns.AddVisible(NameOf(RolDto.IdRol), "Id").Width = 70
            _viewRoles.Columns.AddVisible(NameOf(RolDto.Codigo), "Codigo").Width = 160
            _viewRoles.Columns.AddVisible(NameOf(RolDto.Nombre), "Nombre").Width = 240
            _viewRoles.Columns.AddVisible(NameOf(RolDto.EsSistema), "Sistema").Width = 90
            _viewRoles.Columns.AddVisible(NameOf(RolDto.Activo), "Activo").Width = 80
        End Sub

        Private Sub ConfigureAsignacionColumns()
            _viewAsignaciones.Columns.Clear()
            _viewAsignaciones.Columns.AddVisible(NameOf(AsignacionGridRow.IdAsignacionRolUsuario), "Id").Width = 70
            _viewAsignaciones.Columns.AddVisible(NameOf(AsignacionGridRow.Usuario), "Usuario").Width = 190
            _viewAsignaciones.Columns.AddVisible(NameOf(AsignacionGridRow.Rol), "Rol").Width = 200
            _viewAsignaciones.Columns.AddVisible(NameOf(AsignacionGridRow.Alcance), "Alcance").Width = 140
            _viewAsignaciones.Columns.AddVisible(NameOf(AsignacionGridRow.FechaInicioUtc), "Inicio").Width = 150
            _viewAsignaciones.Columns.AddVisible(NameOf(AsignacionGridRow.FechaFinUtc), "Fin").Width = 150
            _viewAsignaciones.Columns.AddVisible(NameOf(AsignacionGridRow.Activo), "Activo").Width = 80
        End Sub
        Private Sub WireEvents()
            AddHandler Shown, AddressOf OnFirstShown
            AddHandler _tabs.SelectedIndexChanged, AddressOf OnTabChanged
            AddHandler _btnTabUsuarios.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 0
            AddHandler _btnTabRoles.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 1
            AddHandler _btnTabAsignaciones.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 2
            AddHandler _btnNuevo.ItemClick, AddressOf OnNuevoClick
            AddHandler _btnGuardar.ItemClick, AddressOf OnGuardarClickAsync
            AddHandler _btnRefrescar.ItemClick, AddressOf OnRefrescarClickAsync
            AddHandler _btnDesactivar.ItemClick, AddressOf OnDesactivarClickAsync
            AddHandler _btnLimpiar.ItemClick, AddressOf OnLimpiarClick

            AddHandler _txtBuscarUsuarios.EditValueChanged, Sub(sender, e) _viewUsuarios.FindFilterText = _txtBuscarUsuarios.Text
            AddHandler _txtBuscarRoles.EditValueChanged, Sub(sender, e) _viewRoles.FindFilterText = _txtBuscarRoles.Text
            AddHandler _txtBuscarAsignaciones.EditValueChanged, Sub(sender, e) _viewAsignaciones.FindFilterText = _txtBuscarAsignaciones.Text

            AddHandler _viewUsuarios.FocusedRowChanged, AddressOf OnUsuarioFocusedRowChanged
            AddHandler _viewRoles.FocusedRowChanged, AddressOf OnRolFocusedRowChanged
            AddHandler _viewAsignaciones.FocusedRowChanged, AddressOf OnAsignacionFocusedRowChanged
        End Sub

        Private Async Sub OnFirstShown(ByVal sender As Object, ByVal e As EventArgs)
            If _firstLoadDone Then Return
            _firstLoadDone = True

            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If

            Await LoadInitialDataAsync().ConfigureAwait(True)
        End Sub

        Private Sub OnTabChanged(ByVal sender As Object, ByVal e As EventArgs)
            _statusModulo.Caption = "Seccion: " & GetCurrentSectionName()
        End Sub

        Private Function GetCurrentSectionName() As String
            Select Case _tabs.SelectedIndex
                Case 0
                    Return "Usuarios"
                Case 1
                    Return "Roles"
                Case Else
                    Return "Asignaciones"
            End Select
        End Function

        Private Async Function LoadInitialDataAsync() As Task
            SetBusy(True, "Cargando catalogos IAM...")
            Try
                Await LoadCatalogosAsync().ConfigureAwait(True)
                Await LoadUsuariosAsync().ConfigureAwait(True)
                Await LoadRolesAsync().ConfigureAwait(True)
                Await LoadAsignacionesAsync().ConfigureAwait(True)
                ResetUsuarioEditor()
                ResetRolEditor()
                ResetAsignacionEditor()
                _statusInfo.Caption = "Centro IAM cargado."
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Centro IAM", MessageBoxButtons.OK, MessageBoxIcon.Error)
                _statusInfo.Caption = "Error al cargar datos IAM."
            Finally
                SetBusy(False)
            End Try
        End Function

        Private Async Function LoadCatalogosAsync() As Task
            Dim estados = Await _apiClient.GetAsync(Of List(Of EstadoUsuarioDto))("api/v1/catalogo/estado_usuario").ConfigureAwait(True)
            Dim alcances = Await _apiClient.GetAsync(Of List(Of AlcanceAsignacionDto))("api/v1/catalogo/alcance_asignacion").ConfigureAwait(True)

            _estadosUsuario = If(estados, New List(Of EstadoUsuarioDto)()).
                Where(Function(x) x IsNot Nothing AndAlso x.IdEstadoUsuario.HasValue).
                OrderBy(Function(x) x.OrdenVisual.GetValueOrDefault()).
                ToList()

            _alcances = If(alcances, New List(Of AlcanceAsignacionDto)()).
                Where(Function(x) x IsNot Nothing AndAlso x.IdAlcanceAsignacion.HasValue).
                OrderBy(Function(x) x.OrdenVisual.GetValueOrDefault()).
                ToList()

            _cmbUsuarioEstado.Properties.Items.Clear()
            For Each item In _estadosUsuario
                _cmbUsuarioEstado.Properties.Items.Add(New ComboShortItem With {
                    .Id = item.IdEstadoUsuario.Value,
                    .Nombre = item.Nombre
                })
            Next

            _cmbAsignAlcance.Properties.Items.Clear()
            For Each item In _alcances
                _cmbAsignAlcance.Properties.Items.Add(New ComboShortItem With {
                    .Id = item.IdAlcanceAsignacion.Value,
                    .Nombre = item.Nombre
                })
            Next
        End Function

        Private Async Function LoadUsuariosAsync() As Task
            SetBusy(True, "Cargando usuarios...")
            Dim list = Await _apiClient.GetAsync(Of List(Of UsuarioDto))("api/v1/seguridad/usuario").ConfigureAwait(True)
            _usuarios = If(list, New List(Of UsuarioDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderBy(Function(x) If(x.NombreMostrar, x.LoginPrincipal)).
                ToList()

            _gridUsuarios.DataSource = _usuarios
            BuildUsuariosCombo()
            SetBusy(False)
        End Function

        Private Async Function LoadRolesAsync() As Task
            SetBusy(True, "Cargando roles...")
            Dim list = Await _apiClient.GetAsync(Of List(Of RolDto))("api/v1/seguridad/rol").ConfigureAwait(True)
            _roles = If(list, New List(Of RolDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderBy(Function(x) x.Nombre).
                ToList()

            _gridRoles.DataSource = _roles
            BuildRolesCombo()
            SetBusy(False)
        End Function

        Private Async Function LoadAsignacionesAsync() As Task
            SetBusy(True, "Cargando asignaciones...")
            Dim list = Await _apiClient.GetAsync(Of List(Of AsignacionRolUsuarioDto))("api/v1/seguridad/asignacion_rol_usuario").ConfigureAwait(True)
            _asignaciones = If(list, New List(Of AsignacionRolUsuarioDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderByDescending(Function(x) x.IdAsignacionRolUsuario.GetValueOrDefault()).
                ToList()

            _gridAsignaciones.DataSource = BuildAsignacionRows(_asignaciones)
            SetBusy(False)
        End Function

        Private Sub BuildUsuariosCombo()
            _cmbAsignUsuario.Properties.Items.Clear()
            For Each u In _usuarios.Where(Function(x) x.IdUsuario.HasValue)
                _cmbAsignUsuario.Properties.Items.Add(New ComboLongItem With {
                    .Id = u.IdUsuario.Value,
                    .Nombre = If(String.IsNullOrWhiteSpace(u.NombreMostrar), u.LoginPrincipal, u.NombreMostrar)
                })
            Next
        End Sub

        Private Sub BuildRolesCombo()
            _cmbAsignRol.Properties.Items.Clear()
            For Each r In _roles.Where(Function(x) x.IdRol.HasValue)
                _cmbAsignRol.Properties.Items.Add(New ComboLongItem With {
                    .Id = r.IdRol.Value,
                    .Nombre = r.Nombre
                })
            Next
        End Sub

        Private Function BuildAsignacionRows(ByVal source As IEnumerable(Of AsignacionRolUsuarioDto)) As List(Of AsignacionGridRow)
            Dim userMap = _usuarios.Where(Function(u) u.IdUsuario.HasValue).ToDictionary(Function(u) u.IdUsuario.Value, Function(u) If(String.IsNullOrWhiteSpace(u.NombreMostrar), u.LoginPrincipal, u.NombreMostrar))
            Dim roleMap = _roles.Where(Function(r) r.IdRol.HasValue).ToDictionary(Function(r) r.IdRol.Value, Function(r) r.Nombre)
            Dim alcanceMap = _alcances.Where(Function(a) a.IdAlcanceAsignacion.HasValue).ToDictionary(Function(a) a.IdAlcanceAsignacion.Value, Function(a) a.Nombre)

            Dim rows As New List(Of AsignacionGridRow)()
            For Each item In source
                If Not item.IdAsignacionRolUsuario.HasValue Then Continue For
                Dim usuarioTexto = If(item.IdUsuario.HasValue AndAlso userMap.ContainsKey(item.IdUsuario.Value), userMap(item.IdUsuario.Value), "Usuario #" & item.IdUsuario.GetValueOrDefault().ToString())
                Dim rolTexto = If(item.IdRol.HasValue AndAlso roleMap.ContainsKey(item.IdRol.Value), roleMap(item.IdRol.Value), "Rol #" & item.IdRol.GetValueOrDefault().ToString())
                Dim alcanceTexto = If(item.IdAlcanceAsignacion.HasValue AndAlso alcanceMap.ContainsKey(item.IdAlcanceAsignacion.Value), alcanceMap(item.IdAlcanceAsignacion.Value), "Alcance #" & item.IdAlcanceAsignacion.GetValueOrDefault().ToString())

                rows.Add(New AsignacionGridRow With {
                    .IdAsignacionRolUsuario = item.IdAsignacionRolUsuario.Value,
                    .Usuario = usuarioTexto,
                    .Rol = rolTexto,
                    .Alcance = alcanceTexto,
                    .FechaInicioUtc = item.FechaInicioUtc,
                    .FechaFinUtc = item.FechaFinUtc,
                    .Activo = item.Activo.GetValueOrDefault(True)
                })
            Next

            Return rows
        End Function
        Private Sub OnUsuarioFocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
            If _loadingEditors Then Return
            Dim dto = TryCast(_viewUsuarios.GetFocusedRow(), UsuarioDto)
            If dto Is Nothing Then Return
            LoadUsuarioEditor(dto)
        End Sub

        Private Sub OnRolFocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
            If _loadingEditors Then Return
            Dim dto = TryCast(_viewRoles.GetFocusedRow(), RolDto)
            If dto Is Nothing Then Return
            LoadRolEditor(dto)
        End Sub

        Private Sub OnAsignacionFocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
            If _loadingEditors Then Return
            Dim row = TryCast(_viewAsignaciones.GetFocusedRow(), AsignacionGridRow)
            If row Is Nothing Then Return

            Dim dto = _asignaciones.FirstOrDefault(Function(x) x.IdAsignacionRolUsuario.HasValue AndAlso x.IdAsignacionRolUsuario.Value = row.IdAsignacionRolUsuario)
            If dto Is Nothing Then Return
            LoadAsignacionEditor(dto)
        End Sub

        Private Sub LoadUsuarioEditor(ByVal dto As UsuarioDto)
            _loadingEditors = True
            Try
                _editingUsuarioId = dto.IdUsuario
                _usuarioCreadoUtc = dto.CreadoUtc
                _txtUsuarioCodigo.Text = If(dto.Codigo, String.Empty)
                _txtUsuarioLogin.Text = If(dto.LoginPrincipal, String.Empty)
                _txtUsuarioNombre.Text = If(dto.Nombre, String.Empty)
                _txtUsuarioApellido.Text = If(dto.Apellido, String.Empty)
                _txtUsuarioNombreMostrar.Text = If(dto.NombreMostrar, String.Empty)
                _txtUsuarioCorreo.Text = If(dto.CorreoElectronico, String.Empty)
                _chkUsuarioActivo.Checked = dto.Activo.GetValueOrDefault(True)
                _chkUsuarioMfa.Checked = dto.MfaHabilitado.GetValueOrDefault(False)
                _chkUsuarioCambioClave.Checked = dto.RequiereCambioClave.GetValueOrDefault(False)

                SelectComboShortItem(_cmbUsuarioEstado, dto.IdEstadoUsuario)
            Finally
                _loadingEditors = False
            End Try
        End Sub

        Private Sub LoadRolEditor(ByVal dto As RolDto)
            _loadingEditors = True
            Try
                _editingRolId = dto.IdRol
                _rolCreadoUtc = dto.CreadoUtc
                _txtRolCodigo.Text = If(dto.Codigo, String.Empty)
                _txtRolNombre.Text = If(dto.Nombre, String.Empty)
                _memoRolDescripcion.Text = If(dto.Descripcion, String.Empty)
                _chkRolActivo.Checked = dto.Activo.GetValueOrDefault(True)
                _chkRolSistema.Checked = dto.EsSistema.GetValueOrDefault(False)
            Finally
                _loadingEditors = False
            End Try
        End Sub

        Private Sub LoadAsignacionEditor(ByVal dto As AsignacionRolUsuarioDto)
            _loadingEditors = True
            Try
                _editingAsignacionId = dto.IdAsignacionRolUsuario
                _asignacionCreadoUtc = dto.CreadoUtc
                SelectComboLongItem(_cmbAsignUsuario, dto.IdUsuario)
                SelectComboLongItem(_cmbAsignRol, dto.IdRol)
                SelectComboShortItem(_cmbAsignAlcance, dto.IdAlcanceAsignacion)
                _dteAsignInicio.EditValue = If(dto.FechaInicioUtc.HasValue, dto.FechaInicioUtc.Value, CType(DateTime.UtcNow, Object))
                _dteAsignFin.EditValue = If(dto.FechaFinUtc.HasValue, dto.FechaFinUtc.Value, Nothing)
                _chkAsignActivo.Checked = dto.Activo.GetValueOrDefault(True)
            Finally
                _loadingEditors = False
            End Try
        End Sub

        Private Sub ResetUsuarioEditor()
            _loadingEditors = True
            Try
                _editingUsuarioId = Nothing
                _usuarioCreadoUtc = Nothing
                _txtUsuarioCodigo.Text = String.Empty
                _txtUsuarioLogin.Text = String.Empty
                _txtUsuarioNombre.Text = String.Empty
                _txtUsuarioApellido.Text = String.Empty
                _txtUsuarioNombreMostrar.Text = String.Empty
                _txtUsuarioCorreo.Text = String.Empty
                _chkUsuarioActivo.Checked = True
                _chkUsuarioMfa.Checked = False
                _chkUsuarioCambioClave.Checked = False
                If _cmbUsuarioEstado.Properties.Items.Count > 0 Then
                    _cmbUsuarioEstado.SelectedIndex = 0
                End If
            Finally
                _loadingEditors = False
            End Try
        End Sub

        Private Sub ResetRolEditor()
            _loadingEditors = True
            Try
                _editingRolId = Nothing
                _rolCreadoUtc = Nothing
                _txtRolCodigo.Text = String.Empty
                _txtRolNombre.Text = String.Empty
                _memoRolDescripcion.Text = String.Empty
                _chkRolActivo.Checked = True
                _chkRolSistema.Checked = False
            Finally
                _loadingEditors = False
            End Try
        End Sub

        Private Sub ResetAsignacionEditor()
            _loadingEditors = True
            Try
                _editingAsignacionId = Nothing
                _asignacionCreadoUtc = Nothing
                If _cmbAsignUsuario.Properties.Items.Count > 0 Then _cmbAsignUsuario.SelectedIndex = 0
                If _cmbAsignRol.Properties.Items.Count > 0 Then _cmbAsignRol.SelectedIndex = 0
                If _cmbAsignAlcance.Properties.Items.Count > 0 Then _cmbAsignAlcance.SelectedIndex = 0
                _dteAsignInicio.EditValue = DateTime.UtcNow
                _dteAsignFin.EditValue = Nothing
                _chkAsignActivo.Checked = True
            Finally
                _loadingEditors = False
            End Try
        End Sub

        Private Sub OnNuevoClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return

            Select Case _tabs.SelectedIndex
                Case 0
                    ResetUsuarioEditor()
                Case 1
                    ResetRolEditor()
                Case Else
                    ResetAsignacionEditor()
            End Select
        End Sub

        Private Async Sub OnGuardarClickAsync(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return
            Try
                Select Case _tabs.SelectedIndex
                    Case 0
                        Await SaveUsuarioAsync().ConfigureAwait(True)
                    Case 1
                        Await SaveRolAsync().ConfigureAwait(True)
                    Case Else
                        Await SaveAsignacionAsync().ConfigureAwait(True)
                End Select
            Catch ex As ApiClientException
                Dim detalle = If(String.IsNullOrWhiteSpace(ex.ResponseBody), ex.Message, ex.ResponseBody)
                XtraMessageBox.Show(Me, detalle, "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Async Sub OnRefrescarClickAsync(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return
            Try
                Select Case _tabs.SelectedIndex
                    Case 0
                        Await LoadUsuariosAsync().ConfigureAwait(True)
                    Case 1
                        Await LoadRolesAsync().ConfigureAwait(True)
                    Case Else
                        Await LoadAsignacionesAsync().ConfigureAwait(True)
                End Select

                _statusInfo.Caption = "Datos actualizados."
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Refrescar", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Async Sub OnDesactivarClickAsync(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return

            If XtraMessageBox.Show(Me, "Confirma desactivar el registro seleccionado?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            Try
                Select Case _tabs.SelectedIndex
                    Case 0
                        If Not _editingUsuarioId.HasValue Then Return
                        SetBusy(True, "Desactivando usuario...")
                        Await _apiClient.DeleteAsync("api/v1/seguridad/usuario/" & _editingUsuarioId.Value.ToString()).ConfigureAwait(True)
                        Await LoadUsuariosAsync().ConfigureAwait(True)
                        Await LoadAsignacionesAsync().ConfigureAwait(True)
                        ResetUsuarioEditor()
                    Case 1
                        If Not _editingRolId.HasValue Then Return
                        SetBusy(True, "Desactivando rol...")
                        Await _apiClient.DeleteAsync("api/v1/seguridad/rol/" & _editingRolId.Value.ToString()).ConfigureAwait(True)
                        Await LoadRolesAsync().ConfigureAwait(True)
                        Await LoadAsignacionesAsync().ConfigureAwait(True)
                        ResetRolEditor()
                    Case Else
                        If Not _editingAsignacionId.HasValue Then Return
                        SetBusy(True, "Desactivando asignacion...")
                        Await _apiClient.DeleteAsync("api/v1/seguridad/asignacion_rol_usuario/" & _editingAsignacionId.Value.ToString()).ConfigureAwait(True)
                        Await LoadAsignacionesAsync().ConfigureAwait(True)
                        ResetAsignacionEditor()
                End Select
                _statusInfo.Caption = "Registro desactivado correctamente."
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                SetBusy(False)
            End Try
        End Sub

        Private Sub OnLimpiarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return
            OnNuevoClick(sender, e)
        End Sub

        Private Async Function SaveUsuarioAsync() As Task
            Dim login = _txtUsuarioLogin.Text.Trim()
            Dim nombre = _txtUsuarioNombre.Text.Trim()
            Dim nombreMostrar = _txtUsuarioNombreMostrar.Text.Trim()
            Dim estadoId = GetSelectedShortId(_cmbUsuarioEstado)

            If String.IsNullOrWhiteSpace(login) Then
                XtraMessageBox.Show(Me, "Ingrese el login del usuario.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtUsuarioLogin.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(nombre) Then
                XtraMessageBox.Show(Me, "Ingrese el nombre del usuario.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtUsuarioNombre.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(nombreMostrar) Then
                nombreMostrar = nombre
                _txtUsuarioNombreMostrar.Text = nombreMostrar
            End If

            If Not estadoId.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione un estado para el usuario.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _cmbUsuarioEstado.Focus()
                Return
            End If

            Dim now = DateTime.UtcNow
            Dim correo = NullIfWhiteSpace(_txtUsuarioCorreo.Text)
            Dim dto As New UsuarioDto With {
                .IdUsuario = _editingUsuarioId,
                .Codigo = NullIfWhiteSpace(_txtUsuarioCodigo.Text),
                .LoginPrincipal = login,
                .LoginNormalizado = login.ToUpperInvariant(),
                .Nombre = nombre,
                .Apellido = NullIfWhiteSpace(_txtUsuarioApellido.Text),
                .NombreMostrar = nombreMostrar,
                .CorreoElectronico = correo,
                .CorreoNormalizado = If(String.IsNullOrWhiteSpace(correo), Nothing, correo.ToUpperInvariant()),
                .TelefonoMovil = Nothing,
                .Idioma = "es-GT",
                .ZonaHoraria = "America/Guatemala",
                .IdEstadoUsuario = estadoId.Value,
                .BloqueadoHastaUtc = Nothing,
                .MfaHabilitado = _chkUsuarioMfa.Checked,
                .RequiereCambioClave = _chkUsuarioCambioClave.Checked,
                .UltimoAccesoUtc = Nothing,
                .Activo = _chkUsuarioActivo.Checked,
                .CreadoPor = _sessionContext.IdUsuario,
                .CreadoUtc = If(_editingUsuarioId.HasValue AndAlso _usuarioCreadoUtc.HasValue, _usuarioCreadoUtc.Value, now),
                .ActualizadoPor = _sessionContext.IdUsuario,
                .ActualizadoUtc = now
            }

            SetBusy(True, "Guardando usuario...")
            If _editingUsuarioId.HasValue Then
                Await _apiClient.PutAsync("api/v1/seguridad/usuario/" & _editingUsuarioId.Value.ToString(), dto).ConfigureAwait(True)
            Else
                Await _apiClient.PostAsync(Of UsuarioDto, Dictionary(Of String, Object))("api/v1/seguridad/usuario", dto).ConfigureAwait(True)
            End If

            Await LoadUsuariosAsync().ConfigureAwait(True)
            Await LoadAsignacionesAsync().ConfigureAwait(True)
            _statusInfo.Caption = "Usuario guardado correctamente."
            SetBusy(False)
        End Function
        Private Async Function SaveRolAsync() As Task
            If Not _sessionContext.IdTenant.HasValue Then
                XtraMessageBox.Show(Me, "La sesion no tiene id_tenant para guardar roles.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim codigo = _txtRolCodigo.Text.Trim()
            Dim nombre = _txtRolNombre.Text.Trim()
            If String.IsNullOrWhiteSpace(codigo) Then
                XtraMessageBox.Show(Me, "Ingrese el codigo del rol.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtRolCodigo.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(nombre) Then
                XtraMessageBox.Show(Me, "Ingrese el nombre del rol.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtRolNombre.Focus()
                Return
            End If

            Dim now = DateTime.UtcNow
            Dim dto As New RolDto With {
                .IdRol = _editingRolId,
                .IdTenant = _sessionContext.IdTenant.Value,
                .Codigo = codigo,
                .Nombre = nombre,
                .Descripcion = NullIfWhiteSpace(_memoRolDescripcion.Text),
                .EsSistema = _chkRolSistema.Checked,
                .Activo = _chkRolActivo.Checked,
                .CreadoUtc = If(_editingRolId.HasValue AndAlso _rolCreadoUtc.HasValue, _rolCreadoUtc.Value, now),
                .ActualizadoUtc = now
            }

            SetBusy(True, "Guardando rol...")
            If _editingRolId.HasValue Then
                Await _apiClient.PutAsync("api/v1/seguridad/rol/" & _editingRolId.Value.ToString(), dto).ConfigureAwait(True)
            Else
                Await _apiClient.PostAsync(Of RolDto, Dictionary(Of String, Object))("api/v1/seguridad/rol", dto).ConfigureAwait(True)
            End If

            Await LoadRolesAsync().ConfigureAwait(True)
            Await LoadAsignacionesAsync().ConfigureAwait(True)
            _statusInfo.Caption = "Rol guardado correctamente."
            SetBusy(False)
        End Function

        Private Async Function SaveAsignacionAsync() As Task
            If Not _sessionContext.IdTenant.HasValue Then
                XtraMessageBox.Show(Me, "La sesion no tiene id_tenant para guardar asignaciones.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim idUsuario = GetSelectedLongId(_cmbAsignUsuario)
            Dim idRol = GetSelectedLongId(_cmbAsignRol)
            Dim idAlcance = GetSelectedShortId(_cmbAsignAlcance)

            If Not idUsuario.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione un usuario.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not idRol.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione un rol.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not idAlcance.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione un alcance de asignacion.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim fechaInicio As DateTime = DateTime.UtcNow
            If _dteAsignInicio.EditValue IsNot Nothing Then
                fechaInicio = Convert.ToDateTime(_dteAsignInicio.EditValue)
            End If

            Dim fechaFin As DateTime? = Nothing
            If _dteAsignFin.EditValue IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(_dteAsignFin.Text) Then
                fechaFin = Convert.ToDateTime(_dteAsignFin.EditValue)
            End If

            Dim now = DateTime.UtcNow
            Dim dto As New AsignacionRolUsuarioDto With {
                .IdAsignacionRolUsuario = _editingAsignacionId,
                .IdUsuario = idUsuario.Value,
                .IdTenant = _sessionContext.IdTenant.Value,
                .IdRol = idRol.Value,
                .IdAlcanceAsignacion = idAlcance.Value,
                .IdGrupoEmpresarial = Nothing,
                .IdEmpresa = _sessionContext.IdEmpresa,
                .IdUnidadOrganizativa = Nothing,
                .FechaInicioUtc = fechaInicio,
                .FechaFinUtc = fechaFin,
                .ConcedidoPor = _sessionContext.IdUsuario,
                .Activo = _chkAsignActivo.Checked,
                .CreadoUtc = If(_editingAsignacionId.HasValue AndAlso _asignacionCreadoUtc.HasValue, _asignacionCreadoUtc.Value, now),
                .ActualizadoUtc = now
            }

            SetBusy(True, "Guardando asignacion...")
            If _editingAsignacionId.HasValue Then
                Await _apiClient.PutAsync("api/v1/seguridad/asignacion_rol_usuario/" & _editingAsignacionId.Value.ToString(), dto).ConfigureAwait(True)
            Else
                Await _apiClient.PostAsync(Of AsignacionRolUsuarioDto, Dictionary(Of String, Object))("api/v1/seguridad/asignacion_rol_usuario", dto).ConfigureAwait(True)
            End If

            Await LoadAsignacionesAsync().ConfigureAwait(True)
            _statusInfo.Caption = "Asignacion guardada correctamente."
            SetBusy(False)
        End Function

        Private Sub SetBusy(ByVal busy As Boolean, Optional ByVal message As String = Nothing)
            _busy = busy
            _ribbon.Enabled = Not busy
            Cursor = If(busy, Cursors.WaitCursor, Cursors.Default)
            If Not String.IsNullOrWhiteSpace(message) Then
                _statusInfo.Caption = message
            End If
        End Sub

        Private Shared Function NullIfWhiteSpace(ByVal text As String) As String
            If String.IsNullOrWhiteSpace(text) Then Return Nothing
            Return text.Trim()
        End Function

        Private Shared Sub SelectComboLongItem(ByVal combo As ComboBoxEdit, ByVal id As Long?)
            If Not id.HasValue Then
                combo.SelectedItem = Nothing
                Return
            End If

            For Each obj In combo.Properties.Items
                Dim item = TryCast(obj, ComboLongItem)
                If item IsNot Nothing AndAlso item.Id = id.Value Then
                    combo.SelectedItem = item
                    Exit Sub
                End If
            Next
        End Sub

        Private Shared Sub SelectComboShortItem(ByVal combo As ComboBoxEdit, ByVal id As Short?)
            If Not id.HasValue Then
                combo.SelectedItem = Nothing
                Return
            End If

            For Each obj In combo.Properties.Items
                Dim item = TryCast(obj, ComboShortItem)
                If item IsNot Nothing AndAlso item.Id = id.Value Then
                    combo.SelectedItem = item
                    Exit Sub
                End If
            Next
        End Sub

        Private Shared Function GetSelectedLongId(ByVal combo As ComboBoxEdit) As Long?
            Dim item = TryCast(combo.SelectedItem, ComboLongItem)
            If item Is Nothing Then Return Nothing
            Return item.Id
        End Function

        Private Shared Function GetSelectedShortId(ByVal combo As ComboBoxEdit) As Short?
            Dim item = TryCast(combo.SelectedItem, ComboShortItem)
            If item Is Nothing Then Return Nothing
            Return item.Id
        End Function

        Private Shared Sub AssignIcon(ByVal item As BarButtonItem, ByVal iconKey As String)
            Dim svg = TryCast(IconService.GetIcon(iconKey), SvgImage)
            If svg IsNot Nothing Then
                item.ImageOptions.SvgImage = svg
            End If
        End Sub
    End Class
End Namespace
