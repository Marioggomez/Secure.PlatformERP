Imports DevExpress.XtraEditors
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Auth
    ''' <summary>
    ''' Pantalla principal de autenticacion del ERP.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Partial Public Class FrmLogin
        Private ReadOnly _apiClient As ApiClient

        ''' <summary>
        ''' Resultado de autenticacion devuelto por la API.
        ''' </summary>
        Public Property LoginResponse As LoginResponseDto

        Public ReadOnly Property TenantCode As String
            Get
                Return _txtTenant.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property UserName As String
            Get
                Return _txtUsuario.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property Password As String
            Get
                Return _txtContrasena.Text
            End Get
        End Property

        Public ReadOnly Property RememberMe As Boolean
            Get
                Return _chkRecordar.Checked
            End Get
        End Property

        ''' <summary>
        ''' Constructor requerido por el diseñador WinForms.
        ''' </summary>
        Public Sub New()
            Me.New(CreateDefaultApiClient())
        End Sub

        Public Sub New(ByVal apiClient As ApiClient)
            _apiClient = apiClient
            InitializeComponent()
            ApplyRuntimeLabels()
            WireEvents()
        End Sub

        Private Shared Function CreateDefaultApiClient() As ApiClient
            Dim baseUrl = AppSettingsProvider.GetApiBaseUrl()
            Return New ApiClient(baseUrl)
        End Function

        Private Sub ApplyRuntimeLabels()
            If _lblApiBase IsNot Nothing Then
                _lblApiBase.Text = $"API: {_apiClient.BaseAddress}"
            End If
        End Sub

        Private Sub WireEvents()
            AddHandler _btnIngresar.Click, AddressOf OnIngresarClickAsync
            AddHandler _btnCancelar.Click, AddressOf OnCancelarClick
            AddHandler _lnkRecuperar.Click, AddressOf OnRecuperarClick
            AddHandler _txtContrasena.KeyDown, AddressOf OnContrasenaKeyDown
            AddHandler _txtUsuario.KeyDown, AddressOf OnUsuarioKeyDown
        End Sub

        Private Async Sub OnIngresarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            If Not ValidateInputs() Then
                Return
            End If

            ToggleUi(False)

            Try
                Dim request As New LoginRequestDto With {
                    .TenantCodigo = TenantCode,
                    .Usuario = UserName,
                    .Contrasena = Password,
                    .HuellaDispositivo = Environment.MachineName,
                    .SolicitudId = Guid.NewGuid().ToString("N")
                }

                Dim response = Await _apiClient.PostAsync(Of LoginRequestDto, LoginResponseDto)(
                    "api/v1/seguridad/flujo_autenticacion/iniciar",
                    request).ConfigureAwait(True)

                If response Is Nothing Then
                    XtraMessageBox.Show(Me, "La API no devolvio respuesta.", "Autenticacion", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                If Not response.Autenticado Then
                    XtraMessageBox.Show(Me, response.Mensaje, "Autenticacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                LoginResponse = response
                DialogResult = DialogResult.OK
                Close()
            Catch apiEx As ApiClientException
                ApiErrorPresenter.Show(Me, apiEx, "inicio de sesion")
            Catch ex As Exception
                ApiErrorPresenter.ShowUnexpected(Me, ex, "inicio de sesion")
            Finally
                ToggleUi(True)
            End Try
        End Sub

        Private Function ValidateInputs() As Boolean
            If String.IsNullOrWhiteSpace(TenantCode) Then
                XtraMessageBox.Show(Me, "Debe ingresar el tenant.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtTenant.Focus()
                Return False
            End If

            If String.IsNullOrWhiteSpace(UserName) Then
                XtraMessageBox.Show(Me, "Debe ingresar el usuario.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtUsuario.Focus()
                Return False
            End If

            If String.IsNullOrWhiteSpace(Password) Then
                XtraMessageBox.Show(Me, "Debe ingresar la contrasena.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtContrasena.Focus()
                Return False
            End If

            Return True
        End Function

        Private Sub ToggleUi(ByVal enabled As Boolean)
            _btnIngresar.Enabled = enabled
            _btnCancelar.Enabled = enabled
            _lnkRecuperar.Enabled = enabled
            Cursor = If(enabled, Cursors.Default, Cursors.WaitCursor)
        End Sub

        Private Sub OnCancelarClick(ByVal sender As Object, ByVal e As EventArgs)
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub OnRecuperarClick(ByVal sender As Object, ByVal e As EventArgs)
            Using frm As New FrmRecuperarContrasena(_apiClient, TenantCode, UserName)
                frm.ShowDialog(Me)
            End Using
        End Sub

        Private Sub OnContrasenaKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                OnIngresarClickAsync(sender, EventArgs.Empty)
            End If
        End Sub

        Private Sub OnUsuarioKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                _txtContrasena.Focus()
            End If
        End Sub
    End Class
End Namespace
