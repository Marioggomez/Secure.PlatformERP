Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout
Imports Secure.Platform.Contracts.Dtos.Catalogo
Imports Secure.Platform.Contracts.Dtos.Observabilidad
Imports Secure.Platform.Contracts.Dtos.Seguridad

Namespace Forms.Seguridad
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class FrmIamAdminCenter
        Private _ribbon As RibbonControl
        Private _statusBar As RibbonStatusBar
        Private _statusInfo As BarStaticItem
        Private _statusModulo As BarStaticItem
        Private _btnTabUsuarios As BarButtonItem
        Private _btnTabRoles As BarButtonItem
        Private _btnTabAsignaciones As BarButtonItem
        Private _btnTabPermisos As BarButtonItem
        Private _btnTabMenu As BarButtonItem
        Private _btnTabSesiones As BarButtonItem
        Private _btnTabHelpdesk As BarButtonItem
        Private _btnTabAuditoria As BarButtonItem
        Private _btnNuevo As BarButtonItem
        Private _btnGuardar As BarButtonItem
        Private _btnRefrescar As BarButtonItem
        Private _btnDesactivar As BarButtonItem
        Private _btnLimpiar As BarButtonItem
        Private _btnResetClave As BarButtonItem

        Private _mainLayout As LayoutControl
        Private _tabs As TabControl

        Private _splitUsuarios As SplitContainerControl
        Private _txtBuscarUsuarios As TextEdit
        Private _gridUsuarios As GridControl
        Private _viewUsuarios As GridView
        Private _txtUsuarioCodigo As TextEdit
        Private _txtUsuarioLogin As TextEdit
        Private _txtUsuarioNombre As TextEdit
        Private _txtUsuarioApellido As TextEdit
        Private _txtUsuarioNombreMostrar As TextEdit
        Private _txtUsuarioCorreo As TextEdit
        Private _cmbUsuarioEstado As ComboBoxEdit
        Private _chkUsuarioActivo As CheckEdit
        Private _chkUsuarioMfa As CheckEdit
        Private _chkUsuarioCambioClave As CheckEdit

        Private _splitRoles As SplitContainerControl
        Private _txtBuscarRoles As TextEdit
        Private _gridRoles As GridControl
        Private _viewRoles As GridView
        Private _txtRolCodigo As TextEdit
        Private _txtRolNombre As TextEdit
        Private _memoRolDescripcion As MemoEdit
        Private _chkRolActivo As CheckEdit
        Private _chkRolSistema As CheckEdit

        Private _splitAsignaciones As SplitContainerControl
        Private _txtBuscarAsignaciones As TextEdit
        Private _gridAsignaciones As GridControl
        Private _viewAsignaciones As GridView
        Private _cmbAsignUsuario As ComboBoxEdit
        Private _cmbAsignRol As ComboBoxEdit
        Private _cmbAsignAlcance As ComboBoxEdit
        Private _dteAsignInicio As DateEdit
        Private _dteAsignFin As DateEdit
        Private _chkAsignActivo As CheckEdit

        Private _splitPermisos As SplitContainerControl
        Private _txtBuscarPermisos As TextEdit
        Private _cmbPermisosRol As ComboBoxEdit
        Private _btnPermisoConceder As SimpleButton
        Private _btnPermisoRevocar As SimpleButton
        Private _gridPermisos As GridControl
        Private _viewPermisos As GridView

        Private _splitMenu As SplitContainerControl
        Private _txtBuscarMenu As TextEdit
        Private _cmbMenuRol As ComboBoxEdit
        Private _btnMenuConceder As SimpleButton
        Private _btnMenuRevocar As SimpleButton
        Private _gridMenu As GridControl
        Private _viewMenu As GridView

        Private _splitSesiones As SplitContainerControl
        Private _txtBuscarSesiones As TextEdit
        Private _gridSesiones As GridControl
        Private _viewSesiones As GridView

        Private _splitHelpdesk As SplitContainerControl
        Private _cmbHelpdeskUsuario As ComboBoxEdit
        Private _cmbHelpdeskAprobador As ComboBoxEdit
        Private _chkHelpdeskCritico As CheckEdit
        Private _memoHelpdeskMotivo As MemoEdit
        Private _btnHelpdeskRegistrar As SimpleButton
        Private _txtBuscarHelpdesk As TextEdit
        Private _gridHelpdesk As GridControl
        Private _viewHelpdesk As GridView

        Private _splitAuditoria As SplitContainerControl
        Private _txtBuscarAuditoria As TextEdit
        Private _cmbSimUsuario As ComboBoxEdit
        Private _cmbSimEmpresa As ComboBoxEdit
        Private _btnSimularScope As SimpleButton
        Private _memoSimulador As MemoEdit
        Private _gridAuditoria As GridControl
        Private _viewAuditoria As GridView

        Private Sub InitializeComponent()
            _ribbon = New RibbonControl()
            _statusBar = New RibbonStatusBar()
            _statusModulo = New BarStaticItem()
            _statusInfo = New BarStaticItem()
            _btnTabUsuarios = New BarButtonItem()
            _btnTabRoles = New BarButtonItem()
            _btnTabAsignaciones = New BarButtonItem()
            _btnTabPermisos = New BarButtonItem()
            _btnTabMenu = New BarButtonItem()
            _btnTabSesiones = New BarButtonItem()
            _btnTabHelpdesk = New BarButtonItem()
            _btnTabAuditoria = New BarButtonItem()
            _btnNuevo = New BarButtonItem()
            _btnGuardar = New BarButtonItem()
            _btnRefrescar = New BarButtonItem()
            _btnDesactivar = New BarButtonItem()
            _btnLimpiar = New BarButtonItem()
            _btnResetClave = New BarButtonItem()
            pageInicio = New RibbonPage()
            groupSecciones = New RibbonPageGroup()
            groupAcciones = New RibbonPageGroup()
            _mainLayout = New LayoutControl()
            Root = New LayoutControlGroup()
            _tabs = New TabControl()
            tabUsuarios = New TabPage()
            _splitUsuarios = New SplitContainerControl()
            _gridUsuarios = New GridControl()
            _viewUsuarios = New GridView()
            _txtUsuarioLogin = New TextEdit()
            tabRoles = New TabPage()
            _splitRoles = New SplitContainerControl()
            _gridRoles = New GridControl()
            _viewRoles = New GridView()
            _txtRolNombre = New TextEdit()
            tabAsignaciones = New TabPage()
            _splitAsignaciones = New SplitContainerControl()
            _gridAsignaciones = New GridControl()
            _viewAsignaciones = New GridView()
            _cmbAsignUsuario = New ComboBoxEdit()
            tabPermisos = New TabPage()
            _splitPermisos = New SplitContainerControl()
            _cmbPermisosRol = New ComboBoxEdit()
            _gridPermisos = New GridControl()
            _viewPermisos = New GridView()
            tabMenu = New TabPage()
            _splitMenu = New SplitContainerControl()
            _cmbMenuRol = New ComboBoxEdit()
            _gridMenu = New GridControl()
            _viewMenu = New GridView()
            tabSesiones = New TabPage()
            _splitSesiones = New SplitContainerControl()
            _txtBuscarSesiones = New TextEdit()
            _gridSesiones = New GridControl()
            _viewSesiones = New GridView()
            tabHelpdesk = New TabPage()
            _splitHelpdesk = New SplitContainerControl()
            _memoHelpdeskMotivo = New MemoEdit()
            _gridHelpdesk = New GridControl()
            _viewHelpdesk = New GridView()
            tabAuditoria = New TabPage()
            _splitAuditoria = New SplitContainerControl()
            _memoSimulador = New MemoEdit()
            _gridAuditoria = New GridControl()
            _viewAuditoria = New GridView()
            _txtBuscarUsuarios = New TextEdit()
            _txtUsuarioCodigo = New TextEdit()
            _txtUsuarioNombre = New TextEdit()
            _txtUsuarioApellido = New TextEdit()
            _txtUsuarioNombreMostrar = New TextEdit()
            _txtUsuarioCorreo = New TextEdit()
            _cmbUsuarioEstado = New ComboBoxEdit()
            _chkUsuarioActivo = New CheckEdit()
            _chkUsuarioMfa = New CheckEdit()
            _chkUsuarioCambioClave = New CheckEdit()
            _txtBuscarRoles = New TextEdit()
            _txtRolCodigo = New TextEdit()
            _memoRolDescripcion = New MemoEdit()
            _chkRolActivo = New CheckEdit()
            _chkRolSistema = New CheckEdit()
            _txtBuscarAsignaciones = New TextEdit()
            _cmbAsignRol = New ComboBoxEdit()
            _cmbAsignAlcance = New ComboBoxEdit()
            _dteAsignInicio = New DateEdit()
            _dteAsignFin = New DateEdit()
            _chkAsignActivo = New CheckEdit()
            _txtBuscarPermisos = New TextEdit()
            _btnPermisoConceder = New SimpleButton()
            _btnPermisoRevocar = New SimpleButton()
            _txtBuscarMenu = New TextEdit()
            _btnMenuConceder = New SimpleButton()
            _btnMenuRevocar = New SimpleButton()
            _cmbHelpdeskUsuario = New ComboBoxEdit()
            _cmbHelpdeskAprobador = New ComboBoxEdit()
            _chkHelpdeskCritico = New CheckEdit()
            _btnHelpdeskRegistrar = New SimpleButton()
            _txtBuscarHelpdesk = New TextEdit()
            _txtBuscarAuditoria = New TextEdit()
            _cmbSimUsuario = New ComboBoxEdit()
            _cmbSimEmpresa = New ComboBoxEdit()
            _btnSimularScope = New SimpleButton()
            LayoutControlItem1 = New LayoutControlItem()
            CType(_ribbon, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_mainLayout, System.ComponentModel.ISupportInitialize).BeginInit()
            _mainLayout.SuspendLayout()
            CType(Root, System.ComponentModel.ISupportInitialize).BeginInit()
            _tabs.SuspendLayout()
            tabUsuarios.SuspendLayout()
            CType(_splitUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitUsuarios.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitUsuarios.Panel1.SuspendLayout()
            CType(_splitUsuarios.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitUsuarios.Panel2.SuspendLayout()
            _splitUsuarios.SuspendLayout()
            CType(_gridUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuarioLogin.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            tabRoles.SuspendLayout()
            CType(_splitRoles, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitRoles.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitRoles.Panel1.SuspendLayout()
            CType(_splitRoles.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitRoles.Panel2.SuspendLayout()
            _splitRoles.SuspendLayout()
            CType(_gridRoles, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewRoles, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtRolNombre.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            tabAsignaciones.SuspendLayout()
            CType(_splitAsignaciones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitAsignaciones.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitAsignaciones.Panel1.SuspendLayout()
            CType(_splitAsignaciones.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitAsignaciones.Panel2.SuspendLayout()
            _splitAsignaciones.SuspendLayout()
            CType(_gridAsignaciones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewAsignaciones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbAsignUsuario.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            tabPermisos.SuspendLayout()
            CType(_splitPermisos, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitPermisos.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitPermisos.Panel1.SuspendLayout()
            CType(_splitPermisos.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitPermisos.Panel2.SuspendLayout()
            _splitPermisos.SuspendLayout()
            CType(_cmbPermisosRol.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_gridPermisos, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewPermisos, System.ComponentModel.ISupportInitialize).BeginInit()
            tabMenu.SuspendLayout()
            CType(_splitMenu, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitMenu.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitMenu.Panel1.SuspendLayout()
            CType(_splitMenu.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitMenu.Panel2.SuspendLayout()
            _splitMenu.SuspendLayout()
            CType(_cmbMenuRol.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_gridMenu, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewMenu, System.ComponentModel.ISupportInitialize).BeginInit()
            tabSesiones.SuspendLayout()
            CType(_splitSesiones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitSesiones.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitSesiones.Panel1.SuspendLayout()
            CType(_splitSesiones.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitSesiones.Panel2.SuspendLayout()
            _splitSesiones.SuspendLayout()
            CType(_txtBuscarSesiones.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_gridSesiones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewSesiones, System.ComponentModel.ISupportInitialize).BeginInit()
            tabHelpdesk.SuspendLayout()
            CType(_splitHelpdesk, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitHelpdesk.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitHelpdesk.Panel1.SuspendLayout()
            CType(_splitHelpdesk.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitHelpdesk.Panel2.SuspendLayout()
            _splitHelpdesk.SuspendLayout()
            CType(_memoHelpdeskMotivo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_gridHelpdesk, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewHelpdesk, System.ComponentModel.ISupportInitialize).BeginInit()
            tabAuditoria.SuspendLayout()
            CType(_splitAuditoria, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_splitAuditoria.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitAuditoria.Panel1.SuspendLayout()
            CType(_splitAuditoria.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
            _splitAuditoria.Panel2.SuspendLayout()
            _splitAuditoria.SuspendLayout()
            CType(_memoSimulador.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_gridAuditoria, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_viewAuditoria, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtBuscarUsuarios.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuarioCodigo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuarioNombre.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuarioApellido.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuarioNombreMostrar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuarioCorreo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbUsuarioEstado.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkUsuarioActivo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkUsuarioMfa.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkUsuarioCambioClave.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtBuscarRoles.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtRolCodigo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_memoRolDescripcion.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkRolActivo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkRolSistema.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtBuscarAsignaciones.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbAsignRol.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbAsignAlcance.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_dteAsignInicio.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_dteAsignInicio.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_dteAsignFin.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_dteAsignFin.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkAsignActivo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtBuscarPermisos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtBuscarMenu.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbHelpdeskUsuario.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbHelpdeskAprobador.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkHelpdeskCritico.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtBuscarHelpdesk.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtBuscarAuditoria.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbSimUsuario.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(_cmbSimEmpresa.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' _ribbon
            ' 
            _ribbon.ExpandCollapseItem.Id = 0
            _ribbon.Items.AddRange(New BarItem() {_ribbon.ExpandCollapseItem, _statusModulo, _statusInfo, _btnTabUsuarios, _btnTabRoles, _btnTabAsignaciones, _btnTabPermisos, _btnTabMenu, _btnTabSesiones, _btnTabHelpdesk, _btnTabAuditoria, _btnNuevo, _btnGuardar, _btnRefrescar, _btnDesactivar, _btnLimpiar, _btnResetClave})
            _ribbon.Location = New Point(0, 0)
            _ribbon.MaxItemId = 17
            _ribbon.MdiMergeStyle = RibbonMdiMergeStyle.Never
            _ribbon.Name = "_ribbon"
            _ribbon.Pages.AddRange(New RibbonPage() {pageInicio})
            _ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            _ribbon.Size = New Size(855, 158)
            _ribbon.StatusBar = _statusBar
            _ribbon.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden
            ' 
            ' _statusBar
            ' 
            _statusBar.ItemLinks.Add(_statusModulo)
            _statusBar.ItemLinks.Add(_statusInfo)
            _statusBar.Location = New Point(0, 474)
            _statusBar.Name = "_statusBar"
            _statusBar.Ribbon = _ribbon
            _statusBar.Size = New Size(855, 27)
            ' 
            ' _statusModulo
            ' 
            _statusModulo.Caption = "Seccion: Usuarios"
            _statusModulo.Id = 15
            _statusModulo.Name = "_statusModulo"
            ' 
            ' _statusInfo
            ' 
            _statusInfo.Caption = "Centro IAM listo."
            _statusInfo.Id = 16
            _statusInfo.Name = "_statusInfo"
            ' 
            ' _btnTabUsuarios
            ' 
            _btnTabUsuarios.Caption = "Usuarios"
            _btnTabUsuarios.Id = 1
            _btnTabUsuarios.Name = "_btnTabUsuarios"
            ' 
            ' _btnTabRoles
            ' 
            _btnTabRoles.Caption = "Roles"
            _btnTabRoles.Id = 2
            _btnTabRoles.Name = "_btnTabRoles"
            ' 
            ' _btnTabAsignaciones
            ' 
            _btnTabAsignaciones.Caption = "Asignaciones"
            _btnTabAsignaciones.Id = 3
            _btnTabAsignaciones.Name = "_btnTabAsignaciones"
            ' 
            ' _btnTabPermisos
            ' 
            _btnTabPermisos.Caption = "Permisos"
            _btnTabPermisos.Id = 4
            _btnTabPermisos.Name = "_btnTabPermisos"
            ' 
            ' _btnTabMenu
            ' 
            _btnTabMenu.Caption = "Menu UI"
            _btnTabMenu.Id = 5
            _btnTabMenu.Name = "_btnTabMenu"
            ' 
            ' _btnTabSesiones
            ' 
            _btnTabSesiones.Caption = "Sesiones"
            _btnTabSesiones.Id = 6
            _btnTabSesiones.Name = "_btnTabSesiones"
            ' 
            ' _btnTabHelpdesk
            ' 
            _btnTabHelpdesk.Caption = "Helpdesk MFA"
            _btnTabHelpdesk.Id = 7
            _btnTabHelpdesk.Name = "_btnTabHelpdesk"
            ' 
            ' _btnTabAuditoria
            ' 
            _btnTabAuditoria.Caption = "Auditoria"
            _btnTabAuditoria.Id = 8
            _btnTabAuditoria.Name = "_btnTabAuditoria"
            ' 
            ' _btnNuevo
            ' 
            _btnNuevo.Caption = "Nuevo"
            _btnNuevo.Id = 9
            _btnNuevo.Name = "_btnNuevo"
            ' 
            ' _btnGuardar
            ' 
            _btnGuardar.Caption = "Guardar"
            _btnGuardar.Id = 10
            _btnGuardar.Name = "_btnGuardar"
            ' 
            ' _btnRefrescar
            ' 
            _btnRefrescar.Caption = "Refrescar"
            _btnRefrescar.Id = 11
            _btnRefrescar.Name = "_btnRefrescar"
            ' 
            ' _btnDesactivar
            ' 
            _btnDesactivar.Caption = "Desactivar"
            _btnDesactivar.Id = 12
            _btnDesactivar.Name = "_btnDesactivar"
            ' 
            ' _btnLimpiar
            ' 
            _btnLimpiar.Caption = "Limpiar editor"
            _btnLimpiar.Id = 13
            _btnLimpiar.Name = "_btnLimpiar"
            ' 
            ' _btnResetClave
            ' 
            _btnResetClave.Caption = "Reset clave"
            _btnResetClave.Id = 14
            _btnResetClave.Name = "_btnResetClave"
            ' 
            ' pageInicio
            ' 
            pageInicio.Groups.AddRange(New RibbonPageGroup() {groupSecciones, groupAcciones})
            pageInicio.Name = "pageInicio"
            pageInicio.Text = "Inicio"
            ' 
            ' groupSecciones
            ' 
            groupSecciones.ItemLinks.Add(_btnTabUsuarios)
            groupSecciones.ItemLinks.Add(_btnTabRoles)
            groupSecciones.ItemLinks.Add(_btnTabAsignaciones)
            groupSecciones.ItemLinks.Add(_btnTabPermisos)
            groupSecciones.ItemLinks.Add(_btnTabMenu)
            groupSecciones.ItemLinks.Add(_btnTabSesiones)
            groupSecciones.ItemLinks.Add(_btnTabHelpdesk)
            groupSecciones.ItemLinks.Add(_btnTabAuditoria)
            groupSecciones.Name = "groupSecciones"
            groupSecciones.Text = "Secciones"
            ' 
            ' groupAcciones
            ' 
            groupAcciones.ItemLinks.Add(_btnNuevo)
            groupAcciones.ItemLinks.Add(_btnGuardar)
            groupAcciones.ItemLinks.Add(_btnRefrescar)
            groupAcciones.ItemLinks.Add(_btnDesactivar)
            groupAcciones.ItemLinks.Add(_btnLimpiar)
            groupAcciones.ItemLinks.Add(_btnResetClave)
            groupAcciones.Name = "groupAcciones"
            groupAcciones.Text = "Acciones"
            ' 
            ' _mainLayout
            ' 
            _mainLayout.Controls.Add(_tabs)
            _mainLayout.Dock = DockStyle.Fill
            _mainLayout.Location = New Point(0, 158)
            _mainLayout.Name = "_mainLayout"
            _mainLayout.Root = Root
            _mainLayout.Size = New Size(855, 316)
            _mainLayout.TabIndex = 0
            ' 
            ' Root
            ' 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            Root.GroupBordersVisible = False
            Root.Items.AddRange(New BaseLayoutItem() {LayoutControlItem1})
            Root.Name = "Root"
            Root.Size = New Size(855, 316)
            ' 
            ' _tabs
            ' 
            _tabs.Controls.Add(tabUsuarios)
            _tabs.Controls.Add(tabRoles)
            _tabs.Controls.Add(tabAsignaciones)
            _tabs.Controls.Add(tabPermisos)
            _tabs.Controls.Add(tabMenu)
            _tabs.Controls.Add(tabSesiones)
            _tabs.Controls.Add(tabHelpdesk)
            _tabs.Controls.Add(tabAuditoria)
            _tabs.Location = New Point(120, 12)
            _tabs.Name = "_tabs"
            _tabs.Padding = New Point(16, 6)
            _tabs.SelectedIndex = 0
            _tabs.Size = New Size(723, 292)
            _tabs.TabIndex = 0
            ' 
            ' tabUsuarios
            ' 
            tabUsuarios.Controls.Add(_splitUsuarios)
            tabUsuarios.Location = New Point(4, 28)
            tabUsuarios.Name = "tabUsuarios"
            tabUsuarios.Size = New Size(715, 260)
            tabUsuarios.TabIndex = 0
            tabUsuarios.Text = "Usuarios"
            ' 
            ' _splitUsuarios
            ' 
            _splitUsuarios.Dock = DockStyle.Fill
            _splitUsuarios.Location = New Point(0, 0)
            _splitUsuarios.Name = "_splitUsuarios"
            ' 
            ' _splitUsuarios.Panel1
            ' 
            _splitUsuarios.Panel1.Controls.Add(_gridUsuarios)
            ' 
            ' _splitUsuarios.Panel2
            ' 
            _splitUsuarios.Panel2.Controls.Add(_txtUsuarioLogin)
            _splitUsuarios.Size = New Size(715, 260)
            _splitUsuarios.TabIndex = 0
            ' 
            ' _gridUsuarios
            ' 
            _gridUsuarios.Location = New Point(0, 0)
            _gridUsuarios.MainView = _viewUsuarios
            _gridUsuarios.Name = "_gridUsuarios"
            _gridUsuarios.Size = New Size(400, 200)
            _gridUsuarios.TabIndex = 0
            _gridUsuarios.ViewCollection.AddRange(New Views.Base.BaseView() {_viewUsuarios})
            ' 
            ' _viewUsuarios
            ' 
            _viewUsuarios.GridControl = _gridUsuarios
            _viewUsuarios.Name = "_viewUsuarios"
            ' 
            ' _txtUsuarioLogin
            ' 
            _txtUsuarioLogin.Location = New Point(0, 0)
            _txtUsuarioLogin.Name = "_txtUsuarioLogin"
            _txtUsuarioLogin.Size = New Size(100, 20)
            _txtUsuarioLogin.TabIndex = 0
            ' 
            ' tabRoles
            ' 
            tabRoles.Controls.Add(_splitRoles)
            tabRoles.Location = New Point(4, 28)
            tabRoles.Name = "tabRoles"
            tabRoles.Size = New Size(715, 260)
            tabRoles.TabIndex = 1
            tabRoles.Text = "Roles"
            ' 
            ' _splitRoles
            ' 
            _splitRoles.Dock = DockStyle.Fill
            _splitRoles.Location = New Point(0, 0)
            _splitRoles.Name = "_splitRoles"
            ' 
            ' _splitRoles.Panel1
            ' 
            _splitRoles.Panel1.Controls.Add(_gridRoles)
            ' 
            ' _splitRoles.Panel2
            ' 
            _splitRoles.Panel2.Controls.Add(_txtRolNombre)
            _splitRoles.Size = New Size(715, 260)
            _splitRoles.TabIndex = 0
            ' 
            ' _gridRoles
            ' 
            _gridRoles.Location = New Point(0, 0)
            _gridRoles.MainView = _viewRoles
            _gridRoles.Name = "_gridRoles"
            _gridRoles.Size = New Size(400, 200)
            _gridRoles.TabIndex = 0
            _gridRoles.ViewCollection.AddRange(New Views.Base.BaseView() {_viewRoles})
            ' 
            ' _viewRoles
            ' 
            _viewRoles.GridControl = _gridRoles
            _viewRoles.Name = "_viewRoles"
            ' 
            ' _txtRolNombre
            ' 
            _txtRolNombre.Location = New Point(0, 0)
            _txtRolNombre.Name = "_txtRolNombre"
            _txtRolNombre.Size = New Size(100, 20)
            _txtRolNombre.TabIndex = 0
            ' 
            ' tabAsignaciones
            ' 
            tabAsignaciones.Controls.Add(_splitAsignaciones)
            tabAsignaciones.Location = New Point(4, 30)
            tabAsignaciones.Name = "tabAsignaciones"
            tabAsignaciones.Size = New Size(192, 66)
            tabAsignaciones.TabIndex = 2
            tabAsignaciones.Text = "Asignaciones"
            ' 
            ' _splitAsignaciones
            ' 
            _splitAsignaciones.Dock = DockStyle.Fill
            _splitAsignaciones.Location = New Point(0, 0)
            _splitAsignaciones.Name = "_splitAsignaciones"
            ' 
            ' _splitAsignaciones.Panel1
            ' 
            _splitAsignaciones.Panel1.Controls.Add(_gridAsignaciones)
            ' 
            ' _splitAsignaciones.Panel2
            ' 
            _splitAsignaciones.Panel2.Controls.Add(_cmbAsignUsuario)
            _splitAsignaciones.Size = New Size(192, 66)
            _splitAsignaciones.TabIndex = 0
            ' 
            ' _gridAsignaciones
            ' 
            _gridAsignaciones.Location = New Point(0, 0)
            _gridAsignaciones.MainView = _viewAsignaciones
            _gridAsignaciones.Name = "_gridAsignaciones"
            _gridAsignaciones.Size = New Size(400, 200)
            _gridAsignaciones.TabIndex = 0
            _gridAsignaciones.ViewCollection.AddRange(New Views.Base.BaseView() {_viewAsignaciones})
            ' 
            ' _viewAsignaciones
            ' 
            _viewAsignaciones.GridControl = _gridAsignaciones
            _viewAsignaciones.Name = "_viewAsignaciones"
            ' 
            ' _cmbAsignUsuario
            ' 
            _cmbAsignUsuario.Location = New Point(0, 0)
            _cmbAsignUsuario.Name = "_cmbAsignUsuario"
            _cmbAsignUsuario.Size = New Size(100, 20)
            _cmbAsignUsuario.TabIndex = 0
            ' 
            ' tabPermisos
            ' 
            tabPermisos.Controls.Add(_splitPermisos)
            tabPermisos.Location = New Point(4, 30)
            tabPermisos.Name = "tabPermisos"
            tabPermisos.Size = New Size(192, 66)
            tabPermisos.TabIndex = 3
            tabPermisos.Text = "Permisos"
            ' 
            ' _splitPermisos
            ' 
            _splitPermisos.Dock = DockStyle.Fill
            _splitPermisos.Location = New Point(0, 0)
            _splitPermisos.Name = "_splitPermisos"
            ' 
            ' _splitPermisos.Panel1
            ' 
            _splitPermisos.Panel1.Controls.Add(_cmbPermisosRol)
            ' 
            ' _splitPermisos.Panel2
            ' 
            _splitPermisos.Panel2.Controls.Add(_gridPermisos)
            _splitPermisos.Size = New Size(192, 66)
            _splitPermisos.TabIndex = 0
            ' 
            ' _cmbPermisosRol
            ' 
            _cmbPermisosRol.Location = New Point(0, 0)
            _cmbPermisosRol.Name = "_cmbPermisosRol"
            _cmbPermisosRol.Size = New Size(100, 20)
            _cmbPermisosRol.TabIndex = 0
            ' 
            ' _gridPermisos
            ' 
            _gridPermisos.Location = New Point(0, 0)
            _gridPermisos.MainView = _viewPermisos
            _gridPermisos.Name = "_gridPermisos"
            _gridPermisos.Size = New Size(400, 200)
            _gridPermisos.TabIndex = 0
            _gridPermisos.ViewCollection.AddRange(New Views.Base.BaseView() {_viewPermisos})
            ' 
            ' _viewPermisos
            ' 
            _viewPermisos.GridControl = _gridPermisos
            _viewPermisos.Name = "_viewPermisos"
            ' 
            ' tabMenu
            ' 
            tabMenu.Controls.Add(_splitMenu)
            tabMenu.Location = New Point(4, 30)
            tabMenu.Name = "tabMenu"
            tabMenu.Size = New Size(192, 66)
            tabMenu.TabIndex = 4
            tabMenu.Text = "Menu UI"
            ' 
            ' _splitMenu
            ' 
            _splitMenu.Dock = DockStyle.Fill
            _splitMenu.Location = New Point(0, 0)
            _splitMenu.Name = "_splitMenu"
            ' 
            ' _splitMenu.Panel1
            ' 
            _splitMenu.Panel1.Controls.Add(_cmbMenuRol)
            ' 
            ' _splitMenu.Panel2
            ' 
            _splitMenu.Panel2.Controls.Add(_gridMenu)
            _splitMenu.Size = New Size(192, 66)
            _splitMenu.TabIndex = 0
            ' 
            ' _cmbMenuRol
            ' 
            _cmbMenuRol.Location = New Point(0, 0)
            _cmbMenuRol.Name = "_cmbMenuRol"
            _cmbMenuRol.Size = New Size(100, 20)
            _cmbMenuRol.TabIndex = 0
            ' 
            ' _gridMenu
            ' 
            _gridMenu.Location = New Point(0, 0)
            _gridMenu.MainView = _viewMenu
            _gridMenu.Name = "_gridMenu"
            _gridMenu.Size = New Size(400, 200)
            _gridMenu.TabIndex = 0
            _gridMenu.ViewCollection.AddRange(New Views.Base.BaseView() {_viewMenu})
            ' 
            ' _viewMenu
            ' 
            _viewMenu.GridControl = _gridMenu
            _viewMenu.Name = "_viewMenu"
            ' 
            ' tabSesiones
            ' 
            tabSesiones.Controls.Add(_splitSesiones)
            tabSesiones.Location = New Point(4, 30)
            tabSesiones.Name = "tabSesiones"
            tabSesiones.Size = New Size(192, 66)
            tabSesiones.TabIndex = 5
            tabSesiones.Text = "Sesiones"
            ' 
            ' _splitSesiones
            ' 
            _splitSesiones.Dock = DockStyle.Fill
            _splitSesiones.Location = New Point(0, 0)
            _splitSesiones.Name = "_splitSesiones"
            ' 
            ' _splitSesiones.Panel1
            ' 
            _splitSesiones.Panel1.Controls.Add(_txtBuscarSesiones)
            ' 
            ' _splitSesiones.Panel2
            ' 
            _splitSesiones.Panel2.Controls.Add(_gridSesiones)
            _splitSesiones.Size = New Size(192, 66)
            _splitSesiones.TabIndex = 0
            ' 
            ' _txtBuscarSesiones
            ' 
            _txtBuscarSesiones.Location = New Point(0, 0)
            _txtBuscarSesiones.Name = "_txtBuscarSesiones"
            _txtBuscarSesiones.Size = New Size(100, 20)
            _txtBuscarSesiones.TabIndex = 0
            ' 
            ' _gridSesiones
            ' 
            _gridSesiones.Location = New Point(0, 0)
            _gridSesiones.MainView = _viewSesiones
            _gridSesiones.Name = "_gridSesiones"
            _gridSesiones.Size = New Size(400, 200)
            _gridSesiones.TabIndex = 0
            _gridSesiones.ViewCollection.AddRange(New Views.Base.BaseView() {_viewSesiones})
            ' 
            ' _viewSesiones
            ' 
            _viewSesiones.GridControl = _gridSesiones
            _viewSesiones.Name = "_viewSesiones"
            ' 
            ' tabHelpdesk
            ' 
            tabHelpdesk.Controls.Add(_splitHelpdesk)
            tabHelpdesk.Location = New Point(4, 30)
            tabHelpdesk.Name = "tabHelpdesk"
            tabHelpdesk.Size = New Size(192, 66)
            tabHelpdesk.TabIndex = 6
            tabHelpdesk.Text = "Helpdesk MFA"
            ' 
            ' _splitHelpdesk
            ' 
            _splitHelpdesk.Dock = DockStyle.Fill
            _splitHelpdesk.Location = New Point(0, 0)
            _splitHelpdesk.Name = "_splitHelpdesk"
            ' 
            ' _splitHelpdesk.Panel1
            ' 
            _splitHelpdesk.Panel1.Controls.Add(_memoHelpdeskMotivo)
            ' 
            ' _splitHelpdesk.Panel2
            ' 
            _splitHelpdesk.Panel2.Controls.Add(_gridHelpdesk)
            _splitHelpdesk.Size = New Size(192, 66)
            _splitHelpdesk.TabIndex = 0
            ' 
            ' _memoHelpdeskMotivo
            ' 
            _memoHelpdeskMotivo.Location = New Point(0, 0)
            _memoHelpdeskMotivo.Name = "_memoHelpdeskMotivo"
            _memoHelpdeskMotivo.Size = New Size(100, 96)
            _memoHelpdeskMotivo.TabIndex = 0
            ' 
            ' _gridHelpdesk
            ' 
            _gridHelpdesk.Location = New Point(0, 0)
            _gridHelpdesk.MainView = _viewHelpdesk
            _gridHelpdesk.Name = "_gridHelpdesk"
            _gridHelpdesk.Size = New Size(400, 200)
            _gridHelpdesk.TabIndex = 0
            _gridHelpdesk.ViewCollection.AddRange(New Views.Base.BaseView() {_viewHelpdesk})
            ' 
            ' _viewHelpdesk
            ' 
            _viewHelpdesk.GridControl = _gridHelpdesk
            _viewHelpdesk.Name = "_viewHelpdesk"
            ' 
            ' tabAuditoria
            ' 
            tabAuditoria.Controls.Add(_splitAuditoria)
            tabAuditoria.Location = New Point(4, 30)
            tabAuditoria.Name = "tabAuditoria"
            tabAuditoria.Size = New Size(192, 66)
            tabAuditoria.TabIndex = 7
            tabAuditoria.Text = "Auditoria"
            ' 
            ' _splitAuditoria
            ' 
            _splitAuditoria.Dock = DockStyle.Fill
            _splitAuditoria.Location = New Point(0, 0)
            _splitAuditoria.Name = "_splitAuditoria"
            ' 
            ' _splitAuditoria.Panel1
            ' 
            _splitAuditoria.Panel1.Controls.Add(_memoSimulador)
            ' 
            ' _splitAuditoria.Panel2
            ' 
            _splitAuditoria.Panel2.Controls.Add(_gridAuditoria)
            _splitAuditoria.Size = New Size(192, 66)
            _splitAuditoria.TabIndex = 0
            ' 
            ' _memoSimulador
            ' 
            _memoSimulador.Location = New Point(0, 0)
            _memoSimulador.Name = "_memoSimulador"
            _memoSimulador.Size = New Size(100, 96)
            _memoSimulador.TabIndex = 0
            ' 
            ' _gridAuditoria
            ' 
            _gridAuditoria.Location = New Point(0, 0)
            _gridAuditoria.MainView = _viewAuditoria
            _gridAuditoria.Name = "_gridAuditoria"
            _gridAuditoria.Size = New Size(400, 200)
            _gridAuditoria.TabIndex = 0
            _gridAuditoria.ViewCollection.AddRange(New Views.Base.BaseView() {_viewAuditoria})
            ' 
            ' _viewAuditoria
            ' 
            _viewAuditoria.GridControl = _gridAuditoria
            _viewAuditoria.Name = "_viewAuditoria"
            ' 
            ' _txtBuscarUsuarios
            ' 
            _txtBuscarUsuarios.Location = New Point(0, 0)
            _txtBuscarUsuarios.Name = "_txtBuscarUsuarios"
            _txtBuscarUsuarios.Size = New Size(100, 20)
            _txtBuscarUsuarios.TabIndex = 0
            ' 
            ' _txtUsuarioCodigo
            ' 
            _txtUsuarioCodigo.Location = New Point(0, 0)
            _txtUsuarioCodigo.Name = "_txtUsuarioCodigo"
            _txtUsuarioCodigo.Size = New Size(100, 20)
            _txtUsuarioCodigo.TabIndex = 0
            ' 
            ' _txtUsuarioNombre
            ' 
            _txtUsuarioNombre.Location = New Point(0, 0)
            _txtUsuarioNombre.Name = "_txtUsuarioNombre"
            _txtUsuarioNombre.Size = New Size(100, 20)
            _txtUsuarioNombre.TabIndex = 0
            ' 
            ' _txtUsuarioApellido
            ' 
            _txtUsuarioApellido.Location = New Point(0, 0)
            _txtUsuarioApellido.Name = "_txtUsuarioApellido"
            _txtUsuarioApellido.Size = New Size(100, 20)
            _txtUsuarioApellido.TabIndex = 0
            ' 
            ' _txtUsuarioNombreMostrar
            ' 
            _txtUsuarioNombreMostrar.Location = New Point(0, 0)
            _txtUsuarioNombreMostrar.Name = "_txtUsuarioNombreMostrar"
            _txtUsuarioNombreMostrar.Size = New Size(100, 20)
            _txtUsuarioNombreMostrar.TabIndex = 0
            ' 
            ' _txtUsuarioCorreo
            ' 
            _txtUsuarioCorreo.Location = New Point(0, 0)
            _txtUsuarioCorreo.Name = "_txtUsuarioCorreo"
            _txtUsuarioCorreo.Size = New Size(100, 20)
            _txtUsuarioCorreo.TabIndex = 0
            ' 
            ' _cmbUsuarioEstado
            ' 
            _cmbUsuarioEstado.Location = New Point(0, 0)
            _cmbUsuarioEstado.Name = "_cmbUsuarioEstado"
            _cmbUsuarioEstado.Size = New Size(100, 20)
            _cmbUsuarioEstado.TabIndex = 0
            ' 
            ' _chkUsuarioActivo
            ' 
            _chkUsuarioActivo.Location = New Point(0, 0)
            _chkUsuarioActivo.Name = "_chkUsuarioActivo"
            _chkUsuarioActivo.Size = New Size(75, 20)
            _chkUsuarioActivo.TabIndex = 0
            ' 
            ' _chkUsuarioMfa
            ' 
            _chkUsuarioMfa.Location = New Point(0, 0)
            _chkUsuarioMfa.Name = "_chkUsuarioMfa"
            _chkUsuarioMfa.Size = New Size(75, 20)
            _chkUsuarioMfa.TabIndex = 0
            ' 
            ' _chkUsuarioCambioClave
            ' 
            _chkUsuarioCambioClave.Location = New Point(0, 0)
            _chkUsuarioCambioClave.Name = "_chkUsuarioCambioClave"
            _chkUsuarioCambioClave.Size = New Size(75, 20)
            _chkUsuarioCambioClave.TabIndex = 0
            ' 
            ' _txtBuscarRoles
            ' 
            _txtBuscarRoles.Location = New Point(0, 0)
            _txtBuscarRoles.Name = "_txtBuscarRoles"
            _txtBuscarRoles.Size = New Size(100, 20)
            _txtBuscarRoles.TabIndex = 0
            ' 
            ' _txtRolCodigo
            ' 
            _txtRolCodigo.Location = New Point(0, 0)
            _txtRolCodigo.Name = "_txtRolCodigo"
            _txtRolCodigo.Size = New Size(100, 20)
            _txtRolCodigo.TabIndex = 0
            ' 
            ' _memoRolDescripcion
            ' 
            _memoRolDescripcion.Location = New Point(0, 0)
            _memoRolDescripcion.Name = "_memoRolDescripcion"
            _memoRolDescripcion.Size = New Size(100, 96)
            _memoRolDescripcion.TabIndex = 0
            ' 
            ' _chkRolActivo
            ' 
            _chkRolActivo.Location = New Point(0, 0)
            _chkRolActivo.Name = "_chkRolActivo"
            _chkRolActivo.Size = New Size(75, 20)
            _chkRolActivo.TabIndex = 0
            ' 
            ' _chkRolSistema
            ' 
            _chkRolSistema.Location = New Point(0, 0)
            _chkRolSistema.Name = "_chkRolSistema"
            _chkRolSistema.Size = New Size(75, 20)
            _chkRolSistema.TabIndex = 0
            ' 
            ' _txtBuscarAsignaciones
            ' 
            _txtBuscarAsignaciones.Location = New Point(0, 0)
            _txtBuscarAsignaciones.Name = "_txtBuscarAsignaciones"
            _txtBuscarAsignaciones.Size = New Size(100, 20)
            _txtBuscarAsignaciones.TabIndex = 0
            ' 
            ' _cmbAsignRol
            ' 
            _cmbAsignRol.Location = New Point(0, 0)
            _cmbAsignRol.Name = "_cmbAsignRol"
            _cmbAsignRol.Size = New Size(100, 20)
            _cmbAsignRol.TabIndex = 0
            ' 
            ' _cmbAsignAlcance
            ' 
            _cmbAsignAlcance.Location = New Point(0, 0)
            _cmbAsignAlcance.Name = "_cmbAsignAlcance"
            _cmbAsignAlcance.Size = New Size(100, 20)
            _cmbAsignAlcance.TabIndex = 0
            ' 
            ' _dteAsignInicio
            ' 
            _dteAsignInicio.EditValue = New Date(2026, 3, 14, 0, 0, 0, 0)
            _dteAsignInicio.Location = New Point(0, 0)
            _dteAsignInicio.Name = "_dteAsignInicio"
            _dteAsignInicio.Properties.CalendarTimeProperties.Buttons.AddRange(New Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            _dteAsignInicio.Size = New Size(100, 20)
            _dteAsignInicio.TabIndex = 0
            ' 
            ' _dteAsignFin
            ' 
            _dteAsignFin.EditValue = New Date(2026, 3, 14, 0, 0, 0, 0)
            _dteAsignFin.Location = New Point(0, 0)
            _dteAsignFin.Name = "_dteAsignFin"
            _dteAsignFin.Properties.CalendarTimeProperties.Buttons.AddRange(New Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            _dteAsignFin.Size = New Size(100, 20)
            _dteAsignFin.TabIndex = 0
            ' 
            ' _chkAsignActivo
            ' 
            _chkAsignActivo.Location = New Point(0, 0)
            _chkAsignActivo.Name = "_chkAsignActivo"
            _chkAsignActivo.Size = New Size(75, 20)
            _chkAsignActivo.TabIndex = 0
            ' 
            ' _txtBuscarPermisos
            ' 
            _txtBuscarPermisos.Location = New Point(0, 0)
            _txtBuscarPermisos.Name = "_txtBuscarPermisos"
            _txtBuscarPermisos.Size = New Size(100, 20)
            _txtBuscarPermisos.TabIndex = 0
            ' 
            ' _btnPermisoConceder
            ' 
            _btnPermisoConceder.Location = New Point(0, 0)
            _btnPermisoConceder.Name = "_btnPermisoConceder"
            _btnPermisoConceder.Size = New Size(75, 23)
            _btnPermisoConceder.TabIndex = 0
            ' 
            ' _btnPermisoRevocar
            ' 
            _btnPermisoRevocar.Location = New Point(0, 0)
            _btnPermisoRevocar.Name = "_btnPermisoRevocar"
            _btnPermisoRevocar.Size = New Size(75, 23)
            _btnPermisoRevocar.TabIndex = 0
            ' 
            ' _txtBuscarMenu
            ' 
            _txtBuscarMenu.Location = New Point(0, 0)
            _txtBuscarMenu.Name = "_txtBuscarMenu"
            _txtBuscarMenu.Size = New Size(100, 20)
            _txtBuscarMenu.TabIndex = 0
            ' 
            ' _btnMenuConceder
            ' 
            _btnMenuConceder.Location = New Point(0, 0)
            _btnMenuConceder.Name = "_btnMenuConceder"
            _btnMenuConceder.Size = New Size(75, 23)
            _btnMenuConceder.TabIndex = 0
            ' 
            ' _btnMenuRevocar
            ' 
            _btnMenuRevocar.Location = New Point(0, 0)
            _btnMenuRevocar.Name = "_btnMenuRevocar"
            _btnMenuRevocar.Size = New Size(75, 23)
            _btnMenuRevocar.TabIndex = 0
            ' 
            ' _cmbHelpdeskUsuario
            ' 
            _cmbHelpdeskUsuario.Location = New Point(0, 0)
            _cmbHelpdeskUsuario.Name = "_cmbHelpdeskUsuario"
            _cmbHelpdeskUsuario.Size = New Size(100, 20)
            _cmbHelpdeskUsuario.TabIndex = 0
            ' 
            ' _cmbHelpdeskAprobador
            ' 
            _cmbHelpdeskAprobador.Location = New Point(0, 0)
            _cmbHelpdeskAprobador.Name = "_cmbHelpdeskAprobador"
            _cmbHelpdeskAprobador.Size = New Size(100, 20)
            _cmbHelpdeskAprobador.TabIndex = 0
            ' 
            ' _chkHelpdeskCritico
            ' 
            _chkHelpdeskCritico.Location = New Point(0, 0)
            _chkHelpdeskCritico.Name = "_chkHelpdeskCritico"
            _chkHelpdeskCritico.Size = New Size(75, 20)
            _chkHelpdeskCritico.TabIndex = 0
            ' 
            ' _btnHelpdeskRegistrar
            ' 
            _btnHelpdeskRegistrar.Location = New Point(0, 0)
            _btnHelpdeskRegistrar.Name = "_btnHelpdeskRegistrar"
            _btnHelpdeskRegistrar.Size = New Size(75, 23)
            _btnHelpdeskRegistrar.TabIndex = 0
            ' 
            ' _txtBuscarHelpdesk
            ' 
            _txtBuscarHelpdesk.Location = New Point(0, 0)
            _txtBuscarHelpdesk.Name = "_txtBuscarHelpdesk"
            _txtBuscarHelpdesk.Size = New Size(100, 20)
            _txtBuscarHelpdesk.TabIndex = 0
            ' 
            ' _txtBuscarAuditoria
            ' 
            _txtBuscarAuditoria.Location = New Point(0, 0)
            _txtBuscarAuditoria.Name = "_txtBuscarAuditoria"
            _txtBuscarAuditoria.Size = New Size(100, 20)
            _txtBuscarAuditoria.TabIndex = 0
            ' 
            ' _cmbSimUsuario
            ' 
            _cmbSimUsuario.Location = New Point(0, 0)
            _cmbSimUsuario.Name = "_cmbSimUsuario"
            _cmbSimUsuario.Size = New Size(100, 20)
            _cmbSimUsuario.TabIndex = 0
            ' 
            ' _cmbSimEmpresa
            ' 
            _cmbSimEmpresa.Location = New Point(0, 0)
            _cmbSimEmpresa.Name = "_cmbSimEmpresa"
            _cmbSimEmpresa.Size = New Size(100, 20)
            _cmbSimEmpresa.TabIndex = 0
            ' 
            ' _btnSimularScope
            ' 
            _btnSimularScope.Location = New Point(0, 0)
            _btnSimularScope.Name = "_btnSimularScope"
            _btnSimularScope.Size = New Size(75, 23)
            _btnSimularScope.TabIndex = 0
            ' 
            ' LayoutControlItem1
            ' 
            LayoutControlItem1.Control = _tabs
            LayoutControlItem1.Location = New Point(0, 0)
            LayoutControlItem1.Name = "LayoutControlItem1"
            LayoutControlItem1.Size = New Size(835, 296)
            LayoutControlItem1.TextSize = New Size(96, 13)
            ' 
            ' FrmIamAdminCenter
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(855, 501)
            Controls.Add(_mainLayout)
            Controls.Add(_ribbon)
            Controls.Add(_statusBar)
            FormBorderStyle = FormBorderStyle.None
            Name = "FrmIamAdminCenter"
            Ribbon = _ribbon
            StatusBar = _statusBar
            Text = "Centro IAM"
            CType(_ribbon, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_mainLayout, System.ComponentModel.ISupportInitialize).EndInit()
            _mainLayout.ResumeLayout(False)
            CType(Root, System.ComponentModel.ISupportInitialize).EndInit()
            _tabs.ResumeLayout(False)
            tabUsuarios.ResumeLayout(False)
            CType(_splitUsuarios.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitUsuarios.Panel1.ResumeLayout(False)
            CType(_splitUsuarios.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitUsuarios.Panel2.ResumeLayout(False)
            CType(_splitUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
            _splitUsuarios.ResumeLayout(False)
            CType(_gridUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtUsuarioLogin.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            tabRoles.ResumeLayout(False)
            CType(_splitRoles.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitRoles.Panel1.ResumeLayout(False)
            CType(_splitRoles.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitRoles.Panel2.ResumeLayout(False)
            CType(_splitRoles, System.ComponentModel.ISupportInitialize).EndInit()
            _splitRoles.ResumeLayout(False)
            CType(_gridRoles, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewRoles, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtRolNombre.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            tabAsignaciones.ResumeLayout(False)
            CType(_splitAsignaciones.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitAsignaciones.Panel1.ResumeLayout(False)
            CType(_splitAsignaciones.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitAsignaciones.Panel2.ResumeLayout(False)
            CType(_splitAsignaciones, System.ComponentModel.ISupportInitialize).EndInit()
            _splitAsignaciones.ResumeLayout(False)
            CType(_gridAsignaciones, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewAsignaciones, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbAsignUsuario.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            tabPermisos.ResumeLayout(False)
            CType(_splitPermisos.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitPermisos.Panel1.ResumeLayout(False)
            CType(_splitPermisos.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitPermisos.Panel2.ResumeLayout(False)
            CType(_splitPermisos, System.ComponentModel.ISupportInitialize).EndInit()
            _splitPermisos.ResumeLayout(False)
            CType(_cmbPermisosRol.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_gridPermisos, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewPermisos, System.ComponentModel.ISupportInitialize).EndInit()
            tabMenu.ResumeLayout(False)
            CType(_splitMenu.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitMenu.Panel1.ResumeLayout(False)
            CType(_splitMenu.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitMenu.Panel2.ResumeLayout(False)
            CType(_splitMenu, System.ComponentModel.ISupportInitialize).EndInit()
            _splitMenu.ResumeLayout(False)
            CType(_cmbMenuRol.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_gridMenu, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewMenu, System.ComponentModel.ISupportInitialize).EndInit()
            tabSesiones.ResumeLayout(False)
            CType(_splitSesiones.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitSesiones.Panel1.ResumeLayout(False)
            CType(_splitSesiones.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitSesiones.Panel2.ResumeLayout(False)
            CType(_splitSesiones, System.ComponentModel.ISupportInitialize).EndInit()
            _splitSesiones.ResumeLayout(False)
            CType(_txtBuscarSesiones.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_gridSesiones, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewSesiones, System.ComponentModel.ISupportInitialize).EndInit()
            tabHelpdesk.ResumeLayout(False)
            CType(_splitHelpdesk.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitHelpdesk.Panel1.ResumeLayout(False)
            CType(_splitHelpdesk.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitHelpdesk.Panel2.ResumeLayout(False)
            CType(_splitHelpdesk, System.ComponentModel.ISupportInitialize).EndInit()
            _splitHelpdesk.ResumeLayout(False)
            CType(_memoHelpdeskMotivo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_gridHelpdesk, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewHelpdesk, System.ComponentModel.ISupportInitialize).EndInit()
            tabAuditoria.ResumeLayout(False)
            CType(_splitAuditoria.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
            _splitAuditoria.Panel1.ResumeLayout(False)
            CType(_splitAuditoria.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
            _splitAuditoria.Panel2.ResumeLayout(False)
            CType(_splitAuditoria, System.ComponentModel.ISupportInitialize).EndInit()
            _splitAuditoria.ResumeLayout(False)
            CType(_memoSimulador.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_gridAuditoria, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_viewAuditoria, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtBuscarUsuarios.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtUsuarioCodigo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtUsuarioNombre.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtUsuarioApellido.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtUsuarioNombreMostrar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtUsuarioCorreo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbUsuarioEstado.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_chkUsuarioActivo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_chkUsuarioMfa.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_chkUsuarioCambioClave.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtBuscarRoles.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtRolCodigo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_memoRolDescripcion.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_chkRolActivo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_chkRolSistema.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtBuscarAsignaciones.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbAsignRol.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbAsignAlcance.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_dteAsignInicio.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_dteAsignInicio.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_dteAsignFin.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_dteAsignFin.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_chkAsignActivo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtBuscarPermisos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtBuscarMenu.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbHelpdeskUsuario.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbHelpdeskAprobador.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_chkHelpdeskCritico.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtBuscarHelpdesk.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_txtBuscarAuditoria.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbSimUsuario.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(_cmbSimEmpresa.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
            PerformLayout()
        End Sub

        Private Sub ConfigureRibbon()
            _ribbon.Dock = DockStyle.Top
            _ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            _ribbon.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden
            _ribbon.MdiMergeStyle = If(IsDesignHost(), RibbonMdiMergeStyle.Never, RibbonMdiMergeStyle.Always)
            Ribbon = _ribbon

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
            AssignIcon(_btnResetClave, "BusinessObjects.BOChange")
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
            _btnResetClave.PaintStyle = BarItemPaintStyle.CaptionGlyph

            _ribbon.Items.AddRange(New BarItem() {
                _btnTabUsuarios, _btnTabRoles, _btnTabAsignaciones, _btnTabPermisos, _btnTabMenu, _btnTabSesiones, _btnTabHelpdesk, _btnTabAuditoria,
                _btnNuevo, _btnGuardar, _btnRefrescar, _btnDesactivar, _btnLimpiar, _btnResetClave,
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
            groupAcciones.ItemLinks.Add(_btnResetClave)

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

            TryBuildTab(tabUsuarios, AddressOf BuildUsuariosTab)
            TryBuildTab(tabRoles, AddressOf BuildRolesTab)
            TryBuildTab(tabAsignaciones, AddressOf BuildAsignacionesTab)
            TryBuildTab(tabPermisos, AddressOf BuildPermisosTab)
            TryBuildTab(tabMenu, AddressOf BuildMenuTab)
            TryBuildTab(tabSesiones, AddressOf BuildSesionesTab)
            TryBuildTab(tabHelpdesk, AddressOf BuildHelpdeskTab)
            TryBuildTab(tabAuditoria, AddressOf BuildAuditoriaTab)

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
            _ribbon.StatusBar = _statusBar
            StatusBar = _statusBar
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

        Friend WithEvents Root As LayoutControlGroup
        Friend WithEvents pageInicio As RibbonPage
        Friend WithEvents groupSecciones As RibbonPageGroup
        Friend WithEvents groupAcciones As RibbonPageGroup
        Friend WithEvents tabUsuarios As TabPage
        Friend WithEvents tabRoles As TabPage
        Friend WithEvents tabAsignaciones As TabPage
        Friend WithEvents tabPermisos As TabPage
        Friend WithEvents tabMenu As TabPage
        Friend WithEvents tabSesiones As TabPage
        Friend WithEvents tabHelpdesk As TabPage
        Friend WithEvents tabAuditoria As TabPage
        Friend WithEvents LayoutControlItem1 As LayoutControlItem
    End Class
End Namespace

