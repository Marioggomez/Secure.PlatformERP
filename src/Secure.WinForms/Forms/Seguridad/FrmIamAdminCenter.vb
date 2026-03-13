Imports System.Linq
Imports System.ComponentModel
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout
Imports Secure.Platform.Contracts.Dtos.Organizacion
Imports Secure.Platform.Contracts.Dtos.Catalogo
Imports Secure.Platform.Contracts.Dtos.Observabilidad
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

        Private NotInheritable Class SesionGridRow
            Public Property IdSesionUsuario As Guid
            Public Property Usuario As String = String.Empty
            Public Property Empresa As String = String.Empty
            Public Property Origen As String = String.Empty
            Public Property MfaValidado As Boolean
            Public Property Activo As Boolean
            Public Property UltimaActividadUtc As DateTime?
            Public Property ExpiraAbsolutaUtc As DateTime?
            Public Property IpOrigen As String = String.Empty
        End Class

        Private NotInheritable Class HelpdeskGridRow
            Public Property IdAuditoria As Long
            Public Property UsuarioAfectado As String = String.Empty
            Public Property Administrador As String = String.Empty
            Public Property Motivo As String = String.Empty
            Public Property FechaUtc As DateTime?
            Public Property Empresa As String = String.Empty
        End Class

        Private NotInheritable Class PermisoMatrixRow
            Public Property IdPermiso As Integer
            Public Property IdDeber As Long?
            Public Property Asignado As Boolean
            Public Property Codigo As String = String.Empty
            Public Property Modulo As String = String.Empty
            Public Property Accion As String = String.Empty
            Public Property Nombre As String = String.Empty
            Public Property Sensible As Boolean
            Public Property Activo As Boolean
        End Class

        Private NotInheritable Class MenuMatrixRow
            Public Property IdRecursoUi As Long
            Public Property Asignado As Boolean
            Public Property Codigo As String = String.Empty
            Public Property Nombre As String = String.Empty
            Public Property Ruta As String = String.Empty
            Public Property Componente As String = String.Empty
            Public Property Visible As Boolean
            Public Property Activo As Boolean
            Public Property Permisos As String = String.Empty
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
        Private ReadOnly _btnTabPermisos As BarButtonItem
        Private ReadOnly _btnTabMenu As BarButtonItem
        Private ReadOnly _btnTabSesiones As BarButtonItem
        Private ReadOnly _btnTabHelpdesk As BarButtonItem
        Private ReadOnly _btnTabAuditoria As BarButtonItem
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

        Private ReadOnly _splitPermisos As SplitContainerControl
        Private ReadOnly _txtBuscarPermisos As TextEdit
        Private ReadOnly _cmbPermisosRol As ComboBoxEdit
        Private ReadOnly _btnPermisoConceder As SimpleButton
        Private ReadOnly _btnPermisoRevocar As SimpleButton
        Private ReadOnly _gridPermisos As GridControl
        Private ReadOnly _viewPermisos As GridView

        Private ReadOnly _splitMenu As SplitContainerControl
        Private ReadOnly _txtBuscarMenu As TextEdit
        Private ReadOnly _cmbMenuRol As ComboBoxEdit
        Private ReadOnly _btnMenuConceder As SimpleButton
        Private ReadOnly _btnMenuRevocar As SimpleButton
        Private ReadOnly _gridMenu As GridControl
        Private ReadOnly _viewMenu As GridView

        Private ReadOnly _splitSesiones As SplitContainerControl
        Private ReadOnly _txtBuscarSesiones As TextEdit
        Private ReadOnly _gridSesiones As GridControl
        Private ReadOnly _viewSesiones As GridView

        Private ReadOnly _splitHelpdesk As SplitContainerControl
        Private ReadOnly _cmbHelpdeskUsuario As ComboBoxEdit
        Private ReadOnly _cmbHelpdeskAprobador As ComboBoxEdit
        Private ReadOnly _chkHelpdeskCritico As CheckEdit
        Private ReadOnly _memoHelpdeskMotivo As MemoEdit
        Private ReadOnly _btnHelpdeskRegistrar As SimpleButton
        Private ReadOnly _txtBuscarHelpdesk As TextEdit
        Private ReadOnly _gridHelpdesk As GridControl
        Private ReadOnly _viewHelpdesk As GridView

        Private ReadOnly _splitAuditoria As SplitContainerControl
        Private ReadOnly _txtBuscarAuditoria As TextEdit
        Private ReadOnly _cmbSimUsuario As ComboBoxEdit
        Private ReadOnly _cmbSimEmpresa As ComboBoxEdit
        Private ReadOnly _btnSimularScope As SimpleButton
        Private ReadOnly _memoSimulador As MemoEdit
        Private ReadOnly _gridAuditoria As GridControl
        Private ReadOnly _viewAuditoria As GridView

        Private _usuarios As List(Of UsuarioDto)
        Private _roles As List(Of RolDto)
        Private _asignaciones As List(Of AsignacionRolUsuarioDto)
        Private _permisos As List(Of PermisoDto)
        Private _deberes As List(Of DeberDto)
        Private _rolDeberes As List(Of RolDeberDto)
        Private _recursosUi As List(Of RecursoUiDto)
        Private _recursoPermisos As List(Of RecursoUiPermisoDto)
        Private _empresas As List(Of EmpresaDto)
        Private _sesiones As List(Of SesionUsuarioDto)
        Private _auditoriaEventos As List(Of AuditoriaEventoSeguridadDto)
        Private _auditoriaHelpdesk As List(Of AuditoriaReinicioMesaAyudaDto)
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
            _btnTabPermisos = New BarButtonItem() With {.Caption = "Permisos"}
            _btnTabMenu = New BarButtonItem() With {.Caption = "Menu UI"}
            _btnTabSesiones = New BarButtonItem() With {.Caption = "Sesiones"}
            _btnTabHelpdesk = New BarButtonItem() With {.Caption = "Helpdesk MFA"}
            _btnTabAuditoria = New BarButtonItem() With {.Caption = "Auditoria"}
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

            _splitPermisos = New SplitContainerControl()
            _txtBuscarPermisos = New TextEdit()
            _cmbPermisosRol = New ComboBoxEdit()
            _btnPermisoConceder = New SimpleButton() With {.Text = "Conceder permiso"}
            _btnPermisoRevocar = New SimpleButton() With {.Text = "Revocar permiso"}
            _gridPermisos = New GridControl()
            _viewPermisos = New GridView()

            _splitMenu = New SplitContainerControl()
            _txtBuscarMenu = New TextEdit()
            _cmbMenuRol = New ComboBoxEdit()
            _btnMenuConceder = New SimpleButton() With {.Text = "Conceder menu"}
            _btnMenuRevocar = New SimpleButton() With {.Text = "Revocar menu"}
            _gridMenu = New GridControl()
            _viewMenu = New GridView()

            _splitSesiones = New SplitContainerControl()
            _txtBuscarSesiones = New TextEdit()
            _gridSesiones = New GridControl()
            _viewSesiones = New GridView()

            _splitHelpdesk = New SplitContainerControl()
            _cmbHelpdeskUsuario = New ComboBoxEdit()
            _cmbHelpdeskAprobador = New ComboBoxEdit()
            _chkHelpdeskCritico = New CheckEdit() With {.Text = "Operacion critica (requiere 4 ojos)"}
            _memoHelpdeskMotivo = New MemoEdit()
            _btnHelpdeskRegistrar = New SimpleButton() With {.Text = "Registrar Reinicio MFA"}
            _txtBuscarHelpdesk = New TextEdit()
            _gridHelpdesk = New GridControl()
            _viewHelpdesk = New GridView()

            _splitAuditoria = New SplitContainerControl()
            _txtBuscarAuditoria = New TextEdit()
            _cmbSimUsuario = New ComboBoxEdit()
            _cmbSimEmpresa = New ComboBoxEdit()
            _btnSimularScope = New SimpleButton() With {.Text = "Simular alcance"}
            _memoSimulador = New MemoEdit()
            _gridAuditoria = New GridControl()
            _viewAuditoria = New GridView()

            _usuarios = New List(Of UsuarioDto)()
            _roles = New List(Of RolDto)()
            _asignaciones = New List(Of AsignacionRolUsuarioDto)()
            _permisos = New List(Of PermisoDto)()
            _deberes = New List(Of DeberDto)()
            _rolDeberes = New List(Of RolDeberDto)()
            _recursosUi = New List(Of RecursoUiDto)()
            _recursoPermisos = New List(Of RecursoUiPermisoDto)()
            _empresas = New List(Of EmpresaDto)()
            _sesiones = New List(Of SesionUsuarioDto)()
            _auditoriaEventos = New List(Of AuditoriaEventoSeguridadDto)()
            _auditoriaHelpdesk = New List(Of AuditoriaReinicioMesaAyudaDto)()
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
            SuspendLayout()
            ' 
            ' FrmIamAdminCenter
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(1256, 859)
            FormBorderStyle = FormBorderStyle.None
            Name = "FrmIamAdminCenter"
            Text = "Centro IAM"

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

            AssignIcon(_btnTabUsuarios, "BusinessObjects.BOUser")
            AssignIcon(_btnTabRoles, "BusinessObjects.BORole")
            AssignIcon(_btnTabAsignaciones, "BusinessObjects.BOPosition")
            AssignIcon(_btnTabPermisos, "BusinessObjects.BOPermission")
            AssignIcon(_btnTabMenu, "BusinessObjects.BOTask")
            AssignIcon(_btnTabSesiones, "Actions.NavigateBackward")
            AssignIcon(_btnTabHelpdesk, "Support.Support")
            AssignIcon(_btnTabAuditoria, "Reports.Report")
            AssignIcon(_btnNuevo, "Actions.Add")
            AssignIcon(_btnGuardar, "Save.Save")
            AssignIcon(_btnRefrescar, "Actions.Refresh")
            AssignIcon(_btnDesactivar, "Actions.Cancel")
            AssignIcon(_btnLimpiar, "Actions.Clear")
            _btnTabUsuarios.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabRoles.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabAsignaciones.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabPermisos.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabMenu.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabSesiones.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabHelpdesk.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnTabAuditoria.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnNuevo.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnGuardar.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnRefrescar.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnDesactivar.PaintStyle = BarItemPaintStyle.CaptionGlyph
            _btnLimpiar.PaintStyle = BarItemPaintStyle.CaptionGlyph

            _ribbon.Items.AddRange(New BarItem() {
                _btnTabUsuarios, _btnTabRoles, _btnTabAsignaciones, _btnTabPermisos, _btnTabMenu, _btnTabSesiones, _btnTabHelpdesk, _btnTabAuditoria,
                _btnNuevo, _btnGuardar, _btnRefrescar, _btnDesactivar, _btnLimpiar,
                _statusInfo, _statusModulo
            })

            Dim page As New RibbonPage("Inicio")
            page.Name = "rpInicio"
            Dim groupSecciones As New RibbonPageGroup("Secciones")
            groupSecciones.ItemLinks.Add(_btnTabUsuarios)
            groupSecciones.ItemLinks.Add(_btnTabRoles)
            groupSecciones.ItemLinks.Add(_btnTabAsignaciones)
            groupSecciones.ItemLinks.Add(_btnTabPermisos)
            groupSecciones.ItemLinks.Add(_btnTabMenu)
            groupSecciones.ItemLinks.Add(_btnTabSesiones)
            groupSecciones.ItemLinks.Add(_btnTabHelpdesk)
            groupSecciones.ItemLinks.Add(_btnTabAuditoria)

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
            Dim tabPermisos As New TabPage("Permisos")
            Dim tabMenu As New TabPage("Menu UI")
            Dim tabSesiones As New TabPage("Sesiones")
            Dim tabHelpdesk As New TabPage("Helpdesk MFA")
            Dim tabAuditoria As New TabPage("Auditoria")

            BuildUsuariosTab(tabUsuarios)
            BuildRolesTab(tabRoles)
            BuildAsignacionesTab(tabAsignaciones)
            BuildPermisosTab(tabPermisos)
            BuildMenuTab(tabMenu)
            BuildSesionesTab(tabSesiones)
            BuildHelpdeskTab(tabHelpdesk)
            BuildAuditoriaTab(tabAuditoria)

            _tabs.TabPages.Add(tabUsuarios)
            _tabs.TabPages.Add(tabRoles)
            _tabs.TabPages.Add(tabAsignaciones)
            _tabs.TabPages.Add(tabPermisos)
            _tabs.TabPages.Add(tabMenu)
            _tabs.TabPages.Add(tabSesiones)
            _tabs.TabPages.Add(tabHelpdesk)
            _tabs.TabPages.Add(tabAuditoria)

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

        Private Sub BuildPermisosTab(ByVal tab As TabPage)
            _splitPermisos.Dock = DockStyle.Fill
            _splitPermisos.Horizontal = False
            _splitPermisos.SplitterPosition = 84
            tab.Controls.Add(_splitPermisos)

            Dim panelTop As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar permiso:", .Location = New Point(8, 16)}
            _txtBuscarPermisos.Location = New Point(100, 12)
            _txtBuscarPermisos.Width = 360
            _txtBuscarPermisos.Properties.NullValuePrompt = "Codigo, modulo, accion o nombre..."
            _txtBuscarPermisos.Properties.NullValuePromptShowForEmptyValue = True
            Dim lblRol As New LabelControl() With {.Text = "Rol:", .Location = New Point(8, 52)}
            _cmbPermisosRol.Location = New Point(100, 48)
            _cmbPermisosRol.Width = 280
            _cmbPermisosRol.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _btnPermisoConceder.Location = New Point(396, 46)
            _btnPermisoConceder.Width = 130
            _btnPermisoRevocar.Location = New Point(534, 46)
            _btnPermisoRevocar.Width = 130
            panelTop.Controls.Add(lblSearch)
            panelTop.Controls.Add(_txtBuscarPermisos)
            panelTop.Controls.Add(lblRol)
            panelTop.Controls.Add(_cmbPermisosRol)
            panelTop.Controls.Add(_btnPermisoConceder)
            panelTop.Controls.Add(_btnPermisoRevocar)
            _splitPermisos.Panel1.Controls.Add(panelTop)

            _gridPermisos.Dock = DockStyle.Fill
            _gridPermisos.MainView = _viewPermisos
            ConfigureGridView(_viewPermisos)
            _splitPermisos.Panel2.Controls.Add(_gridPermisos)
            ConfigurePermisoColumns()
        End Sub

        Private Sub BuildMenuTab(ByVal tab As TabPage)
            _splitMenu.Dock = DockStyle.Fill
            _splitMenu.Horizontal = False
            _splitMenu.SplitterPosition = 84
            tab.Controls.Add(_splitMenu)

            Dim panelTop As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar recurso UI:", .Location = New Point(8, 16)}
            _txtBuscarMenu.Location = New Point(108, 12)
            _txtBuscarMenu.Width = 360
            _txtBuscarMenu.Properties.NullValuePrompt = "Codigo, nombre, ruta o componente..."
            _txtBuscarMenu.Properties.NullValuePromptShowForEmptyValue = True
            Dim lblRol As New LabelControl() With {.Text = "Rol:", .Location = New Point(8, 52)}
            _cmbMenuRol.Location = New Point(108, 48)
            _cmbMenuRol.Width = 280
            _cmbMenuRol.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _btnMenuConceder.Location = New Point(396, 46)
            _btnMenuConceder.Width = 120
            _btnMenuRevocar.Location = New Point(524, 46)
            _btnMenuRevocar.Width = 120
            panelTop.Controls.Add(lblSearch)
            panelTop.Controls.Add(_txtBuscarMenu)
            panelTop.Controls.Add(lblRol)
            panelTop.Controls.Add(_cmbMenuRol)
            panelTop.Controls.Add(_btnMenuConceder)
            panelTop.Controls.Add(_btnMenuRevocar)
            _splitMenu.Panel1.Controls.Add(panelTop)

            _gridMenu.Dock = DockStyle.Fill
            _gridMenu.MainView = _viewMenu
            ConfigureGridView(_viewMenu)
            _splitMenu.Panel2.Controls.Add(_gridMenu)
            ConfigureMenuColumns()
        End Sub

        Private Sub BuildSesionesTab(ByVal tab As TabPage)
            _splitSesiones.Dock = DockStyle.Fill
            _splitSesiones.Horizontal = False
            _splitSesiones.SplitterPosition = 52
            tab.Controls.Add(_splitSesiones)

            Dim panelTop As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar sesion:", .Location = New Point(8, 16)}
            _txtBuscarSesiones.Location = New Point(95, 12)
            _txtBuscarSesiones.Width = 360
            _txtBuscarSesiones.Properties.NullValuePrompt = "Usuario, origen, IP..."
            _txtBuscarSesiones.Properties.NullValuePromptShowForEmptyValue = True
            panelTop.Controls.Add(lblSearch)
            panelTop.Controls.Add(_txtBuscarSesiones)
            _splitSesiones.Panel1.Controls.Add(panelTop)

            _gridSesiones.Dock = DockStyle.Fill
            _gridSesiones.MainView = _viewSesiones
            ConfigureGridView(_viewSesiones)
            _splitSesiones.Panel2.Controls.Add(_gridSesiones)
            ConfigureSesionColumns()
        End Sub

        Private Sub BuildHelpdeskTab(ByVal tab As TabPage)
            _splitHelpdesk.Dock = DockStyle.Fill
            _splitHelpdesk.SplitterPosition = 420
            tab.Controls.Add(_splitHelpdesk)

            Dim panelLeft As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitHelpdesk.Panel1.Controls.Add(panelLeft)

            Dim layout As New LayoutControl() With {.Dock = DockStyle.Fill}
            panelLeft.Controls.Add(layout)

            Dim lblInfo As New LabelControl() With {.Text = "Mesa de ayuda MFA: registra reinicio controlado con motivo y auditoria."}
            lblInfo.Appearance.ForeColor = Color.DimGray
            _cmbHelpdeskUsuario.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _cmbHelpdeskAprobador.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _memoHelpdeskMotivo.Properties.NullValuePrompt = "Motivo obligatorio (incidente, verificacion identidad, ticket, evidencia)."
            _memoHelpdeskMotivo.Properties.NullValuePromptShowForEmptyValue = True

            layout.Controls.Add(lblInfo)
            layout.Controls.Add(_cmbHelpdeskUsuario)
            layout.Controls.Add(_cmbHelpdeskAprobador)
            layout.Controls.Add(_chkHelpdeskCritico)
            layout.Controls.Add(_memoHelpdeskMotivo)
            layout.Controls.Add(_btnHelpdeskRegistrar)

            Dim root As New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }
            layout.Root = root

            Dim iInfo = root.AddItem(String.Empty, lblInfo)
            iInfo.TextVisible = False
            iInfo.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 8, 10)
            root.AddItem("Usuario afectado*", _cmbHelpdeskUsuario)
            root.AddItem("Aprobador 4 ojos", _cmbHelpdeskAprobador)
            Dim iCritico = root.AddItem(String.Empty, _chkHelpdeskCritico)
            iCritico.TextVisible = False
            root.AddItem("Motivo*", _memoHelpdeskMotivo)
            Dim iBtn = root.AddItem(String.Empty, _btnHelpdeskRegistrar)
            iBtn.TextVisible = False
            iBtn.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 4, 8, 4)

            Dim panelRight As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            _splitHelpdesk.Panel2.Controls.Add(panelRight)

            Dim pnlSearch As New PanelControl() With {.Dock = DockStyle.Top, .Height = 42, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar historial:", .Location = New Point(8, 14)}
            _txtBuscarHelpdesk.Location = New Point(96, 11)
            _txtBuscarHelpdesk.Width = 320
            _txtBuscarHelpdesk.Properties.NullValuePrompt = "Usuario, admin o motivo..."
            _txtBuscarHelpdesk.Properties.NullValuePromptShowForEmptyValue = True
            pnlSearch.Controls.Add(lblSearch)
            pnlSearch.Controls.Add(_txtBuscarHelpdesk)

            _gridHelpdesk.Dock = DockStyle.Fill
            _gridHelpdesk.MainView = _viewHelpdesk
            ConfigureGridView(_viewHelpdesk)
            panelRight.Controls.Add(_gridHelpdesk)
            panelRight.Controls.Add(pnlSearch)
            ConfigureHelpdeskColumns()
        End Sub

        Private Sub BuildAuditoriaTab(ByVal tab As TabPage)
            _splitAuditoria.Dock = DockStyle.Fill
            _splitAuditoria.Horizontal = False
            _splitAuditoria.SplitterPosition = 52
            tab.Controls.Add(_splitAuditoria)

            Dim panelTop As New PanelControl() With {.Dock = DockStyle.Fill, .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder}
            Dim lblSearch As New LabelControl() With {.Text = "Buscar evento:", .Location = New Point(8, 16)}
            _txtBuscarAuditoria.Location = New Point(92, 12)
            _txtBuscarAuditoria.Width = 360
            _txtBuscarAuditoria.Properties.NullValuePrompt = "Detalle, IP, solicitud..."
            _txtBuscarAuditoria.Properties.NullValuePromptShowForEmptyValue = True
            Dim lblSimUsuario As New LabelControl() With {.Text = "Usuario simulado:", .Location = New Point(472, 16)}
            _cmbSimUsuario.Location = New Point(578, 12)
            _cmbSimUsuario.Width = 210
            _cmbSimUsuario.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            Dim lblSimEmpresa As New LabelControl() With {.Text = "Empresa:", .Location = New Point(800, 16)}
            _cmbSimEmpresa.Location = New Point(862, 12)
            _cmbSimEmpresa.Width = 180
            _cmbSimEmpresa.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            _btnSimularScope.Location = New Point(1052, 10)
            _btnSimularScope.Width = 124
            panelTop.Controls.Add(lblSearch)
            panelTop.Controls.Add(_txtBuscarAuditoria)
            panelTop.Controls.Add(lblSimUsuario)
            panelTop.Controls.Add(_cmbSimUsuario)
            panelTop.Controls.Add(lblSimEmpresa)
            panelTop.Controls.Add(_cmbSimEmpresa)
            panelTop.Controls.Add(_btnSimularScope)
            _splitAuditoria.Panel1.Controls.Add(panelTop)

            _gridAuditoria.Dock = DockStyle.Fill
            _gridAuditoria.MainView = _viewAuditoria
            ConfigureGridView(_viewAuditoria)
            _memoSimulador.Dock = DockStyle.Bottom
            _memoSimulador.Height = 170
            _memoSimulador.Properties.ReadOnly = True
            _memoSimulador.Properties.NullValuePrompt = "Ejecuta el simulador para ver permisos y menu efectivos por usuario/empresa."
            _memoSimulador.Properties.NullValuePromptShowForEmptyValue = True
            _splitAuditoria.Panel2.Controls.Add(_gridAuditoria)
            _splitAuditoria.Panel2.Controls.Add(_memoSimulador)
            ConfigureAuditoriaColumns()
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

        Private Sub ConfigurePermisoColumns()
            _viewPermisos.Columns.Clear()
            _viewPermisos.Columns.AddVisible(NameOf(PermisoMatrixRow.Asignado), "Asignado").Width = 90
            _viewPermisos.Columns.AddVisible(NameOf(PermisoMatrixRow.Codigo), "Codigo").Width = 180
            _viewPermisos.Columns.AddVisible(NameOf(PermisoMatrixRow.Modulo), "Modulo").Width = 140
            _viewPermisos.Columns.AddVisible(NameOf(PermisoMatrixRow.Accion), "Accion").Width = 130
            _viewPermisos.Columns.AddVisible(NameOf(PermisoMatrixRow.Nombre), "Nombre").Width = 260
            _viewPermisos.Columns.AddVisible(NameOf(PermisoMatrixRow.Sensible), "Sensible").Width = 90
            _viewPermisos.Columns.AddVisible(NameOf(PermisoMatrixRow.Activo), "Activo").Width = 80
        End Sub

        Private Sub ConfigureMenuColumns()
            _viewMenu.Columns.Clear()
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Asignado), "Asignado").Width = 90
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Codigo), "Codigo").Width = 170
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Nombre), "Nombre").Width = 220
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Ruta), "Ruta").Width = 220
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Componente), "Componente").Width = 180
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Permisos), "Permisos").Width = 280
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Visible), "Visible").Width = 90
            _viewMenu.Columns.AddVisible(NameOf(MenuMatrixRow.Activo), "Activo").Width = 80
        End Sub

        Private Sub ConfigureSesionColumns()
            _viewSesiones.Columns.Clear()
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.Usuario), "Usuario").Width = 220
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.Empresa), "Empresa").Width = 130
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.Origen), "Origen").Width = 120
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.MfaValidado), "MFA").Width = 70
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.Activo), "Activa").Width = 70
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.UltimaActividadUtc), "Ultima actividad").Width = 150
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.ExpiraAbsolutaUtc), "Expira").Width = 150
            _viewSesiones.Columns.AddVisible(NameOf(SesionGridRow.IpOrigen), "IP").Width = 130
        End Sub

        Private Sub ConfigureHelpdeskColumns()
            _viewHelpdesk.Columns.Clear()
            _viewHelpdesk.Columns.AddVisible(NameOf(HelpdeskGridRow.IdAuditoria), "Id").Width = 70
            _viewHelpdesk.Columns.AddVisible(NameOf(HelpdeskGridRow.UsuarioAfectado), "Usuario afectado").Width = 180
            _viewHelpdesk.Columns.AddVisible(NameOf(HelpdeskGridRow.Administrador), "Administrador").Width = 180
            _viewHelpdesk.Columns.AddVisible(NameOf(HelpdeskGridRow.Empresa), "Empresa").Width = 100
            _viewHelpdesk.Columns.AddVisible(NameOf(HelpdeskGridRow.Motivo), "Motivo").Width = 320
            _viewHelpdesk.Columns.AddVisible(NameOf(HelpdeskGridRow.FechaUtc), "Fecha UTC").Width = 150
        End Sub

        Private Sub ConfigureAuditoriaColumns()
            _viewAuditoria.Columns.Clear()
            _viewAuditoria.Columns.AddVisible(NameOf(AuditoriaEventoSeguridadDto.FechaUtc), "Fecha UTC").Width = 150
            _viewAuditoria.Columns.AddVisible(NameOf(AuditoriaEventoSeguridadDto.IdTipoEventoSeguridad), "Tipo").Width = 70
            _viewAuditoria.Columns.AddVisible(NameOf(AuditoriaEventoSeguridadDto.IdUsuario), "Usuario").Width = 90
            _viewAuditoria.Columns.AddVisible(NameOf(AuditoriaEventoSeguridadDto.IdEmpresa), "Empresa").Width = 90
            _viewAuditoria.Columns.AddVisible(NameOf(AuditoriaEventoSeguridadDto.IpOrigen), "IP").Width = 130
            _viewAuditoria.Columns.AddVisible(NameOf(AuditoriaEventoSeguridadDto.Detalle), "Detalle").Width = 400
        End Sub
        Private Sub WireEvents()
            AddHandler Shown, AddressOf OnFirstShown
            AddHandler _tabs.SelectedIndexChanged, AddressOf OnTabChanged
            AddHandler _btnTabUsuarios.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 0
            AddHandler _btnTabRoles.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 1
            AddHandler _btnTabAsignaciones.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 2
            AddHandler _btnTabPermisos.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 3
            AddHandler _btnTabMenu.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 4
            AddHandler _btnTabSesiones.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 5
            AddHandler _btnTabHelpdesk.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 6
            AddHandler _btnTabAuditoria.ItemClick, Sub(sender, e) _tabs.SelectedIndex = 7
            AddHandler _btnNuevo.ItemClick, AddressOf OnNuevoClick
            AddHandler _btnGuardar.ItemClick, AddressOf OnGuardarClickAsync
            AddHandler _btnRefrescar.ItemClick, AddressOf OnRefrescarClickAsync
            AddHandler _btnDesactivar.ItemClick, AddressOf OnDesactivarClickAsync
            AddHandler _btnLimpiar.ItemClick, AddressOf OnLimpiarClick

            AddHandler _txtBuscarUsuarios.EditValueChanged, Sub(sender, e) _viewUsuarios.FindFilterText = _txtBuscarUsuarios.Text
            AddHandler _txtBuscarRoles.EditValueChanged, Sub(sender, e) _viewRoles.FindFilterText = _txtBuscarRoles.Text
            AddHandler _txtBuscarAsignaciones.EditValueChanged, Sub(sender, e) _viewAsignaciones.FindFilterText = _txtBuscarAsignaciones.Text
            AddHandler _txtBuscarPermisos.EditValueChanged, Sub(sender, e) _viewPermisos.FindFilterText = _txtBuscarPermisos.Text
            AddHandler _txtBuscarMenu.EditValueChanged, Sub(sender, e) _viewMenu.FindFilterText = _txtBuscarMenu.Text
            AddHandler _txtBuscarSesiones.EditValueChanged, Sub(sender, e) _viewSesiones.FindFilterText = _txtBuscarSesiones.Text
            AddHandler _txtBuscarHelpdesk.EditValueChanged, Sub(sender, e) _viewHelpdesk.FindFilterText = _txtBuscarHelpdesk.Text
            AddHandler _txtBuscarAuditoria.EditValueChanged, Sub(sender, e) _viewAuditoria.FindFilterText = _txtBuscarAuditoria.Text

            AddHandler _viewUsuarios.FocusedRowChanged, AddressOf OnUsuarioFocusedRowChanged
            AddHandler _viewRoles.FocusedRowChanged, AddressOf OnRolFocusedRowChanged
            AddHandler _viewAsignaciones.FocusedRowChanged, AddressOf OnAsignacionFocusedRowChanged
            AddHandler _cmbPermisosRol.SelectedIndexChanged, AddressOf OnPermisosRolChanged
            AddHandler _cmbMenuRol.SelectedIndexChanged, AddressOf OnMenuRolChanged
            AddHandler _btnPermisoConceder.Click, AddressOf OnPermisoConcederClickAsync
            AddHandler _btnPermisoRevocar.Click, AddressOf OnPermisoRevocarClickAsync
            AddHandler _btnMenuConceder.Click, AddressOf OnMenuConcederClickAsync
            AddHandler _btnMenuRevocar.Click, AddressOf OnMenuRevocarClickAsync
            AddHandler _btnSimularScope.Click, AddressOf OnSimularScopeClick
            AddHandler _btnHelpdeskRegistrar.Click, AddressOf OnHelpdeskRegistrarClickAsync
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
                Case 2
                    Return "Asignaciones"
                Case 3
                    Return "Permisos"
                Case 4
                    Return "Menu UI"
                Case 5
                    Return "Sesiones"
                Case 6
                    Return "Helpdesk MFA"
                Case Else
                    Return "Auditoria"
            End Select
        End Function

        Private Async Function LoadInitialDataAsync() As Task
            SetBusy(True, "Cargando catalogos IAM...")
            Try
                Await LoadCatalogosAsync().ConfigureAwait(True)
                Await LoadUsuariosAsync().ConfigureAwait(True)
                Await LoadRolesAsync().ConfigureAwait(True)
                Await LoadAsignacionesAsync().ConfigureAwait(True)
                Await LoadPermisosAsync().ConfigureAwait(True)
                Await LoadDeberesAsync().ConfigureAwait(True)
                Await LoadRolDeberesAsync().ConfigureAwait(True)
                Await LoadRecursosUiAsync().ConfigureAwait(True)
                Await LoadRecursoPermisosAsync().ConfigureAwait(True)
                Await LoadEmpresasAsync().ConfigureAwait(True)
                Await LoadSesionesAsync().ConfigureAwait(True)
                Await LoadHelpdeskHistorialAsync().ConfigureAwait(True)
                Await LoadAuditoriaEventosAsync().ConfigureAwait(True)
                ResetUsuarioEditor()
                ResetRolEditor()
                ResetAsignacionEditor()
                ResetHelpdeskEditor()
                RefreshPermisosMatrix()
                RefreshMenuMatrix()
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

        Private Async Function LoadPermisosAsync() As Task
            SetBusy(True, "Cargando permisos...")
            Dim list = Await _apiClient.GetAsync(Of List(Of PermisoDto))("api/v1/seguridad/permiso").ConfigureAwait(True)
            _permisos = If(list, New List(Of PermisoDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderBy(Function(x) x.Modulo).
                ThenBy(Function(x) x.Codigo).
                ToList()

            _gridPermisos.DataSource = BuildPermisoRowsForSelectedRole()
            SetBusy(False)
        End Function

        Private Async Function LoadDeberesAsync() As Task
            SetBusy(True, "Cargando deberes...")
            Dim list = Await _apiClient.GetAsync(Of List(Of DeberDto))("api/v1/seguridad/deber").ConfigureAwait(True)
            _deberes = If(list, New List(Of DeberDto)()).
                Where(Function(x) x IsNot Nothing AndAlso x.IdDeber.HasValue).
                OrderBy(Function(x) x.Codigo).
                ToList()
            SetBusy(False)
        End Function

        Private Async Function LoadRolDeberesAsync() As Task
            SetBusy(True, "Cargando matriz rol-deber...")
            Dim list = Await _apiClient.GetAsync(Of List(Of RolDeberDto))("api/v1/seguridad/rol_deber").ConfigureAwait(True)
            _rolDeberes = If(list, New List(Of RolDeberDto)()).
                Where(Function(x) x IsNot Nothing AndAlso x.IdRol.HasValue AndAlso x.IdDeber.HasValue).
                ToList()
            SetBusy(False)
        End Function

        Private Async Function LoadRecursosUiAsync() As Task
            SetBusy(True, "Cargando menu UI...")
            Dim list = Await _apiClient.GetAsync(Of List(Of RecursoUiDto))("api/v1/seguridad/recurso_ui").ConfigureAwait(True)
            _recursosUi = If(list, New List(Of RecursoUiDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderBy(Function(x) x.OrdenVisual.GetValueOrDefault(Integer.MaxValue)).
                ThenBy(Function(x) x.Nombre).
                ToList()

            _gridMenu.DataSource = BuildMenuRowsForSelectedRole()
            SetBusy(False)
        End Function

        Private Async Function LoadRecursoPermisosAsync() As Task
            SetBusy(True, "Cargando relacion menu-permisos...")
            Dim list = Await _apiClient.GetAsync(Of List(Of RecursoUiPermisoDto))("api/v1/seguridad/recurso_ui_permiso").ConfigureAwait(True)
            _recursoPermisos = If(list, New List(Of RecursoUiPermisoDto)()).
                Where(Function(x) x IsNot Nothing AndAlso x.IdRecursoUi.HasValue AndAlso x.IdPermiso.HasValue).
                ToList()
            SetBusy(False)
        End Function

        Private Async Function LoadEmpresasAsync() As Task
            SetBusy(True, "Cargando empresas...")
            Dim list = Await _apiClient.GetAsync(Of List(Of EmpresaDto))("api/v1/organizacion/empresa").ConfigureAwait(True)
            _empresas = If(list, New List(Of EmpresaDto)()).
                Where(Function(x) x IsNot Nothing AndAlso x.IdEmpresa.HasValue AndAlso x.Activo.GetValueOrDefault(True)).
                OrderBy(Function(x) x.Nombre).
                ToList()
            BuildEmpresasCombo()
            SetBusy(False)
        End Function

        Private Async Function LoadSesionesAsync() As Task
            SetBusy(True, "Cargando sesiones...")
            Dim list = Await _apiClient.GetAsync(Of List(Of SesionUsuarioDto))("api/v1/seguridad/sesion_usuario").ConfigureAwait(True)
            _sesiones = If(list, New List(Of SesionUsuarioDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderByDescending(Function(x) x.UltimaActividadUtc.GetValueOrDefault(DateTime.MinValue)).
                ToList()

            _gridSesiones.DataSource = BuildSesionRows(_sesiones)
            SetBusy(False)
        End Function

        Private Async Function LoadHelpdeskHistorialAsync() As Task
            SetBusy(True, "Cargando auditoria helpdesk...")
            Dim list = Await _apiClient.GetAsync(Of List(Of AuditoriaReinicioMesaAyudaDto))("api/v1/observabilidad/auditoria_reinicio_mesa_ayuda").ConfigureAwait(True)
            _auditoriaHelpdesk = If(list, New List(Of AuditoriaReinicioMesaAyudaDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderByDescending(Function(x) x.FechaUtc.GetValueOrDefault(DateTime.MinValue)).
                ToList()

            _gridHelpdesk.DataSource = BuildHelpdeskRows(_auditoriaHelpdesk)
            BuildHelpdeskUsuariosCombo()
            SetBusy(False)
        End Function

        Private Async Function LoadAuditoriaEventosAsync() As Task
            SetBusy(True, "Cargando auditoria de eventos...")
            Dim list = Await _apiClient.GetAsync(Of List(Of AuditoriaEventoSeguridadDto))("api/v1/observabilidad/auditoria_evento_seguridad").ConfigureAwait(True)
            _auditoriaEventos = If(list, New List(Of AuditoriaEventoSeguridadDto)()).
                Where(Function(x) x IsNot Nothing).
                OrderByDescending(Function(x) x.FechaUtc.GetValueOrDefault(DateTime.MinValue)).
                ToList()

            _gridAuditoria.DataSource = _auditoriaEventos
            SetBusy(False)
        End Function

        Private Sub BuildUsuariosCombo()
            _cmbAsignUsuario.Properties.Items.Clear()
            _cmbHelpdeskUsuario.Properties.Items.Clear()
            _cmbHelpdeskAprobador.Properties.Items.Clear()
            _cmbSimUsuario.Properties.Items.Clear()
            For Each u In _usuarios.Where(Function(x) x.IdUsuario.HasValue)
                Dim nombre = If(String.IsNullOrWhiteSpace(u.NombreMostrar), u.LoginPrincipal, u.NombreMostrar)
                Dim combo = New ComboLongItem With {
                    .Id = u.IdUsuario.Value,
                    .Nombre = nombre
                }
                _cmbAsignUsuario.Properties.Items.Add(New ComboLongItem With {
                    .Id = u.IdUsuario.Value,
                    .Nombre = nombre
                })
                If u.Activo.GetValueOrDefault(True) Then
                    _cmbHelpdeskUsuario.Properties.Items.Add(combo)
                    _cmbHelpdeskAprobador.Properties.Items.Add(New ComboLongItem With {.Id = combo.Id, .Nombre = combo.Nombre})
                    _cmbSimUsuario.Properties.Items.Add(New ComboLongItem With {.Id = combo.Id, .Nombre = combo.Nombre})
                End If
            Next

            If _cmbSimUsuario.Properties.Items.Count > 0 AndAlso _cmbSimUsuario.SelectedItem Is Nothing Then
                _cmbSimUsuario.SelectedIndex = 0
            End If
        End Sub

        Private Sub BuildRolesCombo()
            _cmbAsignRol.Properties.Items.Clear()
            _cmbPermisosRol.Properties.Items.Clear()
            _cmbMenuRol.Properties.Items.Clear()
            For Each r In _roles.Where(Function(x) x.IdRol.HasValue)
                Dim combo = New ComboLongItem With {
                    .Id = r.IdRol.Value,
                    .Nombre = r.Nombre
                }
                _cmbAsignRol.Properties.Items.Add(New ComboLongItem With {.Id = combo.Id, .Nombre = combo.Nombre})
                _cmbPermisosRol.Properties.Items.Add(New ComboLongItem With {.Id = combo.Id, .Nombre = combo.Nombre})
                _cmbMenuRol.Properties.Items.Add(New ComboLongItem With {.Id = combo.Id, .Nombre = combo.Nombre})
            Next

            If _cmbPermisosRol.Properties.Items.Count > 0 AndAlso _cmbPermisosRol.SelectedItem Is Nothing Then
                _cmbPermisosRol.SelectedIndex = 0
            End If
            If _cmbMenuRol.Properties.Items.Count > 0 AndAlso _cmbMenuRol.SelectedItem Is Nothing Then
                _cmbMenuRol.SelectedIndex = 0
            End If
        End Sub

        Private Sub BuildEmpresasCombo()
            _cmbSimEmpresa.Properties.Items.Clear()
            For Each item In _empresas.Where(Function(x) x.IdEmpresa.HasValue)
                _cmbSimEmpresa.Properties.Items.Add(New ComboLongItem With {
                    .Id = item.IdEmpresa.Value,
                    .Nombre = $"{item.Codigo} - {item.Nombre}"
                })
            Next

            If _cmbSimEmpresa.Properties.Items.Count > 0 AndAlso _cmbSimEmpresa.SelectedItem Is Nothing Then
                _cmbSimEmpresa.SelectedIndex = 0
            End If
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

        Private Function BuildSesionRows(ByVal source As IEnumerable(Of SesionUsuarioDto)) As List(Of SesionGridRow)
            Dim userMap = _usuarios.Where(Function(u) u.IdUsuario.HasValue).ToDictionary(Function(u) u.IdUsuario.Value, Function(u) If(String.IsNullOrWhiteSpace(u.NombreMostrar), u.LoginPrincipal, u.NombreMostrar))
            Dim rows As New List(Of SesionGridRow)()

            For Each item In source
                If Not item.IdSesionUsuario.HasValue Then Continue For

                Dim usuarioTexto = If(item.IdUsuario.HasValue AndAlso userMap.ContainsKey(item.IdUsuario.Value), userMap(item.IdUsuario.Value), "Usuario #" & item.IdUsuario.GetValueOrDefault().ToString())
                rows.Add(New SesionGridRow With {
                    .IdSesionUsuario = item.IdSesionUsuario.Value,
                    .Usuario = usuarioTexto,
                    .Empresa = If(item.IdEmpresa.HasValue, "Empresa #" & item.IdEmpresa.Value.ToString(), "Sin empresa"),
                    .Origen = If(item.OrigenAutenticacion, String.Empty),
                    .MfaValidado = item.MfaValidado.GetValueOrDefault(False),
                    .Activo = item.Activo.GetValueOrDefault(False),
                    .UltimaActividadUtc = item.UltimaActividadUtc,
                    .ExpiraAbsolutaUtc = item.ExpiraAbsolutaUtc,
                    .IpOrigen = If(item.IpOrigen, String.Empty)
                })
            Next

            Return rows
        End Function

        Private Function BuildHelpdeskRows(ByVal source As IEnumerable(Of AuditoriaReinicioMesaAyudaDto)) As List(Of HelpdeskGridRow)
            Dim userMap = _usuarios.Where(Function(u) u.IdUsuario.HasValue).ToDictionary(Function(u) u.IdUsuario.Value, Function(u) If(String.IsNullOrWhiteSpace(u.NombreMostrar), u.LoginPrincipal, u.NombreMostrar))
            Dim rows As New List(Of HelpdeskGridRow)()

            For Each item In source
                If Not item.IdAuditoriaReinicioMesaAyuda.HasValue Then Continue For
                Dim afectado = If(item.IdUsuarioAfectado.HasValue AndAlso userMap.ContainsKey(item.IdUsuarioAfectado.Value), userMap(item.IdUsuarioAfectado.Value), "Usuario #" & item.IdUsuarioAfectado.GetValueOrDefault().ToString())
                Dim admin = If(item.IdUsuarioAdministrador.HasValue AndAlso userMap.ContainsKey(item.IdUsuarioAdministrador.Value), userMap(item.IdUsuarioAdministrador.Value), "Usuario #" & item.IdUsuarioAdministrador.GetValueOrDefault().ToString())
                rows.Add(New HelpdeskGridRow With {
                    .IdAuditoria = item.IdAuditoriaReinicioMesaAyuda.Value,
                    .UsuarioAfectado = afectado,
                    .Administrador = admin,
                    .Motivo = If(item.Motivo, String.Empty),
                    .FechaUtc = item.FechaUtc,
                    .Empresa = If(item.IdEmpresa.HasValue, "Empresa #" & item.IdEmpresa.Value.ToString(), "Sin empresa")
                })
            Next

            Return rows
        End Function

        Private Sub BuildHelpdeskUsuariosCombo()
            _cmbHelpdeskUsuario.Properties.Items.Clear()
            _cmbHelpdeskAprobador.Properties.Items.Clear()
            For Each u In _usuarios.Where(Function(x) x.IdUsuario.HasValue AndAlso x.Activo.GetValueOrDefault(True)).
                OrderBy(Function(x) If(String.IsNullOrWhiteSpace(x.NombreMostrar), x.LoginPrincipal, x.NombreMostrar))
                Dim nombre = If(String.IsNullOrWhiteSpace(u.NombreMostrar), u.LoginPrincipal, u.NombreMostrar)
                _cmbHelpdeskUsuario.Properties.Items.Add(New ComboLongItem With {
                    .Id = u.IdUsuario.Value,
                    .Nombre = nombre
                })
                _cmbHelpdeskAprobador.Properties.Items.Add(New ComboLongItem With {
                    .Id = u.IdUsuario.Value,
                    .Nombre = nombre
                })
            Next
        End Sub

        Private Function BuildPermisoRowsForSelectedRole() As List(Of PermisoMatrixRow)
            Dim roleId = GetSelectedLongId(_cmbPermisosRol)
            Dim deberByCode = _deberes.
                Where(Function(x) x.IdDeber.HasValue AndAlso Not String.IsNullOrWhiteSpace(x.Codigo)).
                GroupBy(Function(x) x.Codigo.Trim().ToUpperInvariant()).
                ToDictionary(Function(g) g.Key, Function(g) g.First())

            Dim assignedDeberes As New HashSet(Of Long)()
            If roleId.HasValue Then
                For Each rel In _rolDeberes.Where(Function(x) x.IdRol.HasValue AndAlso x.IdRol.Value = roleId.Value AndAlso x.Activo.GetValueOrDefault(True) AndAlso x.IdDeber.HasValue)
                    assignedDeberes.Add(rel.IdDeber.Value)
                Next
            End If

            Dim rows As New List(Of PermisoMatrixRow)()
            For Each p In _permisos.Where(Function(x) x.IdPermiso.HasValue)
                Dim deberId As Long? = Nothing
                Dim key = If(p.Codigo, String.Empty).Trim().ToUpperInvariant()
                If deberByCode.ContainsKey(key) Then
                    deberId = deberByCode(key).IdDeber
                End If

                rows.Add(New PermisoMatrixRow With {
                    .IdPermiso = p.IdPermiso.Value,
                    .IdDeber = deberId,
                    .Asignado = deberId.HasValue AndAlso assignedDeberes.Contains(deberId.Value),
                    .Codigo = If(p.Codigo, String.Empty),
                    .Modulo = If(p.Modulo, String.Empty),
                    .Accion = If(p.Accion, String.Empty),
                    .Nombre = If(p.Nombre, String.Empty),
                    .Sensible = p.EsSensible.GetValueOrDefault(False),
                    .Activo = p.Activo.GetValueOrDefault(True)
                })
            Next

            Return rows
        End Function

        Private Function BuildMenuRowsForSelectedRole() As List(Of MenuMatrixRow)
            Dim roleId = GetSelectedLongId(_cmbMenuRol)
            Dim assignedPermisos As New HashSet(Of Integer)()
            If roleId.HasValue Then
                Dim assignedDeberIds = _rolDeberes.
                    Where(Function(x) x.IdRol.HasValue AndAlso x.IdRol.Value = roleId.Value AndAlso x.Activo.GetValueOrDefault(True) AndAlso x.IdDeber.HasValue).
                    Select(Function(x) x.IdDeber.Value).
                    ToHashSet()
                Dim deberesAsignados = _deberes.Where(Function(d) d.IdDeber.HasValue AndAlso assignedDeberIds.Contains(d.IdDeber.Value)).ToList()
                For Each deber In deberesAsignados
                    Dim permiso = _permisos.FirstOrDefault(Function(p) p.IdPermiso.HasValue AndAlso
                        String.Equals(If(p.Codigo, String.Empty).Trim(), If(deber.Codigo, String.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
                    If permiso IsNot Nothing AndAlso permiso.IdPermiso.HasValue Then
                        assignedPermisos.Add(permiso.IdPermiso.Value)
                    End If
                Next
            End If

            Dim linksByRecurso = _recursoPermisos.
                Where(Function(x) x.IdRecursoUi.HasValue AndAlso x.IdPermiso.HasValue AndAlso x.Activo.GetValueOrDefault(True)).
                GroupBy(Function(x) x.IdRecursoUi.Value).
                ToDictionary(Function(g) g.Key, Function(g) g.Select(Function(x) x.IdPermiso.Value).Distinct().ToList())

            Dim rows As New List(Of MenuMatrixRow)()
            For Each item In _recursosUi.Where(Function(x) x.IdRecursoUi.HasValue)
                Dim permisosRecurso As List(Of Integer) = Nothing
                If Not linksByRecurso.TryGetValue(item.IdRecursoUi.Value, permisosRecurso) Then
                    permisosRecurso = New List(Of Integer)()
                End If

                Dim asignado = permisosRecurso.Count = 0 OrElse permisosRecurso.All(Function(pid) assignedPermisos.Contains(pid))
                Dim permisosTexto = String.Join(", ", permisosRecurso.
                    Select(Function(pid) _permisos.FirstOrDefault(Function(p) p.IdPermiso.HasValue AndAlso p.IdPermiso.Value = pid)).
                    Where(Function(p) p IsNot Nothing).
                    Select(Function(p) If(String.IsNullOrWhiteSpace(p.Codigo), p.Nombre, p.Codigo)))

                rows.Add(New MenuMatrixRow With {
                    .IdRecursoUi = item.IdRecursoUi.Value,
                    .Asignado = asignado,
                    .Codigo = If(item.Codigo, String.Empty),
                    .Nombre = If(item.Nombre, String.Empty),
                    .Ruta = If(item.Ruta, String.Empty),
                    .Componente = If(item.Componente, String.Empty),
                    .Visible = item.EsVisible.GetValueOrDefault(True),
                    .Activo = item.Activo.GetValueOrDefault(True),
                    .Permisos = permisosTexto
                })
            Next

            Return rows
        End Function

        Private Sub RefreshPermisosMatrix()
            _gridPermisos.DataSource = BuildPermisoRowsForSelectedRole()
        End Sub

        Private Sub RefreshMenuMatrix()
            _gridMenu.DataSource = BuildMenuRowsForSelectedRole()
        End Sub

        Private Sub OnPermisosRolChanged(ByVal sender As Object, ByVal e As EventArgs)
            RefreshPermisosMatrix()
        End Sub

        Private Sub OnMenuRolChanged(ByVal sender As Object, ByVal e As EventArgs)
            RefreshMenuMatrix()
        End Sub

        Private Async Sub OnPermisoConcederClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            Await ToggleSelectedPermisoAsync(True).ConfigureAwait(True)
        End Sub

        Private Async Sub OnPermisoRevocarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            Await ToggleSelectedPermisoAsync(False).ConfigureAwait(True)
        End Sub

        Private Async Function ToggleSelectedPermisoAsync(ByVal conceder As Boolean) As Task
            If _busy Then Return
            Dim roleId = GetSelectedLongId(_cmbPermisosRol)
            If Not roleId.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione un rol para editar la matriz.", "Matriz Roles-Permisos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim row = TryCast(_viewPermisos.GetFocusedRow(), PermisoMatrixRow)
            If row Is Nothing Then
                XtraMessageBox.Show(Me, "Seleccione un permiso en la grilla.", "Matriz Roles-Permisos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Await UpsertRolDeberFromPermisoAsync(roleId.Value, row.IdPermiso, conceder).ConfigureAwait(True)
            Await LoadRolDeberesAsync().ConfigureAwait(True)
            RefreshPermisosMatrix()
            RefreshMenuMatrix()
            _statusInfo.Caption = If(conceder, "Permiso concedido al rol.", "Permiso revocado del rol.")
        End Function

        Private Async Sub OnMenuConcederClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            Await ToggleSelectedMenuAsync(True).ConfigureAwait(True)
        End Sub

        Private Async Sub OnMenuRevocarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            Await ToggleSelectedMenuAsync(False).ConfigureAwait(True)
        End Sub

        Private Async Function ToggleSelectedMenuAsync(ByVal conceder As Boolean) As Task
            If _busy Then Return
            Dim roleId = GetSelectedLongId(_cmbMenuRol)
            If Not roleId.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione un rol para asignacion de menu.", "Menu por Rol", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim row = TryCast(_viewMenu.GetFocusedRow(), MenuMatrixRow)
            If row Is Nothing Then
                XtraMessageBox.Show(Me, "Seleccione un recurso de menu en la grilla.", "Menu por Rol", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim permisoIds = _recursoPermisos.
                Where(Function(x) x.IdRecursoUi.HasValue AndAlso x.IdRecursoUi.Value = row.IdRecursoUi AndAlso x.IdPermiso.HasValue AndAlso x.Activo.GetValueOrDefault(True)).
                Select(Function(x) x.IdPermiso.Value).
                Distinct().
                ToList()

            If permisoIds.Count = 0 Then
                _statusInfo.Caption = "El menu no tiene permisos vinculados; no requiere ajuste de rol."
                Return
            End If

            SetBusy(True, If(conceder, "Concediendo menu al rol...", "Revocando menu al rol..."))
            Try
                For Each permisoId In permisoIds
                    Await UpsertRolDeberFromPermisoAsync(roleId.Value, permisoId, conceder).ConfigureAwait(True)
                Next
                Await LoadRolDeberesAsync().ConfigureAwait(True)
                RefreshPermisosMatrix()
                RefreshMenuMatrix()
            Finally
                SetBusy(False)
            End Try

            _statusInfo.Caption = If(conceder, "Menu concedido mediante permisos del rol.", "Menu revocado mediante permisos del rol.")
        End Function

        Private Async Function UpsertRolDeberFromPermisoAsync(ByVal idRol As Long, ByVal idPermiso As Integer, ByVal activo As Boolean) As Task
            Dim permiso = _permisos.FirstOrDefault(Function(x) x.IdPermiso.HasValue AndAlso x.IdPermiso.Value = idPermiso)
            If permiso Is Nothing Then Return

            Dim deberId = Await EnsureDeberFromPermisoAsync(permiso).ConfigureAwait(True)
            If Not deberId.HasValue Then Return

            Dim dto As New RolDeberDto With {
                .IdRol = idRol,
                .IdDeber = deberId.Value,
                .Activo = activo,
                .CreadoUtc = DateTime.UtcNow,
                .ActualizadoUtc = DateTime.UtcNow
            }
            Await _apiClient.PostAsync(Of RolDeberDto, Dictionary(Of String, Object))("api/v1/seguridad/rol_deber/crear", dto).ConfigureAwait(True)
        End Function

        Private Async Function EnsureDeberFromPermisoAsync(ByVal permiso As PermisoDto) As Task(Of Long?)
            Dim codigo = If(permiso.Codigo, String.Empty).Trim()
            If String.IsNullOrWhiteSpace(codigo) Then Return Nothing

            Dim deber = _deberes.FirstOrDefault(Function(x) String.Equals(If(x.Codigo, String.Empty).Trim(), codigo, StringComparison.OrdinalIgnoreCase))
            If deber IsNot Nothing AndAlso deber.IdDeber.HasValue Then
                Return deber.IdDeber.Value
            End If

            If Not _sessionContext.IdTenant.HasValue Then
                Throw New InvalidOperationException("No hay id_tenant en sesion para crear deber asociado al permiso.")
            End If

            Dim dto As New DeberDto With {
                .IdTenant = _sessionContext.IdTenant,
                .Codigo = codigo,
                .Nombre = If(String.IsNullOrWhiteSpace(permiso.Nombre), codigo, permiso.Nombre),
                .Descripcion = $"Auto creado desde permiso {codigo}",
                .EsSistema = False,
                .Activo = True,
                .CreadoUtc = DateTime.UtcNow,
                .ActualizadoUtc = DateTime.UtcNow
            }
            Await _apiClient.PostAsync(Of DeberDto, Dictionary(Of String, Object))("api/v1/seguridad/deber/crear", dto).ConfigureAwait(True)
            Await LoadDeberesAsync().ConfigureAwait(True)
            deber = _deberes.FirstOrDefault(Function(x) String.Equals(If(x.Codigo, String.Empty).Trim(), codigo, StringComparison.OrdinalIgnoreCase))
            Return If(deber?.IdDeber, Nothing)
        End Function

        Private Async Sub OnSimularScopeClick(ByVal sender As Object, ByVal e As EventArgs)
            If _busy Then Return
            Dim idUsuario = GetSelectedLongId(_cmbSimUsuario)
            Dim idEmpresa = GetSelectedLongId(_cmbSimEmpresa)
            If Not idUsuario.HasValue OrElse Not idEmpresa.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione usuario y empresa para simular alcance.", "Simulador", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            SetBusy(True, "Simulando alcance de datos y permisos...")
            Try
                Dim now = DateTime.UtcNow
                Dim asignaciones = _asignaciones.Where(Function(x) x.IdUsuario.HasValue AndAlso x.IdUsuario.Value = idUsuario.Value AndAlso
                                                           x.IdEmpresa.HasValue AndAlso x.IdEmpresa.Value = idEmpresa.Value AndAlso
                                                           x.Activo.GetValueOrDefault(True) AndAlso
                                                           x.FechaInicioUtc.GetValueOrDefault(DateTime.MinValue) <= now AndAlso
                                                           (Not x.FechaFinUtc.HasValue OrElse x.FechaFinUtc.Value >= now)).ToList()

                Dim roleIds = asignaciones.Where(Function(x) x.IdRol.HasValue).Select(Function(x) x.IdRol.Value).Distinct().ToHashSet()
                Dim deberIds = _rolDeberes.Where(Function(x) x.IdRol.HasValue AndAlso roleIds.Contains(x.IdRol.Value) AndAlso x.Activo.GetValueOrDefault(True) AndAlso x.IdDeber.HasValue).
                    Select(Function(x) x.IdDeber.Value).Distinct().ToHashSet()
                Dim deberes = _deberes.Where(Function(x) x.IdDeber.HasValue AndAlso deberIds.Contains(x.IdDeber.Value)).ToList()

                Dim permisos = _permisos.Where(Function(p) Not String.IsNullOrWhiteSpace(p.Codigo) AndAlso
                    deberes.Any(Function(d) String.Equals(If(d.Codigo, String.Empty).Trim(), p.Codigo.Trim(), StringComparison.OrdinalIgnoreCase))).ToList()
                Dim permisoIds = permisos.Where(Function(p) p.IdPermiso.HasValue).Select(Function(p) p.IdPermiso.Value).ToHashSet()

                Dim menus = _recursosUi.Where(Function(r)
                                                  If Not r.IdRecursoUi.HasValue Then Return False
                                                  Dim links = _recursoPermisos.Where(Function(x) x.IdRecursoUi.HasValue AndAlso x.IdRecursoUi.Value = r.IdRecursoUi.Value AndAlso x.IdPermiso.HasValue AndAlso x.Activo.GetValueOrDefault(True)).
                                                      Select(Function(x) x.IdPermiso.Value).
                                                      Distinct().
                                                      ToList()
                                                  Return links.Count = 0 OrElse links.All(Function(pid) permisoIds.Contains(pid))
                                              End Function).ToList()

                Dim scopeEmpresa = Await _apiClient.GetAsync(Of List(Of UsuarioScopeEmpresaDto))("api/v1/seguridad/usuario_scope_empresa").ConfigureAwait(True)
                Dim scopeUnidad = Await _apiClient.GetAsync(Of List(Of UsuarioScopeUnidadDto))("api/v1/seguridad/usuario_scope_unidad").ConfigureAwait(True)
                Dim scopesEmp = If(scopeEmpresa, New List(Of UsuarioScopeEmpresaDto)()).
                    Where(Function(x) x.IdUsuario.HasValue AndAlso x.IdUsuario.Value = idUsuario.Value).
                    ToList()
                Dim scopesUni = If(scopeUnidad, New List(Of UsuarioScopeUnidadDto)()).
                    Where(Function(x) x.IdUsuario.HasValue AndAlso x.IdUsuario.Value = idUsuario.Value).
                    ToList()

                Dim rolesTexto = String.Join(", ", _roles.Where(Function(r) r.IdRol.HasValue AndAlso roleIds.Contains(r.IdRol.Value)).Select(Function(r) r.Nombre))
                Dim permisosTexto = String.Join(Environment.NewLine, permisos.Select(Function(p) "- " & If(p.Codigo, p.Nombre)))
                Dim menusTexto = String.Join(Environment.NewLine, menus.Select(Function(m) "- " & If(m.Nombre, m.Codigo)))

                _memoSimulador.Text = $"Usuario: {idUsuario.Value} | Empresa: {idEmpresa.Value}{Environment.NewLine}" &
                    $"Roles activos: {If(String.IsNullOrWhiteSpace(rolesTexto), "(ninguno)", rolesTexto)}{Environment.NewLine}" &
                    $"Scope empresa explicito: {scopesEmp.Count}{Environment.NewLine}" &
                    $"Scope unidad explicito: {scopesUni.Count}{Environment.NewLine}{Environment.NewLine}" &
                    $"Permisos efectivos ({permisos.Count}):{Environment.NewLine}{If(String.IsNullOrWhiteSpace(permisosTexto), "- ninguno", permisosTexto)}{Environment.NewLine}{Environment.NewLine}" &
                    $"Menu visible ({menus.Count}):{Environment.NewLine}{If(String.IsNullOrWhiteSpace(menusTexto), "- ninguno", menusTexto)}"
            Finally
                SetBusy(False)
            End Try
        End Sub
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

        Private Sub ResetHelpdeskEditor()
            _loadingEditors = True
            Try
                If _cmbHelpdeskUsuario.Properties.Items.Count > 0 Then
                    _cmbHelpdeskUsuario.SelectedIndex = 0
                Else
                    _cmbHelpdeskUsuario.SelectedItem = Nothing
                End If
                _cmbHelpdeskAprobador.SelectedItem = Nothing
                _chkHelpdeskCritico.Checked = False
                _memoHelpdeskMotivo.Text = String.Empty
            Finally
                _loadingEditors = False
            End Try
        End Sub

        Private Async Sub OnHelpdeskRegistrarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            If _busy Then Return
            Try
                Await RegistrarHelpdeskAsync().ConfigureAwait(True)
            Catch ex As ApiClientException
                Dim detalle = If(String.IsNullOrWhiteSpace(ex.ResponseBody), ex.Message, ex.ResponseBody)
                XtraMessageBox.Show(Me, detalle, "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Helpdesk MFA", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Async Function RegistrarHelpdeskAsync() As Task
            If Not _sessionContext.IdTenant.HasValue Then
                XtraMessageBox.Show(Me, "La sesion no tiene id_tenant para registrar override MFA.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not _sessionContext.IdUsuario.HasValue Then
                XtraMessageBox.Show(Me, "La sesion no tiene id_usuario administrador para auditar el override MFA.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim idUsuarioAfectado = GetSelectedLongId(_cmbHelpdeskUsuario)
            If Not idUsuarioAfectado.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione el usuario afectado.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _cmbHelpdeskUsuario.Focus()
                Return
            End If

            Dim motivo = _memoHelpdeskMotivo.Text.Trim()
            If String.IsNullOrWhiteSpace(motivo) OrElse motivo.Length < 12 Then
                XtraMessageBox.Show(Me, "Ingrese un motivo detallado de al menos 12 caracteres.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _memoHelpdeskMotivo.Focus()
                Return
            End If

            Dim idAprobador As Long? = GetSelectedLongId(_cmbHelpdeskAprobador)
            If _chkHelpdeskCritico.Checked Then
                If Not idAprobador.HasValue Then
                    XtraMessageBox.Show(Me, "Para operacion critica debe seleccionar aprobador secundario (4 ojos).", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    _cmbHelpdeskAprobador.Focus()
                    Return
                End If
                If idAprobador.Value = _sessionContext.IdUsuario.Value Then
                    XtraMessageBox.Show(Me, "El aprobador secundario debe ser distinto del administrador operador.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If

            Dim dto As New AuditoriaReinicioMesaAyudaDto With {
                .IdTenant = _sessionContext.IdTenant.Value,
                .IdEmpresa = _sessionContext.IdEmpresa,
                .IdUsuarioAfectado = idUsuarioAfectado.Value,
                .IdUsuarioAdministrador = _sessionContext.IdUsuario.Value,
                .Motivo = If(_chkHelpdeskCritico.Checked, $"{motivo} | 4ojos_aprobador={idAprobador}", motivo),
                .IpOrigen = "127.0.0.1",
                .AgenteUsuario = $"Secure.WinForms IAM/{Application.ProductVersion} {Environment.MachineName}",
                .FechaUtc = DateTime.UtcNow
            }

            SetBusy(True, "Registrando override MFA helpdesk...")
            Try
                Await _apiClient.PostAsync(Of AuditoriaReinicioMesaAyudaDto, Dictionary(Of String, Object))("api/v1/observabilidad/auditoria_reinicio_mesa_ayuda/crear", dto).ConfigureAwait(True)
                If _chkHelpdeskCritico.Checked Then
                    Dim evento As New AuditoriaEventoSeguridadDto With {
                        .FechaUtc = DateTime.UtcNow,
                        .IdTipoEventoSeguridad = 1,
                        .IdTenant = _sessionContext.IdTenant,
                        .IdEmpresa = _sessionContext.IdEmpresa,
                        .IdUsuario = _sessionContext.IdUsuario,
                        .Detalle = $"Helpdesk MFA override CRITICO aprobado por usuario {idAprobador}. Usuario afectado: {idUsuarioAfectado}.",
                        .IpOrigen = "127.0.0.1",
                        .AgenteUsuario = $"Secure.WinForms IAM/{Application.ProductVersion} {Environment.MachineName}",
                        .SolicitudId = Guid.NewGuid().ToString("N")
                    }
                    Await _apiClient.PostAsync(Of AuditoriaEventoSeguridadDto, Dictionary(Of String, Object))("api/v1/observabilidad/auditoria_evento_seguridad/crear", evento).ConfigureAwait(True)
                End If
                Await LoadHelpdeskHistorialAsync().ConfigureAwait(True)
                Await LoadAuditoriaEventosAsync().ConfigureAwait(True)
                ResetHelpdeskEditor()
                _statusInfo.Caption = "Override MFA helpdesk registrado y auditado correctamente."
            Finally
                SetBusy(False)
            End Try
        End Function

        Private Sub OnNuevoClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return

            Select Case _tabs.SelectedIndex
                Case 0
                    ResetUsuarioEditor()
                Case 1
                    ResetRolEditor()
                Case 2
                    ResetAsignacionEditor()
                Case 6
                    ResetHelpdeskEditor()
                Case Else
                    _statusInfo.Caption = "Esta seccion no tiene formulario de captura para limpiar."
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
                    Case 2
                        Await SaveAsignacionAsync().ConfigureAwait(True)
                    Case 6
                        Await RegistrarHelpdeskAsync().ConfigureAwait(True)
                    Case Else
                        _statusInfo.Caption = "Esta seccion es de consulta; no requiere guardar."
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
                    Case 2
                        Await LoadAsignacionesAsync().ConfigureAwait(True)
                    Case 3
                        Await LoadPermisosAsync().ConfigureAwait(True)
                        Await LoadDeberesAsync().ConfigureAwait(True)
                        Await LoadRolDeberesAsync().ConfigureAwait(True)
                        RefreshPermisosMatrix()
                    Case 4
                        Await LoadRecursosUiAsync().ConfigureAwait(True)
                        Await LoadRecursoPermisosAsync().ConfigureAwait(True)
                        Await LoadRolDeberesAsync().ConfigureAwait(True)
                        RefreshMenuMatrix()
                    Case 5
                        Await LoadSesionesAsync().ConfigureAwait(True)
                    Case 6
                        Await LoadHelpdeskHistorialAsync().ConfigureAwait(True)
                    Case Else
                        Await LoadAuditoriaEventosAsync().ConfigureAwait(True)
                        Await LoadEmpresasAsync().ConfigureAwait(True)
                End Select

                _statusInfo.Caption = "Datos actualizados."
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Refrescar", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Async Sub OnDesactivarClickAsync(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return
            If _tabs.SelectedIndex > 2 Then
                _statusInfo.Caption = "Desactivar aplica solo para Usuarios, Roles y Asignaciones."
                Return
            End If

            If XtraMessageBox.Show(Me, "Confirma desactivar el registro seleccionado?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If

            Try
                Select Case _tabs.SelectedIndex
                    Case 0
                        If Not _editingUsuarioId.HasValue Then Return
                        SetBusy(True, "Desactivando usuario...")
                        Await _apiClient.DeleteAsync("api/v1/seguridad/usuario/desactivar/" & _editingUsuarioId.Value.ToString()).ConfigureAwait(True)
                        Await LoadUsuariosAsync().ConfigureAwait(True)
                        Await LoadAsignacionesAsync().ConfigureAwait(True)
                        ResetUsuarioEditor()
                    Case 1
                        If Not _editingRolId.HasValue Then Return
                        SetBusy(True, "Desactivando rol...")
                        Await _apiClient.DeleteAsync("api/v1/seguridad/rol/desactivar/" & _editingRolId.Value.ToString()).ConfigureAwait(True)
                        Await LoadRolesAsync().ConfigureAwait(True)
                        Await LoadAsignacionesAsync().ConfigureAwait(True)
                        ResetRolEditor()
                    Case Else
                        If Not _editingAsignacionId.HasValue Then Return
                        SetBusy(True, "Desactivando asignacion...")
                        Await _apiClient.DeleteAsync("api/v1/seguridad/asignacion_rol_usuario/desactivar/" & _editingAsignacionId.Value.ToString()).ConfigureAwait(True)
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
                Await _apiClient.PutAsync("api/v1/seguridad/usuario/actualizar/" & _editingUsuarioId.Value.ToString(), dto).ConfigureAwait(True)
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
                Await _apiClient.PutAsync("api/v1/seguridad/rol/actualizar/" & _editingRolId.Value.ToString(), dto).ConfigureAwait(True)
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
                Await _apiClient.PutAsync("api/v1/seguridad/asignacion_rol_usuario/actualizar/" & _editingAsignacionId.Value.ToString(), dto).ConfigureAwait(True)
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

