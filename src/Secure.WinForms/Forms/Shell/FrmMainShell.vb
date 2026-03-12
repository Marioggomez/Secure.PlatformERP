Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports DevExpress.XtraNavBar
Imports System.ComponentModel
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Forms.Empresas
Imports Secure.Platform.WinForms.Forms.Seguridad
Imports Secure.Platform.WinForms.Forms.Terceros
Imports Secure.Platform.WinForms.Forms.Usuarios
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Shell
    ''' <summary>
    ''' Shell principal ERP con ribbon, menu dinamico y area de trabajo tabulada.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Partial Class FrmMainShell
        Inherits RibbonForm

        Private ReadOnly _sessionContext As UserSessionContext
        Private ReadOnly _tenantCodigo As String
        Private ReadOnly _apiClient As ApiClient
        Private ReadOnly _recursosUi As IReadOnlyList(Of RecursoUiAccesoDto)

        Private ReadOnly _ribbon As RibbonControl
        Private ReadOnly _statusBar As RibbonStatusBar
        Private ReadOnly _navBar As NavBarControl
        Private ReadOnly _layout As LayoutControl
        Private ReadOnly _workspaceTabs As TabControl
        Private ReadOnly _statusUsuario As BarStaticItem
        Private ReadOnly _statusTenant As BarStaticItem
        Private ReadOnly _statusHora As BarStaticItem
        Private ReadOnly _statusTheme As BarStaticItem
        Private ReadOnly _statusModulo As BarStaticItem
        Private ReadOnly _skinGallery As SkinRibbonGalleryBarItem
        Private ReadOnly _clockTimer As Timer

        ''' <summary>
        ''' Constructor requerido por el diseñador WinForms.
        ''' </summary>
        Public Sub New()
            _sessionContext = UserSessionContext.CrearDiseno()
            _tenantCodigo = "SEED"
            _apiClient = New ApiClient(AppSettingsProvider.GetApiBaseUrl())
            _recursosUi = New List(Of RecursoUiAccesoDto)()

            _ribbon = New RibbonControl()
            _statusBar = New RibbonStatusBar()
            _navBar = New NavBarControl()
            _layout = New LayoutControl()
            _workspaceTabs = New TabControl()
            _statusUsuario = New BarStaticItem()
            _statusTenant = New BarStaticItem()
            _statusHora = New BarStaticItem()
            _statusTheme = New BarStaticItem()
            _statusModulo = New BarStaticItem()
            _skinGallery = New SkinRibbonGalleryBarItem()
            _clockTimer = New Timer()

            InitializeComponent()

            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If

            BuildDynamicMenu(_recursosUi)
            OpenDashboard()
        End Sub

        Public Sub New(
            ByVal sessionContext As UserSessionContext,
            ByVal tenantCodigo As String,
            ByVal apiClient As ApiClient,
            ByVal recursosUi As IEnumerable(Of RecursoUiAccesoDto))

            _sessionContext = If(sessionContext, UserSessionContext.CrearDiseno())
            _tenantCodigo = If(String.IsNullOrWhiteSpace(tenantCodigo), "N/A", tenantCodigo)
            _apiClient = apiClient
            _recursosUi = If(recursosUi IsNot Nothing, recursosUi.ToList(), New List(Of RecursoUiAccesoDto)())

            _ribbon = New RibbonControl()
            _statusBar = New RibbonStatusBar()
            _navBar = New NavBarControl()
            _layout = New LayoutControl()
            _workspaceTabs = New TabControl()
            _statusUsuario = New BarStaticItem()
            _statusTenant = New BarStaticItem()
            _statusHora = New BarStaticItem()
            _statusTheme = New BarStaticItem()
            _statusModulo = New BarStaticItem()
            _skinGallery = New SkinRibbonGalleryBarItem()
            _clockTimer = New Timer()

            InitializeComponent()

            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If

            BuildDynamicMenu(_recursosUi)
            OpenDashboard()
        End Sub

        Private Sub InitializeComponent()
            Text = "Secure Platform ERP"
            WindowState = FormWindowState.Maximized
            StartPosition = FormStartPosition.CenterScreen
            AutoScaleMode = AutoScaleMode.Dpi

            ConfigureRibbon()
            ConfigureNavBar()
            ConfigureWorkspace()
            ConfigureStatus()
            ConfigureClock()

            AddHandler Resize, AddressOf OnShellResize
            AddHandler DevExpress.LookAndFeel.UserLookAndFeel.Default.StyleChanged, AddressOf OnThemeChanged

            Controls.Add(_layout)
            Controls.Add(_navBar)
            Controls.Add(_statusBar)
            Controls.Add(_ribbon)
        End Sub

        Private Sub ConfigureRibbon()
            _ribbon.Dock = DockStyle.Top
            _ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            _ribbon.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden
            _ribbon.OptionsMenuMinWidth = 320

            Dim btnDashboard As New BarButtonItem() With {.Caption = "Dashboard"}
            Dim btnUsuarios As New BarButtonItem() With {.Caption = "Centro IAM"}
            Dim btnTerceros As New BarButtonItem() With {.Caption = "Terceros"}
            Dim btnEmpresas As New BarButtonItem() With {.Caption = "Empresas"}
            Dim btnApariencia As New BarButtonItem() With {.Caption = "Apariencia"}
            Dim btnCerrarPestana As New BarButtonItem() With {.Caption = "Cerrar pestana"}
            Dim btnCerrarTodas As New BarButtonItem() With {.Caption = "Cerrar todas"}
            Dim btnCerrarSesion As New BarButtonItem() With {.Caption = "Cerrar sesion"}

            btnDashboard.ImageOptions.SvgImage = TryCast(IconService.GetIcon("Navigation.Home"), SvgImage)
            btnUsuarios.ImageOptions.SvgImage = TryCast(IconService.GetIcon("BusinessObjects.BOUser"), SvgImage)
            btnTerceros.ImageOptions.SvgImage = TryCast(IconService.GetIcon("BusinessObjects.BOPerson"), SvgImage)
            btnEmpresas.ImageOptions.SvgImage = TryCast(IconService.GetIcon("BusinessObjects.BOOrganization"), SvgImage)
            btnApariencia.ImageOptions.SvgImage = TryCast(IconService.GetIcon("Image.ColorBalance"), SvgImage)
            btnCerrarPestana.ImageOptions.SvgImage = TryCast(IconService.GetIcon("Actions.Cancel"), SvgImage)
            btnCerrarTodas.ImageOptions.SvgImage = TryCast(IconService.GetIcon("Actions.Clear"), SvgImage)
            btnCerrarSesion.ImageOptions.SvgImage = TryCast(IconService.GetIcon("Actions.Close"), SvgImage)

            _skinGallery.Caption = "Temas"

            _ribbon.Items.AddRange(New BarItem() {
                btnDashboard,
                btnUsuarios,
                btnTerceros,
                btnEmpresas,
                btnApariencia,
                btnCerrarPestana,
                btnCerrarTodas,
                btnCerrarSesion,
                _skinGallery,
                _statusUsuario,
                _statusTenant,
                _statusHora,
                _statusTheme,
                _statusModulo
            })

            Dim pageInicio As New RibbonPage("Inicio")
            Dim pageVista As New RibbonPage("Vista")
            Dim pageSesion As New RibbonPage("Sesion")
            pageInicio.Name = "rpInicio"
            pageVista.Name = "rpVista"
            pageSesion.Name = "rpSesion"

            Dim groupNavegacion As New RibbonPageGroup("Navegacion")
            groupNavegacion.ItemLinks.Add(btnDashboard)
            groupNavegacion.ItemLinks.Add(btnUsuarios)
            groupNavegacion.ItemLinks.Add(btnTerceros)
            groupNavegacion.ItemLinks.Add(btnEmpresas)
            groupNavegacion.ItemLinks.Add(btnApariencia)

            Dim groupVentanas As New RibbonPageGroup("Ventanas")
            groupVentanas.ItemLinks.Add(btnCerrarPestana)
            groupVentanas.ItemLinks.Add(btnCerrarTodas)

            Dim groupApariencia As New RibbonPageGroup("Apariencia")
            groupApariencia.ItemLinks.Add(_skinGallery)

            Dim groupSesion As New RibbonPageGroup("Cuenta")
            groupSesion.ItemLinks.Add(btnCerrarSesion)

            pageInicio.Groups.Add(groupNavegacion)
            pageInicio.Groups.Add(groupVentanas)
            pageVista.Groups.Add(groupApariencia)
            pageSesion.Groups.Add(groupSesion)

            _ribbon.Pages.Add(pageInicio)
            _ribbon.Pages.Add(pageVista)
            _ribbon.Pages.Add(pageSesion)

            AddHandler btnDashboard.ItemClick, AddressOf OnDashboardClick
            AddHandler btnUsuarios.ItemClick, AddressOf OnUsuariosClick
            AddHandler btnTerceros.ItemClick, AddressOf OnTercerosClick
            AddHandler btnEmpresas.ItemClick, AddressOf OnEmpresasClick
            AddHandler btnApariencia.ItemClick, AddressOf OnAparienciaClick
            AddHandler btnCerrarPestana.ItemClick, AddressOf OnCerrarPestanaClick
            AddHandler btnCerrarTodas.ItemClick, AddressOf OnCerrarTodasClick
            AddHandler btnCerrarSesion.ItemClick, AddressOf OnCerrarSesionClick
        End Sub

        Private Sub OnDashboardClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OpenDashboard()
        End Sub

        Private Sub OnUsuariosClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OpenModule("FrmIamAdminCenter")
        End Sub

        Private Sub OnTercerosClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OpenModule("FrmTercerosBuscar")
        End Sub

        Private Sub OnEmpresasClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OpenModule("FrmEmpresasBuscar")
        End Sub

        Private Sub OnAparienciaClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OpenModule("FrmAparienciaHerramientas")
        End Sub

        Private Sub ConfigureNavBar()
            _navBar.Dock = DockStyle.Left
            _navBar.Width = 270
            _navBar.PaintStyleKind = NavBarViewKind.NavigationPane
            _navBar.OptionsNavPane.ExpandedWidth = 270
            _navBar.ShowLinkHint = True
        End Sub

        Private Sub ConfigureWorkspace()
            _layout.Dock = DockStyle.Fill
            _layout.Root = New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }

            _workspaceTabs.Dock = DockStyle.Fill
            _workspaceTabs.Padding = New Point(18, 6)
            _workspaceTabs.HotTrack = True
            AddHandler _workspaceTabs.SelectedIndexChanged, AddressOf OnWorkspaceTabChanged

            _layout.Controls.Add(_workspaceTabs)
            Dim item = DirectCast(_layout.Root, LayoutControlGroup).AddItem(String.Empty, _workspaceTabs)
            item.TextVisible = False
        End Sub

        Private Sub ConfigureStatus()
            _statusBar.Dock = DockStyle.Bottom
            _statusBar.Ribbon = _ribbon

            _statusUsuario.Caption = $"Usuario: {_sessionContext.Usuario}"
            _statusTenant.Caption = $"Tenant: {_tenantCodigo} ({_sessionContext.IdTenant})"
            _statusHora.Caption = $"Hora: {DateTime.Now:HH:mm:ss}"
            _statusTheme.Caption = $"Tema: {ThemeService.GetCurrentTheme()}"
            _statusModulo.Caption = "Modulo: Inicio"

            _statusBar.ItemLinks.Add(_statusUsuario)
            _statusBar.ItemLinks.Add(_statusTenant)
            _statusBar.ItemLinks.Add(_statusModulo)
            _statusBar.ItemLinks.Add(_statusTheme)
            _statusBar.ItemLinks.Add(_statusHora)
        End Sub

        Private Sub ConfigureClock()
            _clockTimer.Interval = 1000
            AddHandler _clockTimer.Tick, AddressOf OnClockTick
            _clockTimer.Start()
        End Sub

        ''' <summary>
        ''' Genera el menu lateral en base a recursos_ui devueltos por API.
        ''' </summary>
        Public Sub BuildDynamicMenu(ByVal recursos As IEnumerable(Of RecursoUiAccesoDto))
            _navBar.Groups.Clear()

            Dim groupGeneral As New NavBarGroup("General") With {
                .Expanded = True
            }
            _navBar.Groups.Add(groupGeneral)

            Dim finalResources = recursos.Where(Function(x) x IsNot Nothing).OrderBy(Function(x) x.OrdenVisual).ToList()
            If finalResources.Count = 0 Then
                finalResources = GetDefaultResources()
            Else
                Dim defaults = GetDefaultResources()
                For Each item In defaults
                    If Not finalResources.Any(Function(existing) String.Equals(existing.Codigo, item.Codigo, StringComparison.OrdinalIgnoreCase)) Then
                        finalResources.Add(item)
                    End If
                Next

                finalResources = finalResources.OrderBy(Function(x) x.OrdenVisual).ToList()
            End If

            Dim homeItem As New NavBarItem("Inicio") With {
                .Tag = "Home",
                .Hint = "/"
            }
            homeItem.ImageOptions.SvgImage = TryCast(IconService.GetIcon("Navigation.Home"), SvgImage)
            groupGeneral.ItemLinks.Add(homeItem)
            AddHandler homeItem.LinkClicked, AddressOf OnMenuItemClicked

            Dim normalizedResources = finalResources.
                Where(Function(item) Not IsHomeResource(item)).
                GroupBy(Function(item) item.IdRecursoUi).
                Select(Function(grouping) grouping.OrderBy(Function(item) item.OrdenVisual).First()).
                ToList()

            Dim resourcesById = normalizedResources.ToDictionary(Function(item) item.IdRecursoUi)
            Dim childrenByParent As New Dictionary(Of Long, List(Of RecursoUiAccesoDto))()
            Dim rootResources As New List(Of RecursoUiAccesoDto)()

            For Each item In normalizedResources
                If item.IdRecursoUiPadre.HasValue AndAlso resourcesById.ContainsKey(item.IdRecursoUiPadre.Value) Then
                    Dim parentId = item.IdRecursoUiPadre.Value
                    If Not childrenByParent.ContainsKey(parentId) Then
                        childrenByParent(parentId) = New List(Of RecursoUiAccesoDto)()
                    End If

                    childrenByParent(parentId).Add(item)
                Else
                    rootResources.Add(item)
                End If
            Next

            For Each parentKey In childrenByParent.Keys.ToList()
                childrenByParent(parentKey) = childrenByParent(parentKey).
                    OrderBy(Function(item) item.OrdenVisual).
                    ThenBy(Function(item) item.Nombre).
                    ToList()
            Next

            If rootResources.Count = 0 Then
                rootResources.AddRange(normalizedResources)
            End If

            Dim moduleGroups As New Dictionary(Of String, NavBarGroup)(StringComparer.OrdinalIgnoreCase)
            Dim renderedResourceIds As New HashSet(Of Long)()
            For Each rootItem In rootResources.OrderBy(Function(item) item.OrdenVisual).ThenBy(Function(item) item.Nombre)
                Dim moduleKey = ResolveModuleKey(rootItem)
                Dim moduleGroup As NavBarGroup = Nothing
                If Not moduleGroups.TryGetValue(moduleKey, moduleGroup) Then
                    moduleGroup = New NavBarGroup(GetModuleCaption(moduleKey)) With {
                        .Expanded = True
                    }
                    moduleGroups(moduleKey) = moduleGroup
                    _navBar.Groups.Add(moduleGroup)
                End If

                AddResourceTree(moduleGroup, rootItem, childrenByParent, 0, renderedResourceIds, New HashSet(Of Long)())
            Next

            Dim pendingResources = normalizedResources.
                Where(Function(item) Not renderedResourceIds.Contains(item.IdRecursoUi)).
                OrderBy(Function(item) item.OrdenVisual).
                ThenBy(Function(item) item.Nombre).
                ToList()

            For Each pendingItem In pendingResources
                Dim moduleKey = ResolveModuleKey(pendingItem)
                Dim moduleGroup As NavBarGroup = Nothing
                If Not moduleGroups.TryGetValue(moduleKey, moduleGroup) Then
                    moduleGroup = New NavBarGroup(GetModuleCaption(moduleKey)) With {
                        .Expanded = True
                    }
                    moduleGroups(moduleKey) = moduleGroup
                    _navBar.Groups.Add(moduleGroup)
                End If

                AddResourceTree(moduleGroup, pendingItem, childrenByParent, 0, renderedResourceIds, New HashSet(Of Long)())
            Next

            Dim toolsItem As New NavBarItem("Apariencia y Herramientas") With {
                .Tag = "FrmAparienciaHerramientas",
                .Hint = "/herramientas/apariencia"
            }
            toolsItem.ImageOptions.SvgImage = TryCast(IconService.GetIcon("Image.ColorBalance"), SvgImage)
            groupGeneral.ItemLinks.Add(toolsItem)
            AddHandler toolsItem.LinkClicked, AddressOf OnMenuItemClicked
        End Sub

        Private Sub AddResourceTree(
            ByVal group As NavBarGroup,
            ByVal item As RecursoUiAccesoDto,
            ByVal childrenByParent As Dictionary(Of Long, List(Of RecursoUiAccesoDto)),
            ByVal depth As Integer,
            ByVal renderedResourceIds As HashSet(Of Long),
            ByVal branchVisited As HashSet(Of Long))

            If item Is Nothing Then Return
            If renderedResourceIds.Contains(item.IdRecursoUi) Then Return
            If branchVisited.Contains(item.IdRecursoUi) Then Return

            branchVisited.Add(item.IdRecursoUi)

            Dim children As List(Of RecursoUiAccesoDto) = Nothing
            Dim hasChildren = childrenByParent.TryGetValue(item.IdRecursoUi, children) AndAlso
                children.Any(Function(child) Not renderedResourceIds.Contains(child.IdRecursoUi))
            RegisterResourceNavItem(group, item, depth, hasChildren)
            renderedResourceIds.Add(item.IdRecursoUi)

            If Not hasChildren Then
                branchVisited.Remove(item.IdRecursoUi)
                Return
            End If

            For Each child In children
                AddResourceTree(group, child, childrenByParent, depth + 1, renderedResourceIds, branchVisited)
            Next

            branchVisited.Remove(item.IdRecursoUi)
        End Sub

        Private Sub RegisterResourceNavItem(
            ByVal group As NavBarGroup,
            ByVal item As RecursoUiAccesoDto,
            ByVal depth As Integer,
            ByVal hasChildren As Boolean)

            Dim captionPrefix = If(depth > 0, New String(" "c, depth * 2) & "|- ", String.Empty)
            Dim navItem As New NavBarItem(captionPrefix & item.Nombre) With {
                .Tag = item.Componente,
                .Hint = item.Ruta
            }

            Dim svg = TryCast(IconService.GetIcon(item.Icono), SvgImage)
            If svg IsNot Nothing Then
                navItem.ImageOptions.SvgImage = svg
            End If

            If String.IsNullOrWhiteSpace(item.Componente) AndAlso hasChildren Then
                navItem.Enabled = False
            End If

            group.ItemLinks.Add(navItem)
            AddHandler navItem.LinkClicked, AddressOf OnMenuItemClicked
        End Sub

        Private Shared Function IsHomeResource(ByVal item As RecursoUiAccesoDto) As Boolean
            If item Is Nothing Then Return False
            If String.Equals(item.Codigo, "NAV.HOME", StringComparison.OrdinalIgnoreCase) Then Return True
            Return String.Equals(item.Componente, "Home", StringComparison.OrdinalIgnoreCase)
        End Function

        Private Shared Function ResolveModuleKey(ByVal item As RecursoUiAccesoDto) As String
            If item Is Nothing Then Return "general"

            Dim route = If(item.Ruta, String.Empty).Trim()
            If route.StartsWith("/", StringComparison.Ordinal) Then route = route.Substring(1)
            If Not String.IsNullOrWhiteSpace(route) Then
                Dim routeParts = route.Split("/"c, StringSplitOptions.RemoveEmptyEntries)
                If routeParts.Length > 0 Then
                    Return routeParts(0).Trim().ToLowerInvariant()
                End If
            End If

            Dim code = If(item.Codigo, String.Empty).Trim()
            If code.StartsWith("NAV.", StringComparison.OrdinalIgnoreCase) Then
                Dim raw = code.Substring(4)
                Dim separators = New Char() {"."c, "_"c}
                Dim codeParts = raw.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                If codeParts.Length > 0 Then
                    Return codeParts(0).Trim().ToLowerInvariant()
                End If
            End If

            Return "general"
        End Function

        Private Shared Function GetModuleCaption(ByVal moduleKey As String) As String
            Dim normalized = If(moduleKey, String.Empty).Trim().ToLowerInvariant()

            Select Case normalized
                Case "seguridad", "iam"
                    Return "Seguridad"
                Case "tercero", "terceros"
                    Return "Tercero"
                Case "organizacion"
                    Return "Organizacion"
                Case "catalogo", "catalogos"
                    Return "Catalogo"
                Case "plataforma"
                    Return "Plataforma"
                Case "cumplimiento"
                    Return "Cumplimiento"
                Case "observabilidad"
                    Return "Observabilidad"
                Case "general", ""
                    Return "General"
                Case Else
                    Return Char.ToUpperInvariant(normalized(0)) & normalized.Substring(1)
            End Select
        End Function

        Private Sub OnMenuItemClicked(ByVal sender As Object, ByVal e As NavBarLinkEventArgs)
            Dim key = TryCast(e.Link.Item.Tag, String)
            If String.IsNullOrWhiteSpace(key) Then
                Return
            End If

            If String.Equals(key, "Home", StringComparison.OrdinalIgnoreCase) Then
                OpenDashboard()
            Else
                OpenModule(key)
            End If
        End Sub

        Private Sub OpenModule(ByVal moduleKey As String)
            Dim moduleForm = CreateModuleForm(moduleKey)
            If moduleForm Is Nothing Then
                XtraMessageBox.Show(Me, $"No existe una pantalla registrada para: {moduleKey}", "Navegacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim tabTitle = BuildUniqueTabTitle(GetModuleFormTitle(moduleForm))
            OpenFormInNewTab(moduleForm, tabTitle)
        End Sub

        Private Function CreateModuleForm(ByVal moduleKey As String) As XtraForm
            Select Case moduleKey
                Case "FrmIamAdminCenter", "FrmUsuariosBuscar", "FrmRolesBuscar", "FrmAsignacionRolUsuarioBuscar"
                    Return New FrmIamAdminCenter(_apiClient, _sessionContext)
                Case "FrmUsuarioEdit"
                    Return New FrmUsuarioEdit()
                Case "FrmTercerosBuscar"
                    Return New FrmTercerosBuscar(_apiClient, _sessionContext)
                Case "FrmTipoPersonaBuscar"
                    Return New FrmTipoPersonaBuscar(_apiClient, _sessionContext)
                Case "FrmIdentificacionTerceroBuscar"
                    Return New FrmIdentificacionTerceroBuscar(_apiClient, _sessionContext)
                Case "FrmDireccionTerceroBuscar"
                    Return New FrmDireccionTerceroBuscar(_apiClient, _sessionContext)
                Case "FrmContactoTerceroBuscar"
                    Return New FrmContactoTerceroBuscar(_apiClient, _sessionContext)
                Case "FrmCuentaBancariaTerceroBuscar"
                    Return New FrmCuentaBancariaTerceroBuscar(_apiClient, _sessionContext)
                Case "FrmTerceroRolBuscar"
                    Return New FrmTerceroRolBuscar(_apiClient, _sessionContext)
                Case "FrmEmpresasBuscar"
                    Return New FrmEmpresasBuscar(_apiClient, _sessionContext)
                Case "FrmEmpresaEdit"
                    Return New FrmEmpresaEdit()
                Case "FrmAparienciaHerramientas"
                    Return New FrmAparienciaHerramientas()
                Case Else
                    Return Nothing
            End Select
        End Function

        Private Sub OpenFormInNewTab(ByVal child As XtraForm, ByVal tabTitle As String)
            If TypeOf child Is BaseSearchForm Then
                DirectCast(child, BaseSearchForm).SetRibbonHostMode(True)
            End If

            If TypeOf child Is BaseEditForm Then
                DirectCast(child, BaseEditForm).SetRibbonHostMode(True)
            End If

            child.TopLevel = False
            child.FormBorderStyle = FormBorderStyle.None
            child.Dock = DockStyle.Fill

            Dim tabPage As New TabPage(tabTitle) With {
                .Tag = child
            }
            tabPage.Controls.Add(child)
            _workspaceTabs.TabPages.Add(tabPage)
            _workspaceTabs.SelectedTab = tabPage

            child.Show()
            RefreshRibbonMerge()
            UpdateModuleStatus(tabTitle)
        End Sub

        Private Sub OpenDashboard()
            Dim existingDashboard = GetDashboardTab()
            If existingDashboard IsNot Nothing Then
                _workspaceTabs.SelectedTab = existingDashboard
                RefreshRibbonMerge()
                UpdateModuleStatus(existingDashboard.Text)
                Return
            End If

            Dim dashboardPanel = BuildDashboardPanel()
            Dim page As New TabPage("Inicio") With {
                .Tag = "DASHBOARD"
            }
            page.Controls.Add(dashboardPanel)

            _workspaceTabs.TabPages.Insert(0, page)
            _workspaceTabs.SelectedTab = page
            RefreshRibbonMerge()
            UpdateModuleStatus(page.Text)
        End Sub

        Private Function BuildDashboardPanel() As Control
            Dim panel As New PanelControl() With {
                .Dock = DockStyle.Fill,
                .BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            }

            Dim title As New LabelControl() With {
                .Text = "Bienvenido a Secure Platform ERP"
            }
            title.Appearance.Font = New Font("Segoe UI", 22.0F, FontStyle.Bold)
            title.Location = New Point(36, 36)

            Dim subtitle As New LabelControl() With {
                .Text = $"Sesion activa de {_sessionContext.Usuario} en tenant {_tenantCodigo}.",
                .AutoSizeMode = LabelAutoSizeMode.None
            }
            subtitle.Appearance.Font = New Font("Segoe UI", 11.0F)
            subtitle.Appearance.ForeColor = Color.DimGray
            subtitle.Location = New Point(40, 90)
            subtitle.Width = 760

            Dim apitxt As New LabelControl() With {
                .Text = $"API conectada: {_apiClient.BaseAddress}",
                .AutoSizeMode = LabelAutoSizeMode.None
            }
            apitxt.Appearance.Font = New Font("Segoe UI", 9.0F)
            apitxt.Appearance.ForeColor = Color.Gray
            apitxt.Location = New Point(40, 122)
            apitxt.Width = 860

            panel.Controls.Add(title)
            panel.Controls.Add(subtitle)
            panel.Controls.Add(apitxt)

            Return panel
        End Function

        Private Function GetDashboardTab() As TabPage
            For Each page As TabPage In _workspaceTabs.TabPages
                If TypeOf page.Tag Is String AndAlso String.Equals(CStr(page.Tag), "DASHBOARD", StringComparison.Ordinal) Then
                    Return page
                End If
            Next

            Return Nothing
        End Function

        Private Function BuildUniqueTabTitle(ByVal baseTitle As String) As String
            Dim count = 0
            For Each page As TabPage In _workspaceTabs.TabPages
                If page.Text.StartsWith(baseTitle, StringComparison.OrdinalIgnoreCase) Then
                    count += 1
                End If
            Next

            If count = 0 Then
                Return baseTitle
            End If

            Return $"{baseTitle} ({count + 1})"
        End Function

        Private Function GetModuleFormTitle(ByVal form As XtraForm) As String
            If TypeOf form Is FrmIamAdminCenter Then
                Return DirectCast(form, FrmIamAdminCenter).ModuleTitle
            End If

            If TypeOf form Is BaseSearchForm Then
                Return DirectCast(form, BaseSearchForm).ModuleTitle
            End If

            If TypeOf form Is BaseEditForm Then
                Return DirectCast(form, BaseEditForm).ModuleTitle
            End If

            Return form.Text
        End Function

        Private Sub OnWorkspaceTabChanged(ByVal sender As Object, ByVal e As EventArgs)
            RefreshRibbonMerge()

            If _workspaceTabs.SelectedTab IsNot Nothing Then
                UpdateModuleStatus(_workspaceTabs.SelectedTab.Text)
            End If
        End Sub

        Private Sub RefreshRibbonMerge()
            Try
                _ribbon.UnMergeRibbon()
            Catch
                ' Ignorar cualquier estado previo de merge.
            End Try

            Dim activeRibbon As RibbonControl = Nothing
            Dim activePage = _workspaceTabs.SelectedTab
            If activePage Is Nothing Then
                Return
            End If

            If TypeOf activePage.Tag Is BaseSearchForm Then
                activeRibbon = DirectCast(activePage.Tag, BaseSearchForm).ModuleRibbon
            ElseIf TypeOf activePage.Tag Is BaseEditForm Then
                activeRibbon = DirectCast(activePage.Tag, BaseEditForm).ModuleRibbon
            ElseIf TypeOf activePage.Tag Is FrmIamAdminCenter Then
                activeRibbon = DirectCast(activePage.Tag, FrmIamAdminCenter).ModuleRibbon
            End If

            If activeRibbon IsNot Nothing Then
                _ribbon.MergeRibbon(activeRibbon)
            End If

            FocusHomeRibbonPage()
        End Sub

        Private Sub FocusHomeRibbonPage()
            For Each page As RibbonPage In _ribbon.Pages
                If String.Equals(page.Text, "Inicio", StringComparison.OrdinalIgnoreCase) Then
                    _ribbon.SelectedPage = page
                    Exit For
                End If
            Next
        End Sub

        Private Sub OnCerrarPestanaClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _workspaceTabs.SelectedTab Is Nothing Then
                Return
            End If

            Dim page = _workspaceTabs.SelectedTab
            If TypeOf page.Tag Is String AndAlso String.Equals(CStr(page.Tag), "DASHBOARD", StringComparison.Ordinal) Then
                Return
            End If

            CloseTab(page)
        End Sub

        Private Sub OnCerrarTodasClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Dim pages As New List(Of TabPage)()
            For Each page As TabPage In _workspaceTabs.TabPages
                If TypeOf page.Tag Is XtraForm Then
                    pages.Add(page)
                End If
            Next

            For Each page In pages
                CloseTab(page)
            Next

            OpenDashboard()
        End Sub

        Private Sub CloseTab(ByVal page As TabPage)
            If page Is Nothing Then
                Return
            End If

            If TypeOf page.Tag Is XtraForm Then
                Dim form = DirectCast(page.Tag, XtraForm)
                form.Close()
                form.Dispose()
            End If

            _workspaceTabs.TabPages.Remove(page)
            page.Dispose()

            If _workspaceTabs.TabPages.Count = 0 Then
                OpenDashboard()
            Else
                RefreshRibbonMerge()
                If _workspaceTabs.SelectedTab IsNot Nothing Then
                    UpdateModuleStatus(_workspaceTabs.SelectedTab.Text)
                End If
            End If
        End Sub

        Private Sub UpdateModuleStatus(ByVal moduleTitle As String)
            _statusModulo.Caption = $"Modulo: {moduleTitle}"
        End Sub

        Private Sub OnClockTick(ByVal sender As Object, ByVal e As EventArgs)
            _statusHora.Caption = $"Hora: {DateTime.Now:HH:mm:ss}"
        End Sub

        Private Sub OnThemeChanged(ByVal sender As Object, ByVal e As EventArgs)
            _statusTheme.Caption = $"Tema: {ThemeService.GetCurrentTheme()}"
        End Sub

        Private Sub OnShellResize(ByVal sender As Object, ByVal e As EventArgs)
            Dim idealWidth = CInt(Math.Max(230, Me.ClientSize.Width * 0.15))
            _navBar.Width = idealWidth
            _navBar.OptionsNavPane.ExpandedWidth = idealWidth
        End Sub

        Private Sub OnCerrarSesionClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If XtraMessageBox.Show(Me, "Desea cerrar sesion?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Close()
            End If
        End Sub

        Private Function GetDefaultResources() As List(Of RecursoUiAccesoDto)
            Return New List(Of RecursoUiAccesoDto) From {
                New RecursoUiAccesoDto With {.IdRecursoUi = 200, .Codigo = "NAV.SEGURIDAD", .Nombre = "Seguridad", .Componente = "FrmIamAdminCenter", .Ruta = "/seguridad", .Icono = "BusinessObjects.BOUser", .OrdenVisual = 10},
                New RecursoUiAccesoDto With {.IdRecursoUi = 201, .Codigo = "NAV.SEGURIDAD.IAM", .Nombre = "Centro IAM", .Componente = "FrmIamAdminCenter", .Ruta = "/seguridad/iam", .Icono = "BusinessObjects.BOUser", .OrdenVisual = 11, .IdRecursoUiPadre = 200},
                New RecursoUiAccesoDto With {.IdRecursoUi = 300, .Codigo = "NAV.ORGANIZACION", .Nombre = "Organizacion", .Componente = "FrmEmpresasBuscar", .Ruta = "/organizacion", .Icono = "BusinessObjects.BOOrganization", .OrdenVisual = 20},
                New RecursoUiAccesoDto With {.IdRecursoUi = 301, .Codigo = "NAV.EMPRESAS", .Nombre = "Empresas", .Componente = "FrmEmpresasBuscar", .Ruta = "/organizacion/empresas", .Icono = "Edit.Edit", .OrdenVisual = 21, .IdRecursoUiPadre = 300},
                New RecursoUiAccesoDto With {.IdRecursoUi = 400, .Codigo = "NAV.TERCEROS", .Nombre = "Terceros", .Componente = "FrmTercerosBuscar", .Ruta = "/tercero/terceros", .Icono = "BusinessObjects.BOPerson", .OrdenVisual = 30},
                New RecursoUiAccesoDto With {.IdRecursoUi = 401, .Codigo = "NAV.TIPO_PERSONA", .Nombre = "Tipos Persona", .Componente = "FrmTipoPersonaBuscar", .Ruta = "/tercero/tipo-persona", .Icono = "BusinessObjects.BOContact", .OrdenVisual = 31, .IdRecursoUiPadre = 400},
                New RecursoUiAccesoDto With {.IdRecursoUi = 402, .Codigo = "NAV.IDENTIFICACION_TERCERO", .Nombre = "Identificaciones", .Componente = "FrmIdentificacionTerceroBuscar", .Ruta = "/tercero/identificaciones", .Icono = "BusinessObjects.BOValidation", .OrdenVisual = 32, .IdRecursoUiPadre = 400},
                New RecursoUiAccesoDto With {.IdRecursoUi = 403, .Codigo = "NAV.DIRECCION_TERCERO", .Nombre = "Direcciones", .Componente = "FrmDireccionTerceroBuscar", .Ruta = "/tercero/direcciones", .Icono = "BusinessObjects.BOAddress", .OrdenVisual = 33, .IdRecursoUiPadre = 400},
                New RecursoUiAccesoDto With {.IdRecursoUi = 404, .Codigo = "NAV.CONTACTO_TERCERO", .Nombre = "Contactos", .Componente = "FrmContactoTerceroBuscar", .Ruta = "/tercero/contactos", .Icono = "BusinessObjects.BOLead", .OrdenVisual = 34, .IdRecursoUiPadre = 400},
                New RecursoUiAccesoDto With {.IdRecursoUi = 405, .Codigo = "NAV.CUENTA_BANCARIA_TERCERO", .Nombre = "Cuentas Bancarias", .Componente = "FrmCuentaBancariaTerceroBuscar", .Ruta = "/tercero/cuentas", .Icono = "BusinessObjects.BOInvoice", .OrdenVisual = 35, .IdRecursoUiPadre = 400},
                New RecursoUiAccesoDto With {.IdRecursoUi = 406, .Codigo = "NAV.TERCERO_ROL", .Nombre = "Roles de Tercero", .Componente = "FrmTerceroRolBuscar", .Ruta = "/tercero/roles", .Icono = "BusinessObjects.BORole", .OrdenVisual = 36, .IdRecursoUiPadre = 400}
            }
        End Function
    End Class
End Namespace
