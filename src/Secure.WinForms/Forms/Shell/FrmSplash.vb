Imports Secure.Platform.WinForms.Infrastructure
Imports System.ComponentModel

Namespace Forms.Shell
    ''' <summary>
    ''' Splash de inicio elegante para la aplicacion ERP.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Partial Class FrmSplash
        Public Sub New()
            InitializeComponent()
            ApplyRuntimeValues()
        End Sub

        ''' <summary>
        ''' Actualiza el mensaje de estado mostrado en el splash.
        ''' </summary>
        Public Sub UpdateStatus(ByVal text As String)
            _lblStatus.Text = text
            _lblStatus.Refresh()
            Application.DoEvents()
        End Sub

        Private Sub ApplyRuntimeValues()
            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If

            _lblVersion.Text = "Version cliente: " & Application.ProductVersion & " | Tema: " & ThemeService.GetCurrentTheme()
        End Sub
    End Class
End Namespace
