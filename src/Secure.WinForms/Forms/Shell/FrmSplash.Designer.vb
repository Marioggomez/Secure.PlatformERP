Namespace Forms.Shell
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FrmSplash
        Inherits DevExpress.XtraEditors.XtraForm

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            _brandPanel = New DevExpress.XtraEditors.PanelControl()
            _lblMarca = New DevExpress.XtraEditors.LabelControl()
            _lblTagline = New DevExpress.XtraEditors.LabelControl()
            _lblVersion = New DevExpress.XtraEditors.LabelControl()
            _contentPanel = New DevExpress.XtraEditors.PanelControl()
            _lblTitulo = New DevExpress.XtraEditors.LabelControl()
            _progress = New DevExpress.XtraEditors.MarqueeProgressBarControl()
            _lblStatus = New DevExpress.XtraEditors.LabelControl()
            CType(_brandPanel, ComponentModel.ISupportInitialize).BeginInit()
            _brandPanel.SuspendLayout()
            CType(_contentPanel, ComponentModel.ISupportInitialize).BeginInit()
            _contentPanel.SuspendLayout()
            CType(_progress.Properties, ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' _brandPanel
            ' 
            _brandPanel.Appearance.BackColor = Color.FromArgb(CByte(16), CByte(67), CByte(112))
            _brandPanel.Appearance.Options.UseBackColor = True
            _brandPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            _brandPanel.Controls.Add(_lblMarca)
            _brandPanel.Controls.Add(_lblTagline)
            _brandPanel.Controls.Add(_lblVersion)
            _brandPanel.Dock = DockStyle.Left
            _brandPanel.Location = New Point(0, 0)
            _brandPanel.Name = "_brandPanel"
            _brandPanel.Size = New Size(280, 340)
            _brandPanel.TabIndex = 0
            ' 
            ' _lblMarca
            ' 
            _lblMarca.Appearance.Font = New Font("Segoe UI Semibold", 21F, FontStyle.Bold)
            _lblMarca.Appearance.ForeColor = Color.White
            _lblMarca.Appearance.Options.UseFont = True
            _lblMarca.Appearance.Options.UseForeColor = True
            _lblMarca.Location = New Point(12, 98)
            _lblMarca.Name = "_lblMarca"
            _lblMarca.Size = New Size(258, 38)
            _lblMarca.TabIndex = 0
            _lblMarca.Text = "Secure Platform ERP"
            ' 
            ' _lblTagline
            ' 
            _lblTagline.Appearance.Font = New Font("Segoe UI", 10F)
            _lblTagline.Appearance.ForeColor = Color.FromArgb(CByte(205), CByte(228), CByte(247))
            _lblTagline.Appearance.Options.UseFont = True
            _lblTagline.Appearance.Options.UseForeColor = True
            _lblTagline.Location = New Point(23, 146)
            _lblTagline.Name = "_lblTagline"
            _lblTagline.Size = New Size(125, 17)
            _lblTagline.TabIndex = 1
            _lblTagline.Text = "Enterprise IAM + ERP"
            ' 
            ' _lblVersion
            ' 
            _lblVersion.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
            _lblVersion.Appearance.Font = New Font("Segoe UI", 8.5F)
            _lblVersion.Appearance.ForeColor = Color.Gray
            _lblVersion.Appearance.Options.UseFont = True
            _lblVersion.Appearance.Options.UseForeColor = True
            _lblVersion.Location = New Point(9, 324)
            _lblVersion.Name = "_lblVersion"
            _lblVersion.Size = New Size(139, 13)
            _lblVersion.TabIndex = 3
            _lblVersion.Text = "Version cliente: -- | Tema: --"
            ' 
            ' _contentPanel
            ' 
            _contentPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            _contentPanel.Controls.Add(_lblTitulo)
            _contentPanel.Controls.Add(_progress)
            _contentPanel.Controls.Add(_lblStatus)
            _contentPanel.Dock = DockStyle.Fill
            _contentPanel.Location = New Point(280, 0)
            _contentPanel.Name = "_contentPanel"
            _contentPanel.Size = New Size(480, 340)
            _contentPanel.TabIndex = 1
            ' 
            ' _lblTitulo
            ' 
            _lblTitulo.Appearance.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
            _lblTitulo.Appearance.ForeColor = Color.FromArgb(CByte(38), CByte(38), CByte(38))
            _lblTitulo.Appearance.Options.UseFont = True
            _lblTitulo.Appearance.Options.UseForeColor = True
            _lblTitulo.Location = New Point(6, 12)
            _lblTitulo.Name = "_lblTitulo"
            _lblTitulo.Size = New Size(224, 30)
            _lblTitulo.TabIndex = 0
            _lblTitulo.Text = "Iniciando plataforma..."
            ' 
            ' _progress
            ' 
            _progress.EditValue = 0
            _progress.Location = New Point(6, 324)
            _progress.Name = "_progress"
            _progress.Properties.MarqueeAnimationSpeed = 26
            _progress.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid
            _progress.Size = New Size(468, 9)
            _progress.TabIndex = 1
            ' 
            ' _lblStatus
            ' 
            _lblStatus.Appearance.Font = New Font("Segoe UI", 9.5F)
            _lblStatus.Appearance.ForeColor = Color.FromArgb(CByte(90), CByte(90), CByte(90))
            _lblStatus.Appearance.Options.UseFont = True
            _lblStatus.Appearance.Options.UseForeColor = True
            _lblStatus.Location = New Point(6, 304)
            _lblStatus.Name = "_lblStatus"
            _lblStatus.Size = New Size(80, 17)
            _lblStatus.TabIndex = 2
            _lblStatus.Text = "Inicializando..."
            ' 
            ' FrmSplash
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(760, 340)
            Controls.Add(_contentPanel)
            Controls.Add(_brandPanel)
            DoubleBuffered = True
            FormBorderStyle = FormBorderStyle.None
            MaximizeBox = False
            MinimizeBox = False
            Name = "FrmSplash"
            ShowInTaskbar = False
            StartPosition = FormStartPosition.CenterScreen
            Text = "Secure Platform ERP"
            TopMost = True
            CType(_brandPanel, ComponentModel.ISupportInitialize).EndInit()
            _brandPanel.ResumeLayout(False)
            _brandPanel.PerformLayout()
            CType(_contentPanel, ComponentModel.ISupportInitialize).EndInit()
            _contentPanel.ResumeLayout(False)
            _contentPanel.PerformLayout()
            CType(_progress.Properties, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)

        End Sub

        Friend WithEvents _brandPanel As DevExpress.XtraEditors.PanelControl
        Friend WithEvents _contentPanel As DevExpress.XtraEditors.PanelControl
        Friend WithEvents _lblMarca As DevExpress.XtraEditors.LabelControl
        Friend WithEvents _lblTagline As DevExpress.XtraEditors.LabelControl
        Friend WithEvents _lblTitulo As DevExpress.XtraEditors.LabelControl
        Friend WithEvents _lblStatus As DevExpress.XtraEditors.LabelControl
        Friend WithEvents _progress As DevExpress.XtraEditors.MarqueeProgressBarControl
        Friend WithEvents _lblVersion As DevExpress.XtraEditors.LabelControl
    End Class
End Namespace
