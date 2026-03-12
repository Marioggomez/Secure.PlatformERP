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

            splash.Show()
            splash.UpdateStatus("Inicializando tema corporativo...")
            ThemeService.Initialize()

            splash.UpdateStatus("Cargando configuracion de API...")
            Dim apiBaseUrl = AppSettingsProvider.GetApiBaseUrl()

            splash.UpdateStatus("Preparando cliente de seguridad...")
            apiClient = New ApiClient(apiBaseUrl)

            splash.UpdateStatus("Cargando interfaz de autenticacion...")
            Application.DoEvents()

            splashWatch.Stop()
            Dim remainingMs = MinSplashVisibleMs - CInt(splashWatch.ElapsedMilliseconds)
            If remainingMs > 0 Then
                Thread.Sleep(remainingMs)
            End If

            splash.UpdateStatus("Listo.")
            Application.DoEvents()
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
End Module
