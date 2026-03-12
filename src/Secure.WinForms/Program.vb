Imports DevExpress.XtraEditors
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Forms.Auth
Imports Secure.Platform.WinForms.Forms.Shell
Imports Secure.Platform.WinForms.Infrastructure
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Threading

Module Program
    ''' <summary>
    ''' Punto de entrada del cliente ERP WinForms.
    ''' Autor: Mario Gomez.
    ''' </summary>
    <STAThread>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim apiClient As ApiClient = Nothing

        Using splash As New FrmSplash()
            Dim splashWatch = Stopwatch.StartNew()
            Const MinSplashVisibleMs As Integer = 10000
            Const MinSplashStepMs As Integer = 900

            splash.Show()
            RunSplashStep(
                splash,
                "Inicializando tema corporativo...",
                MinSplashStepMs,
                Sub() ThemeService.Initialize())

            Dim apiBaseUrl As String = String.Empty
            RunSplashStep(
                splash,
                "Cargando configuracion de API...",
                MinSplashStepMs,
                Sub() apiBaseUrl = AppSettingsProvider.GetApiBaseUrl())

            RunSplashStep(
                splash,
                "Preparando cliente de seguridad...",
                MinSplashStepMs,
                Sub() apiClient = New ApiClient(apiBaseUrl))

            RunSplashStep(
                splash,
                "Cargando interfaz de autenticacion...",
                MinSplashStepMs,
                Nothing)

            splashWatch.Stop()
            WaitSplashMinimumTime(splash, splashWatch.ElapsedMilliseconds, MinSplashVisibleMs)

            RunSplashStep(splash, "Listo.", 500, Nothing)
            splash.Close()
        End Using

        Dim loginResult As LoginResponseDto = Nothing
        Dim mfaResult As ValidarMfaResponseDto = Nothing

        Using login As New FrmLogin(apiClient)
            If login.ShowDialog() <> DialogResult.OK OrElse login.LoginResponse Is Nothing Then
                Return
            End If

            loginResult = login.LoginResponse

            If loginResult.RequiereMfa Then
                Using mfa As New FrmMfa(apiClient, loginResult)
                    If mfa.ShowDialog() <> DialogResult.OK OrElse mfa.MfaResponse Is Nothing Then
                        Return
                    End If

                    mfaResult = mfa.MfaResponse
                End Using
            End If

            Dim tokenSesion As String = If(loginResult.RequiereMfa, mfaResult.TokenSesion, loginResult.TokenSesion)
            If String.IsNullOrWhiteSpace(tokenSesion) Then
                XtraMessageBox.Show("No se recibio token de sesion desde la API.", "Autenticacion", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            apiClient.SetBearerToken(tokenSesion)

            Dim usuarioMostrar = If(Not String.IsNullOrWhiteSpace(loginResult.UsuarioMostrar), loginResult.UsuarioMostrar, login.UserName)
            Dim idTenant = If(loginResult.RequiereMfa, mfaResult.IdTenant, loginResult.IdTenant)
            Dim recursos = If(loginResult.RequiereMfa, mfaResult.RecursosUi, loginResult.RecursosUi)
            Dim idUsuario = If(loginResult.RequiereMfa, mfaResult.IdUsuario, loginResult.IdUsuario)
            Dim idEmpresa = If(loginResult.RequiereMfa, mfaResult.IdEmpresa, loginResult.IdEmpresa)

            Dim sessionContext As New UserSessionContext With {
                .Usuario = usuarioMostrar,
                .IdUsuario = idUsuario,
                .IdTenant = idTenant,
                .IdEmpresa = idEmpresa
            }

            Dim shell As New FrmMainShell(
                sessionContext,
                login.TenantCode,
                apiClient,
                recursos)

            Application.Run(shell)
        End Using
    End Sub

    Private Sub RunSplashStep(ByVal splash As FrmSplash, ByVal message As String, ByVal minVisibleMs As Integer, ByVal action As Action)
        If splash Is Nothing OrElse splash.IsDisposed Then
            action?.Invoke()
            Return
        End If

        Dim sw = Stopwatch.StartNew()
        splash.UpdateStatus(message)
        action?.Invoke()

        WaitSplashUi(splash, Math.Max(0, minVisibleMs - CInt(sw.ElapsedMilliseconds)))
    End Sub

    Private Sub WaitSplashUi(ByVal splash As FrmSplash, ByVal durationMs As Integer)
        If durationMs <= 0 Then Return

        Dim sw = Stopwatch.StartNew()
        Do While sw.ElapsedMilliseconds < durationMs
            If splash Is Nothing OrElse splash.IsDisposed Then Exit Do

            splash.Refresh()
            Application.DoEvents()
            Thread.Sleep(16)
        Loop
    End Sub

    Private Sub WaitSplashMinimumTime(ByVal splash As FrmSplash, ByVal elapsedMs As Long, ByVal minimumMs As Integer)
        Dim remainingMs = minimumMs - CInt(elapsedMs)
        If remainingMs <= 0 Then Return

        WaitSplashUi(splash, remainingMs)
    End Sub
End Module
