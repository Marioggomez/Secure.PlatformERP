Imports DevExpress.XtraEditors
Imports Secure.Platform.WinForms.Infrastructure
Imports System.ComponentModel

Namespace Forms.Shell
    ''' <summary>
    ''' Splash de inicio elegante para la aplicacion ERP.
    ''' Autor: Mario Gomez.
    ''' </summary>
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Public Partial Class FrmSplash
        Inherits XtraForm

        Private ReadOnly _brandPanel As PanelControl
        Private ReadOnly _contentPanel As PanelControl
        Private ReadOnly _lblMarca As LabelControl
        Private ReadOnly _lblTagline As LabelControl
        Private ReadOnly _lblTitulo As LabelControl
        Private ReadOnly _lblStatus As LabelControl
        Private ReadOnly _progress As MarqueeProgressBarControl
        Private ReadOnly _lblVersion As LabelControl

        Public Sub New()
            _brandPanel = New PanelControl()
            _contentPanel = New PanelControl()
            _lblMarca = New LabelControl()
            _lblTagline = New LabelControl()
            _lblTitulo = New LabelControl()
            _lblStatus = New LabelControl()
            _progress = New MarqueeProgressBarControl()
            _lblVersion = New LabelControl()

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

        Private Sub InitializeComponent()
            SuspendLayout()

            Text = "Secure Platform ERP"
            FormBorderStyle = FormBorderStyle.None
            StartPosition = FormStartPosition.CenterScreen
            ShowInTaskbar = False
            TopMost = True
            Width = 760
            Height = 340
            MaximizeBox = False
            MinimizeBox = False
            DoubleBuffered = True
            AutoScaleMode = AutoScaleMode.Dpi
            BackColor = Color.White

            _brandPanel.Dock = DockStyle.Left
            _brandPanel.Width = 280
            _brandPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            _brandPanel.Appearance.BackColor = Color.FromArgb(16, 67, 112)
            _brandPanel.Appearance.Options.UseBackColor = True

            _lblMarca.Text = "Secure Platform ERP"
            _lblMarca.Appearance.Font = New Font("Segoe UI Semibold", 21.0F, FontStyle.Bold)
            _lblMarca.Appearance.ForeColor = Color.White
            _lblMarca.Appearance.Options.UseFont = True
            _lblMarca.Appearance.Options.UseForeColor = True
            _lblMarca.Location = New Point(28, 112)

            _lblTagline.Text = "Enterprise IAM + ERP"
            _lblTagline.Appearance.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
            _lblTagline.Appearance.ForeColor = Color.FromArgb(205, 228, 247)
            _lblTagline.Appearance.Options.UseFont = True
            _lblTagline.Appearance.Options.UseForeColor = True
            _lblTagline.Location = New Point(31, 154)

            _brandPanel.Controls.Add(_lblMarca)
            _brandPanel.Controls.Add(_lblTagline)

            _contentPanel.Dock = DockStyle.Fill
            _contentPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            _contentPanel.Padding = New Padding(34, 36, 34, 26)

            _lblTitulo.Text = "Iniciando plataforma..."
            _lblTitulo.Appearance.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
            _lblTitulo.Appearance.ForeColor = Color.FromArgb(38, 38, 38)
            _lblTitulo.Appearance.Options.UseFont = True
            _lblTitulo.Appearance.Options.UseForeColor = True
            _lblTitulo.Location = New Point(34, 52)

            _progress.Location = New Point(36, 122)
            _progress.Width = 390
            _progress.Height = 14
            _progress.Properties.MarqueeAnimationSpeed = 26
            _progress.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid

            _lblStatus.Text = "Inicializando..."
            _lblStatus.Appearance.Font = New Font("Segoe UI", 9.5F, FontStyle.Regular)
            _lblStatus.Appearance.ForeColor = Color.FromArgb(90, 90, 90)
            _lblStatus.Appearance.Options.UseFont = True
            _lblStatus.Appearance.Options.UseForeColor = True
            _lblStatus.Location = New Point(36, 146)

            _lblVersion.Text = "Version cliente: -- | Tema: --"
            _lblVersion.Appearance.Font = New Font("Segoe UI", 8.5F)
            _lblVersion.Appearance.ForeColor = Color.Gray
            _lblVersion.Appearance.Options.UseFont = True
            _lblVersion.Appearance.Options.UseForeColor = True
            _lblVersion.Anchor = AnchorStyles.Left Or AnchorStyles.Bottom
            _lblVersion.Location = New Point(36, 288)

            _contentPanel.Controls.Add(_lblTitulo)
            _contentPanel.Controls.Add(_progress)
            _contentPanel.Controls.Add(_lblStatus)
            _contentPanel.Controls.Add(_lblVersion)

            Controls.Add(_contentPanel)
            Controls.Add(_brandPanel)

            ResumeLayout(False)
        End Sub
    End Class
End Namespace
