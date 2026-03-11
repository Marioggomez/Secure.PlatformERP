Imports DevExpress.XtraEditors
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Auth
    ''' <summary>
    ''' Pantalla para iniciar y completar recuperacion de contrasena.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Partial Public Class FrmRecuperarContrasena
        Inherits XtraForm

        Private ReadOnly _apiClient As ApiClient
        Private _idFlujoRestablecimiento As Guid

        ''' <summary>
        ''' Constructor requerido por el diseñador WinForms.
        ''' </summary>
        Public Sub New()
            Me.New(CreateDefaultApiClient(), "SEED", String.Empty)
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal tenantInicial As String, ByVal usuarioInicial As String)
            _apiClient = apiClient
            InitializeComponent()

            _txtTenant.Text = If(String.IsNullOrWhiteSpace(tenantInicial), "SEED", tenantInicial)
            _txtUsuarioCorreo.Text = usuarioInicial
            _idFlujoRestablecimiento = Guid.Empty

            WireEvents()
            ToggleCompletar(False)
        End Sub

        Private Shared Function CreateDefaultApiClient() As ApiClient
            Dim baseUrl = AppSettingsProvider.GetApiBaseUrl()
            Return New ApiClient(baseUrl)
        End Function

        Private Sub WireEvents()
            AddHandler _btnEnviar.Click, AddressOf OnEnviarClickAsync
            AddHandler _btnCompletar.Click, AddressOf OnCompletarClickAsync
            AddHandler _btnCerrar.Click, AddressOf OnCerrarClick
        End Sub

        Private Async Sub OnEnviarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            If String.IsNullOrWhiteSpace(_txtTenant.Text) Then
                XtraMessageBox.Show(Me, "Ingrese el tenant.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtTenant.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(_txtUsuarioCorreo.Text) Then
                XtraMessageBox.Show(Me, "Ingrese un usuario o correo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtUsuarioCorreo.Focus()
                Return
            End If

            ToggleUi(False)

            Try
                Dim request As New IniciarRestablecimientoClaveRequestDto With {
                    .TenantCodigo = _txtTenant.Text.Trim(),
                    .UsuarioOCorreo = _txtUsuarioCorreo.Text.Trim(),
                    .IdTipoVerificacionRestablecimiento = GetTipoVerificacion(),
                    .SolicitudId = Guid.NewGuid().ToString("N")
                }

                Dim response = Await _apiClient.PostAsync(Of IniciarRestablecimientoClaveRequestDto, IniciarRestablecimientoClaveResponseDto)(
                    "api/v1/seguridad/flujo_restablecimiento_clave/iniciar",
                    request).ConfigureAwait(True)

                If response Is Nothing OrElse Not response.Iniciado Then
                    Dim msg = If(response?.Mensaje, "No fue posible iniciar el flujo de recuperacion.")
                    XtraMessageBox.Show(Me, msg, "Recuperacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                If response.IdFlujoRestablecimientoClave.HasValue Then
                    _idFlujoRestablecimiento = response.IdFlujoRestablecimientoClave.Value
                End If

                _txtToken.Text = response.TokenRestablecimientoPrueba
                _lblEstado.Text = response.Mensaje
                _lblEstado.Appearance.ForeColor = Color.ForestGreen
                ToggleCompletar(_idFlujoRestablecimiento <> Guid.Empty)

                XtraMessageBox.Show(Me, response.Mensaje, "Recuperacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch apiEx As ApiClientException
                Dim detalle = If(String.IsNullOrWhiteSpace(apiEx.ResponseBody), apiEx.Message, apiEx.ResponseBody)
                XtraMessageBox.Show(Me, detalle, "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                ToggleUi(True)
            End Try
        End Sub

        Private Async Sub OnCompletarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            If _idFlujoRestablecimiento = Guid.Empty Then
                XtraMessageBox.Show(Me, "Primero debe iniciar el flujo de recuperacion.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If String.IsNullOrWhiteSpace(_txtToken.Text) Then
                XtraMessageBox.Show(Me, "Ingrese el token de recuperacion.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtToken.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(_txtNuevaClave.Text) Then
                XtraMessageBox.Show(Me, "Ingrese la nueva contrasena.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtNuevaClave.Focus()
                Return
            End If

            If _txtNuevaClave.Text <> _txtConfirmarClave.Text Then
                XtraMessageBox.Show(Me, "La confirmacion de contrasena no coincide.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtConfirmarClave.Focus()
                Return
            End If

            ToggleUi(False)

            Try
                Dim request As New CompletarRestablecimientoClaveRequestDto With {
                    .IdFlujoRestablecimientoClave = _idFlujoRestablecimiento,
                    .TokenRestablecimiento = _txtToken.Text.Trim(),
                    .NuevaContrasena = _txtNuevaClave.Text,
                    .AgenteUsuario = "Secure.WinForms"
                }

                Dim response = Await _apiClient.PostAsync(Of CompletarRestablecimientoClaveRequestDto, CompletarRestablecimientoClaveResponseDto)(
                    "api/v1/seguridad/flujo_restablecimiento_clave/completar",
                    request).ConfigureAwait(True)

                If response Is Nothing OrElse Not response.Restablecido Then
                    Dim msg = If(response?.Mensaje, "No fue posible completar la recuperacion.")
                    _lblEstado.Text = msg
                    _lblEstado.Appearance.ForeColor = Color.Firebrick
                    XtraMessageBox.Show(Me, msg, "Recuperacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                _lblEstado.Text = response.Mensaje
                _lblEstado.Appearance.ForeColor = Color.ForestGreen
                XtraMessageBox.Show(Me, response.Mensaje, "Recuperacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
                DialogResult = DialogResult.OK
                Close()
            Catch apiEx As ApiClientException
                Dim detalle = If(String.IsNullOrWhiteSpace(apiEx.ResponseBody), apiEx.Message, apiEx.ResponseBody)
                XtraMessageBox.Show(Me, detalle, "Error API", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                XtraMessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                ToggleUi(True)
            End Try
        End Sub

        Private Sub ToggleUi(ByVal enabled As Boolean)
            _btnEnviar.Enabled = enabled
            _btnCompletar.Enabled = enabled
            _btnCerrar.Enabled = enabled
            Cursor = If(enabled, Cursors.Default, Cursors.WaitCursor)
        End Sub

        Private Sub ToggleCompletar(ByVal enabled As Boolean)
            _grpCompletar.Enabled = enabled
            _btnCompletar.Enabled = enabled
        End Sub

        Private Function GetTipoVerificacion() As Short
            Select Case _cmbMetodo.SelectedIndex
                Case 0
                    Return 1
                Case 1
                    Return 1
                Case Else
                    Return 1
            End Select
        End Function

        Private Sub OnCerrarClick(ByVal sender As Object, ByVal e As EventArgs)
            DialogResult = DialogResult.Cancel
            Close()
        End Sub
    End Class
End Namespace
