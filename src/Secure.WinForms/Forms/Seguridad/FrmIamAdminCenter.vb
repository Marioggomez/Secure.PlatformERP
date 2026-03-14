Imports System.Linq
Imports System.Net
Imports System.Security.Cryptography
Imports System.ComponentModel
Imports System.Text
Imports System.Text.Json
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
    Partial Public Class FrmIamAdminCenter
        Inherits RibbonForm
        Private Const PasswordHashAlgorithm As String = "SHA2_512"
        Private Const PasswordHashIterations As Integer = 100000
        Private Const PasswordUpperChars As String = "ABCDEFGHJKLMNPQRSTUVWXYZ"
        Private Const PasswordLowerChars As String = "abcdefghijkmnopqrstuvwxyz"
        Private Const PasswordNumberChars As String = "23456789"
        Private Const PasswordSpecialChars As String = "!@#$%*+-_=?."

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

        ' Bloque visual movido a FrmIamAdminCenter.Designer.vb.
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
            AddHandler _btnResetClave.ItemClick, AddressOf OnResetClaveClickAsync

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
                ApiErrorPresenter.Show(Me, ex, "registro Helpdesk MFA")
            Catch ex As Exception
                ApiErrorPresenter.ShowUnexpected(Me, ex, "registro Helpdesk MFA")
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
                ApiErrorPresenter.Show(Me, ex, "guardar en Centro IAM")
            Catch ex As Exception
                ApiErrorPresenter.ShowUnexpected(Me, ex, "guardar en Centro IAM")
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
            Dim isNewUser = Not _editingUsuarioId.HasValue

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

            If isNewUser AndAlso (Not _sessionContext.IdTenant.HasValue OrElse Not _sessionContext.IdEmpresa.HasValue) Then
                XtraMessageBox.Show(Me, "La sesion activa debe tener tenant y empresa para crear usuarios operables.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
            Try
                If _editingUsuarioId.HasValue Then
                    Await _apiClient.PutAsync("api/v1/seguridad/usuario/actualizar/" & _editingUsuarioId.Value.ToString(), dto).ConfigureAwait(True)
                Else
                    Dim createResult = Await _apiClient.PostAsync(Of UsuarioDto, Dictionary(Of String, Object))("api/v1/seguridad/usuario", dto).ConfigureAwait(True)
                    Dim idNuevoUsuario = ExtractCreatedId(createResult)
                    If Not idNuevoUsuario.HasValue Then
                        Throw New InvalidOperationException("La API no devolvio id del usuario creado.")
                    End If

                    Dim claveTemporal = Await GenerarClaveTemporalSeguraAsync().ConfigureAwait(True)
                    Await EnsureUsuarioTenantEmpresaAsync(idNuevoUsuario.Value, now).ConfigureAwait(True)
                    Await UpsertCredencialUsuarioAsync(idNuevoUsuario.Value, claveTemporal, True).ConfigureAwait(True)
                    Await MarcarUsuarioRequiereCambioClaveAsync(idNuevoUsuario.Value, True).ConfigureAwait(True)

                    XtraMessageBox.Show(
                        Me,
                        $"Usuario creado. Clave temporal inicial: {claveTemporal}{Environment.NewLine}Debe cambiarla en su primer ingreso.",
                        "Onboarding usuario",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
                End If

                Await LoadUsuariosAsync().ConfigureAwait(True)
                Await LoadAsignacionesAsync().ConfigureAwait(True)
                _statusInfo.Caption = "Usuario guardado correctamente."
            Finally
                SetBusy(False)
            End Try
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

        Private Async Sub OnResetClaveClickAsync(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            If _busy Then Return
            If _tabs.SelectedIndex <> 0 Then
                XtraMessageBox.Show(Me, "Reset clave aplica en la pestaÃ±a Usuarios.", "Reset clave", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If Not _editingUsuarioId.HasValue Then
                XtraMessageBox.Show(Me, "Seleccione un usuario para resetear su clave.", "Reset clave", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim sugerida = Await GenerarClaveTemporalSeguraAsync().ConfigureAwait(True)
            Dim input = XtraInputBox.Show("Nueva clave temporal para el usuario:", "Reset clave", sugerida)
            If input Is Nothing Then
                Return
            End If

            Dim nuevaClave = input.ToString().Trim()
            If String.IsNullOrWhiteSpace(nuevaClave) OrElse nuevaClave.Length < 8 Then
                XtraMessageBox.Show(Me, "La clave temporal debe tener al menos 8 caracteres.", "Reset clave", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            SetBusy(True, "Reseteando clave de usuario...")
            Try
                Await UpsertCredencialUsuarioAsync(_editingUsuarioId.Value, nuevaClave, True).ConfigureAwait(True)
                Await MarcarUsuarioRequiereCambioClaveAsync(_editingUsuarioId.Value, True).ConfigureAwait(True)
                Await LoadUsuariosAsync().ConfigureAwait(True)
                _statusInfo.Caption = "Clave temporal aplicada y usuario marcado para cambio obligatorio."
                XtraMessageBox.Show(Me, $"Clave temporal aplicada: {nuevaClave}", "Reset clave", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As ApiClientException
                ApiErrorPresenter.Show(Me, ex, "reset de clave de usuario")
            Catch ex As Exception
                ApiErrorPresenter.ShowUnexpected(Me, ex, "reset de clave de usuario")
            Finally
                SetBusy(False)
            End Try
        End Sub

        Private Async Function EnsureUsuarioTenantEmpresaAsync(ByVal idUsuario As Long, ByVal nowUtc As DateTime) As Task
            If Not _sessionContext.IdTenant.HasValue Then
                Throw New InvalidOperationException("No existe id_tenant en sesion para alta de usuario.")
            End If

            If Not _sessionContext.IdEmpresa.HasValue Then
                Throw New InvalidOperationException("No existe id_empresa en sesion para alta de usuario.")
            End If

            Dim idTenant = _sessionContext.IdTenant.Value
            Dim idEmpresa = _sessionContext.IdEmpresa.Value

            Dim tenantDto As UsuarioTenantDto = Nothing
            Try
                tenantDto = Await _apiClient.GetAsync(Of UsuarioTenantDto)("api/v1/seguridad/usuario_tenant/obtener/" & idUsuario.ToString()).ConfigureAwait(True)
            Catch ex As ApiClientException When ex.StatusCode = HttpStatusCode.NotFound
                tenantDto = Nothing
            End Try

            If tenantDto Is Nothing Then
                tenantDto = New UsuarioTenantDto With {
                    .IdUsuario = idUsuario,
                    .IdTenant = idTenant,
                    .EsAdministradorTenant = False,
                    .EsCuentaServicio = False,
                    .Activo = True,
                    .CreadoUtc = nowUtc,
                    .ActualizadoUtc = nowUtc
                }
                Await _apiClient.PostAsync(Of UsuarioTenantDto, Dictionary(Of String, Object))("api/v1/seguridad/usuario_tenant", tenantDto).ConfigureAwait(True)
            Else
                tenantDto.IdUsuario = idUsuario
                tenantDto.IdTenant = idTenant
                tenantDto.Activo = True
                tenantDto.ActualizadoUtc = nowUtc
                If Not tenantDto.CreadoUtc.HasValue Then
                    tenantDto.CreadoUtc = nowUtc
                End If
                Await _apiClient.PutAsync("api/v1/seguridad/usuario_tenant/actualizar/" & idUsuario.ToString(), tenantDto).ConfigureAwait(True)
            End If

            Dim empresas = Await _apiClient.GetAsync(Of List(Of UsuarioEmpresaDto))("api/v1/seguridad/usuario_empresa/listar").ConfigureAwait(True)
            Dim empresaDto = empresas.FirstOrDefault(
                Function(item) item.IdUsuario.HasValue AndAlso item.IdUsuario.Value = idUsuario AndAlso
                               item.IdTenant.HasValue AndAlso item.IdTenant.Value = idTenant AndAlso
                               item.IdEmpresa.HasValue AndAlso item.IdEmpresa.Value = idEmpresa)

            If empresaDto Is Nothing Then
                empresaDto = New UsuarioEmpresaDto With {
                    .IdUsuario = idUsuario,
                    .IdTenant = idTenant,
                    .IdEmpresa = idEmpresa,
                    .EsEmpresaPredeterminada = True,
                    .PuedeOperar = True,
                    .FechaInicioUtc = nowUtc,
                    .FechaFinUtc = Nothing,
                    .Activo = True,
                    .CreadoUtc = nowUtc,
                    .ActualizadoUtc = nowUtc
                }
                Await _apiClient.PostAsync(Of UsuarioEmpresaDto, Dictionary(Of String, Object))("api/v1/seguridad/usuario_empresa", empresaDto).ConfigureAwait(True)
            Else
                empresaDto.IdUsuario = idUsuario
                empresaDto.IdTenant = idTenant
                empresaDto.IdEmpresa = idEmpresa
                empresaDto.EsEmpresaPredeterminada = True
                empresaDto.PuedeOperar = True
                empresaDto.Activo = True
                empresaDto.FechaInicioUtc = If(empresaDto.FechaInicioUtc, nowUtc)
                empresaDto.FechaFinUtc = Nothing
                empresaDto.ActualizadoUtc = nowUtc
                If Not empresaDto.CreadoUtc.HasValue Then
                    empresaDto.CreadoUtc = nowUtc
                End If
                Await _apiClient.PutAsync("api/v1/seguridad/usuario_empresa/actualizar/" & empresaDto.IdUsuarioEmpresa.Value.ToString(), empresaDto).ConfigureAwait(True)
            End If
        End Function

        Private Async Function UpsertCredencialUsuarioAsync(ByVal idUsuario As Long, ByVal clavePlano As String, ByVal debeCambiarClave As Boolean) As Task
            Dim nowUtc = DateTime.UtcNow
            Dim salt = RandomNumberGenerator.GetBytes(32)
            Dim hash = CalcularHashContrasena(clavePlano, salt)

            Dim dto As New CredencialLocalUsuarioDto With {
                .IdUsuario = idUsuario,
                .HashClave = hash,
                .SaltClave = salt,
                .AlgoritmoClave = PasswordHashAlgorithm,
                .IteracionesClave = PasswordHashIterations,
                .CambioClaveUtc = nowUtc,
                .DebeCambiarClave = debeCambiarClave,
                .Activo = True
            }

            Dim credencialExiste = True
            Try
                Await _apiClient.GetAsync(Of CredencialLocalUsuarioDto)("api/v1/seguridad/credencial_local_usuario/obtener/" & idUsuario.ToString()).ConfigureAwait(True)
            Catch ex As ApiClientException When ex.StatusCode = HttpStatusCode.NotFound
                credencialExiste = False
            End Try

            If credencialExiste Then
                Await _apiClient.PutAsync("api/v1/seguridad/credencial_local_usuario/actualizar/" & idUsuario.ToString(), dto).ConfigureAwait(True)
            Else
                Await _apiClient.PostAsync(Of CredencialLocalUsuarioDto, Dictionary(Of String, Object))("api/v1/seguridad/credencial_local_usuario", dto).ConfigureAwait(True)
            End If
        End Function

        Private Async Function MarcarUsuarioRequiereCambioClaveAsync(ByVal idUsuario As Long, ByVal requiereCambio As Boolean) As Task
            Dim usuario = Await _apiClient.GetAsync(Of UsuarioDto)("api/v1/seguridad/usuario/obtener/" & idUsuario.ToString()).ConfigureAwait(True)
            If usuario Is Nothing Then
                Return
            End If

            usuario.RequiereCambioClave = requiereCambio
            usuario.ActualizadoUtc = DateTime.UtcNow
            usuario.ActualizadoPor = _sessionContext.IdUsuario
            Await _apiClient.PutAsync("api/v1/seguridad/usuario/actualizar/" & idUsuario.ToString(), usuario).ConfigureAwait(True)
        End Function

        Private Shared Function CalcularHashContrasena(ByVal clavePlano As String, ByVal salt As Byte()) As Byte()
            Dim claveBytes = Encoding.UTF8.GetBytes(clavePlano)
            Dim payload(salt.Length + claveBytes.Length - 1) As Byte
            Buffer.BlockCopy(salt, 0, payload, 0, salt.Length)
            Buffer.BlockCopy(claveBytes, 0, payload, salt.Length, claveBytes.Length)
            Return SHA512.HashData(payload)
        End Function

        Private Shared Function ExtractCreatedId(ByVal response As Dictionary(Of String, Object)) As Long?
            If response Is Nothing OrElse Not response.ContainsKey("id") Then
                Return Nothing
            End If

            Dim raw = response("id")
            If raw Is Nothing Then
                Return Nothing
            End If

            If TypeOf raw Is JsonElement Then
                Dim json = DirectCast(raw, JsonElement)
                If json.ValueKind = JsonValueKind.Number Then
                    Dim parsedNumber As Long
                    If json.TryGetInt64(parsedNumber) Then
                        Return parsedNumber
                    End If
                ElseIf json.ValueKind = JsonValueKind.String Then
                    Dim parsedText As Long
                    If Long.TryParse(json.GetString(), parsedText) Then
                        Return parsedText
                    End If
                End If
            End If

            Dim parsed As Long
            Return If(Long.TryParse(raw.ToString(), parsed), parsed, CType(Nothing, Long?))
        End Function

        Private Async Function GenerarClaveTemporalSeguraAsync() As Task(Of String)
            Dim longitudMinima As Integer = 12
            Dim requiereMayuscula As Boolean = True
            Dim requiereMinuscula As Boolean = True
            Dim requiereNumero As Boolean = True
            Dim requiereEspecial As Boolean = True

            Try
                If _sessionContext.IdTenant.HasValue Then
                    Dim politica = Await _apiClient.GetAsync(Of PoliticaTenantDto)("api/v1/seguridad/politica_tenant/obtener/" & _sessionContext.IdTenant.Value.ToString()).ConfigureAwait(True)
                    If politica IsNot Nothing Then
                        longitudMinima = Math.Max(12, CInt(If(politica.LongitudMinimaClave, CByte(12))))
                        requiereMayuscula = If(politica.RequiereMayuscula, True)
                        requiereMinuscula = If(politica.RequiereMinuscula, True)
                        requiereNumero = If(politica.RequiereNumero, True)
                        requiereEspecial = If(politica.RequiereEspecial, True)
                    End If
                End If
            Catch ex As Exception
                ' Si politica no esta disponible, se aplica baseline fuerte por defecto.
            End Try

            Dim grupos As New List(Of String)()
            If requiereMayuscula Then grupos.Add(PasswordUpperChars)
            If requiereMinuscula Then grupos.Add(PasswordLowerChars)
            If requiereNumero Then grupos.Add(PasswordNumberChars)
            If requiereEspecial Then grupos.Add(PasswordSpecialChars)
            If grupos.Count = 0 Then
                grupos.Add(PasswordUpperChars)
                grupos.Add(PasswordLowerChars)
                grupos.Add(PasswordNumberChars)
                grupos.Add(PasswordSpecialChars)
            End If

            If longitudMinima < grupos.Count Then
                longitudMinima = grupos.Count
            End If

            Dim todos = String.Concat(grupos)
            Dim chars As New List(Of Char)(longitudMinima)

            For Each grupo In grupos
                chars.Add(RandomCharFrom(grupo))
            Next

            While chars.Count < longitudMinima
                chars.Add(RandomCharFrom(todos))
            End While

            For i = chars.Count - 1 To 1 Step -1
                Dim j = RandomNumberGenerator.GetInt32(i + 1)
                Dim tmp = chars(i)
                chars(i) = chars(j)
                chars(j) = tmp
            Next

            Return New String(chars.ToArray())
        End Function

        Private Shared Function RandomCharFrom(ByVal source As String) As Char
            Dim idx = RandomNumberGenerator.GetInt32(source.Length)
            Return source(idx)
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
            If IsDesignHost() Then
                Return
            End If

            Try
                Dim svg = TryCast(IconService.GetIcon(iconKey), SvgImage)
                If svg IsNot Nothing Then
                    item.ImageOptions.SvgImage = svg
                End If
            Catch
                ' En diseñador no bloqueamos la carga visual por iconos.
            End Try
        End Sub

        Private Shared Function IsDesignHost() As Boolean
            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return True
            End If

            Try
                Dim exePath = Application.ExecutablePath
                Return Not String.IsNullOrWhiteSpace(exePath) AndAlso
                    (exePath.IndexOf("devenv", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                     exePath.IndexOf("xdesproc", StringComparison.OrdinalIgnoreCase) >= 0)
            Catch
                Return False
            End Try
        End Function

        Private Sub TryBuildTab(ByVal tab As TabPage, ByVal builder As Action(Of TabPage))
            Try
                builder(tab)
            Catch
                If Not IsDesignHost() Then
                    Throw
                End If

                tab.Controls.Clear()
                Dim placeholder As New LabelControl With {
                    .Text = "Vista previa de diseno: componente cargado con modo seguro.",
                    .Dock = DockStyle.Fill,
                    .AutoSizeMode = LabelAutoSizeMode.None
                }
                placeholder.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                placeholder.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
                placeholder.Appearance.ForeColor = Color.DimGray
                tab.Controls.Add(placeholder)
            End Try
        End Sub
    End Class
End Namespace


