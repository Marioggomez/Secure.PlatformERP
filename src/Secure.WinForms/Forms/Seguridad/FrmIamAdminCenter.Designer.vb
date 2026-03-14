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
            _btnResetClave = New BarButtonItem() With {.Caption = "Reset clave"}

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

            _statusBar.Ribbon = _ribbon
            _ribbon.StatusBar = _statusBar

            SuspendLayout()
            ' 
            ' FrmIamAdminCenter
            ' 
            AutoScaleDimensions = New SizeF(96.0F, 96.0F)
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
    End Class
End Namespace

