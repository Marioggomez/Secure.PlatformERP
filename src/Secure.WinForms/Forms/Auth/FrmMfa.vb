Imports DevExpress.XtraEditors
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Auth
    ''' <summary>
    ''' Pantalla de validacion MFA.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Partial Public Class FrmMfa
        Private ReadOnly _apiClient As ApiClient
        Private ReadOnly _loginResponse As LoginResponseDto
        Private _idDesafioActual As Guid

        ''' <summary>
        ''' Resultado de MFA devuelto por la API.
        ''' </summary>
        Public Property MfaResponse As ValidarMfaResponseDto

        Public ReadOnly Property CodigoIngresado As String
            Get
                Return _txtCodigo.Text.Trim()
            End Get
        End Property

        ''' <summary>
        ''' Constructor requerido por el diseñador WinForms.
        ''' </summary>
        Public Sub New()
            Me.New(CreateDefaultApiClient(), New LoginResponseDto With {
                .UsuarioMostrar = "Usuario diseño",
                .IdTenant = 0,
                .IdFlujoAutenticacion = Guid.Empty,
                .IdDesafioMfa = Guid.Empty,
                .CodigoMfaPrueba = "000000"
            })
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal loginResponse As LoginResponseDto)
            _apiClient = apiClient
            _loginResponse = loginResponse
            _idDesafioActual = If(loginResponse IsNot Nothing AndAlso loginResponse.IdDesafioMfa.HasValue, loginResponse.IdDesafioMfa.Value, Guid.Empty)

            InitializeComponent()
            BindInitialState()
            WireEvents()
        End Sub

        Private Shared Function CreateDefaultApiClient() As ApiClient
            Dim baseUrl = AppSettingsProvider.GetApiBaseUrl()
            Return New ApiClient(baseUrl)
        End Function

        Private Sub BindInitialState()
            Dim usuarioTexto = If(_loginResponse?.UsuarioMostrar, String.Empty)
            Dim tenantTexto = If(_loginResponse?.IdTenant, 0)

            _lblUsuario.Text = $"Usuario: {usuarioTexto}".Trim()
            _lblTenant.Text = $"Tenant: {tenantTexto}".Trim()

            If Not String.IsNullOrWhiteSpace(_loginResponse?.CodigoMfaPrueba) Then
                _lblCodigoPrueba.Text = $"Codigo de prueba (QA): {_loginResponse.CodigoMfaPrueba}"
                _lblCodigoPrueba.Visible = True
            Else
                _lblCodigoPrueba.Visible = False
            End If

            AdjustDialogToContent()
        End Sub

        Private Sub WireEvents()
            AddHandler _btnValidar.Click, AddressOf OnValidarClickAsync
            AddHandler _btnReenviar.Click, AddressOf OnReenviarClickAsync
            AddHandler _btnCancelar.Click, AddressOf OnCancelarClick
            AddHandler _txtCodigo.KeyDown, AddressOf OnCodigoKeyDown
            AddHandler Shown, AddressOf OnFormShown
            AddHandler FormClosed, AddressOf OnFormClosedCleanup
            AddHandler DevExpress.LookAndFeel.UserLookAndFeel.Default.StyleChanged, AddressOf OnGlobalStyleChanged
        End Sub

        Private Async Sub OnValidarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            If _loginResponse Is Nothing OrElse _loginResponse.IdFlujoAutenticacion Is Nothing OrElse _loginResponse.IdFlujoAutenticacion.Value = Guid.Empty Then
                XtraMessageBox.Show(Me, "No hay un flujo de autenticacion MFA activo.", "MFA", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If _idDesafioActual = Guid.Empty Then
                XtraMessageBox.Show(Me, "No hay un desafio MFA valido.", "MFA", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If CodigoIngresado.Length <> 6 OrElse Not CodigoIngresado.All(Function(ch) Char.IsDigit(ch)) Then
                XtraMessageBox.Show(Me, "El codigo MFA debe tener 6 digitos numericos.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtCodigo.Focus()
                Return
            End If

            ToggleUi(False)

            Try
                Dim request As New ValidarMfaRequestDto With {
                    .IdFlujoAutenticacion = _loginResponse.IdFlujoAutenticacion.Value,
                    .IdDesafioMfa = _idDesafioActual,
                    .CodigoOtp = CodigoIngresado,
                    .HuellaDispositivo = Environment.MachineName,
                    .SolicitudId = Guid.NewGuid().ToString("N")
                }

                Dim response = Await _apiClient.PostAsync(Of ValidarMfaRequestDto, ValidarMfaResponseDto)(
                    "api/v1/seguridad/desafio_mfa/validar",
                    request).ConfigureAwait(True)

                If response Is Nothing OrElse Not response.Validado Then
                    Dim msg = If(response?.Mensaje, "No fue posible validar el desafio MFA.")
                    XtraMessageBox.Show(Me, msg, "MFA", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                MfaResponse = response
                DialogResult = DialogResult.OK
                Close()
            Catch apiEx As ApiClientException
                ApiErrorPresenter.Show(Me, apiEx, "validacion MFA")
            Catch ex As Exception
                ApiErrorPresenter.ShowUnexpected(Me, ex, "validacion MFA")
            Finally
                ToggleUi(True)
            End Try
        End Sub

        Private Async Sub OnReenviarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            If _loginResponse Is Nothing OrElse _loginResponse.IdFlujoAutenticacion Is Nothing OrElse _loginResponse.IdFlujoAutenticacion.Value = Guid.Empty Then
                XtraMessageBox.Show(Me, "No existe flujo activo para reenviar MFA.", "MFA", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ToggleUi(False)

            Try
                Dim request As New ReenviarMfaRequestDto With {
                    .IdFlujoAutenticacion = _loginResponse.IdFlujoAutenticacion.Value,
                    .IdDesafioMfa = _idDesafioActual
                }

                Dim response = Await _apiClient.PostAsync(Of ReenviarMfaRequestDto, ReenviarMfaResponseDto)(
                    "api/v1/seguridad/desafio_mfa/reenviar",
                    request).ConfigureAwait(True)

                If response Is Nothing OrElse Not response.Reenviado OrElse Not response.IdDesafioMfa.HasValue Then
                    Dim msg = If(response?.Mensaje, "No fue posible reenviar el desafio MFA.")
                    XtraMessageBox.Show(Me, msg, "MFA", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                _idDesafioActual = response.IdDesafioMfa.Value
                _lblCodigoPrueba.Text = $"Codigo de prueba (QA): {response.CodigoMfaPrueba}"
                _lblCodigoPrueba.Visible = True
                AdjustDialogToContent()
                XtraMessageBox.Show(Me, response.Mensaje, "MFA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch apiEx As ApiClientException
                ApiErrorPresenter.Show(Me, apiEx, "reenvio MFA")
            Catch ex As Exception
                ApiErrorPresenter.ShowUnexpected(Me, ex, "reenvio MFA")
            Finally
                ToggleUi(True)
            End Try
        End Sub

        Private Sub ToggleUi(ByVal enabled As Boolean)
            _btnValidar.Enabled = enabled
            _btnReenviar.Enabled = enabled
            _btnCancelar.Enabled = enabled
            Cursor = If(enabled, Cursors.Default, Cursors.WaitCursor)
        End Sub

        Private Sub OnCancelarClick(ByVal sender As Object, ByVal e As EventArgs)
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub OnCodigoKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                OnValidarClickAsync(sender, EventArgs.Empty)
            End If
        End Sub

        Private Sub OnFormShown(ByVal sender As Object, ByVal e As EventArgs)
            AdjustDialogToContent()
        End Sub

        Private Sub OnFormClosedCleanup(ByVal sender As Object, ByVal e As FormClosedEventArgs)
            RemoveHandler DevExpress.LookAndFeel.UserLookAndFeel.Default.StyleChanged, AddressOf OnGlobalStyleChanged
        End Sub

        Private Sub OnGlobalStyleChanged(ByVal sender As Object, ByVal e As EventArgs)
            If IsDisposed Then Return

            If IsHandleCreated Then
                BeginInvoke(New MethodInvoker(AddressOf AdjustDialogToContent))
            End If
        End Sub

        Private Sub AdjustDialogToContent()
            If IsDisposed Then Return
            If _layoutMain Is Nothing OrElse _layoutMain.IsDisposed Then Return

            _layoutMain.SuspendLayout()
            Try
                _layoutMain.PerformLayout()

                Dim maxRight = 0
                Dim maxBottom = 0
                For Each ctl As Control In _layoutMain.Controls
                    If ctl Is Nothing OrElse Not ctl.Visible Then Continue For
                    maxRight = Math.Max(maxRight, ctl.Right)
                    maxBottom = Math.Max(maxBottom, ctl.Bottom)
                Next

                Const horizontalPadding As Integer = 20
                Const verticalPadding As Integer = 20

                Dim targetClientWidth = Math.Max(320, maxRight + horizontalPadding)
                Dim targetClientHeight = Math.Max(240, maxBottom + verticalPadding)

                If ClientSize.Width <> targetClientWidth OrElse ClientSize.Height <> targetClientHeight Then
                    ClientSize = New Size(targetClientWidth, targetClientHeight)
                End If

                Dim nonClientWidth = Width - ClientSize.Width
                Dim nonClientHeight = Height - ClientSize.Height
                MinimumSize = New Size(targetClientWidth + nonClientWidth, targetClientHeight + nonClientHeight)
            Finally
                _layoutMain.ResumeLayout()
            End Try
        End Sub
    End Class
End Namespace
