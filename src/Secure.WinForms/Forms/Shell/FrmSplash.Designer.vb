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
        Me._brandPanel = New DevExpress.XtraEditors.PanelControl()
        Me._lblMarca = New DevExpress.XtraEditors.LabelControl()
        Me._lblTagline = New DevExpress.XtraEditors.LabelControl()
        Me._contentPanel = New DevExpress.XtraEditors.PanelControl()
        Me._lblTitulo = New DevExpress.XtraEditors.LabelControl()
        Me._progress = New DevExpress.XtraEditors.MarqueeProgressBarControl()
        Me._lblStatus = New DevExpress.XtraEditors.LabelControl()
        Me._lblVersion = New DevExpress.XtraEditors.LabelControl()
        CType(Me._brandPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._brandPanel.SuspendLayout()
        CType(Me._contentPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._contentPanel.SuspendLayout()
        CType(Me._progress.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_brandPanel
        '
        Me._brandPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(16, Byte), Integer), CType(CType(67, Byte), Integer), CType(CType(112, Byte), Integer))
        Me._brandPanel.Appearance.Options.UseBackColor = True
        Me._brandPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me._brandPanel.Controls.Add(Me._lblMarca)
        Me._brandPanel.Controls.Add(Me._lblTagline)
        Me._brandPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me._brandPanel.Location = New System.Drawing.Point(0, 0)
        Me._brandPanel.Name = "_brandPanel"
        Me._brandPanel.Size = New System.Drawing.Size(280, 340)
        Me._brandPanel.TabIndex = 0
        '
        '_lblMarca
        '
        Me._lblMarca.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 21.0!, System.Drawing.FontStyle.Bold)
        Me._lblMarca.Appearance.ForeColor = System.Drawing.Color.White
        Me._lblMarca.Appearance.Options.UseFont = True
        Me._lblMarca.Appearance.Options.UseForeColor = True
        Me._lblMarca.Location = New System.Drawing.Point(28, 112)
        Me._lblMarca.Name = "_lblMarca"
        Me._lblMarca.Size = New System.Drawing.Size(246, 38)
        Me._lblMarca.TabIndex = 0
        Me._lblMarca.Text = "Secure Platform ERP"
        '
        '_lblTagline
        '
        Me._lblTagline.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me._lblTagline.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(228, Byte), Integer), CType(CType(247, Byte), Integer))
        Me._lblTagline.Appearance.Options.UseFont = True
        Me._lblTagline.Appearance.Options.UseForeColor = True
        Me._lblTagline.Location = New System.Drawing.Point(31, 154)
        Me._lblTagline.Name = "_lblTagline"
        Me._lblTagline.Size = New System.Drawing.Size(149, 17)
        Me._lblTagline.TabIndex = 1
        Me._lblTagline.Text = "Enterprise IAM + ERP"
        '
        '_contentPanel
        '
        Me._contentPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me._contentPanel.Controls.Add(Me._lblTitulo)
        Me._contentPanel.Controls.Add(Me._progress)
        Me._contentPanel.Controls.Add(Me._lblStatus)
        Me._contentPanel.Controls.Add(Me._lblVersion)
        Me._contentPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._contentPanel.Location = New System.Drawing.Point(280, 0)
        Me._contentPanel.Name = "_contentPanel"
        Me._contentPanel.Size = New System.Drawing.Size(480, 340)
        Me._contentPanel.TabIndex = 1
        '
        '_lblTitulo
        '
        Me._lblTitulo.Appearance.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me._lblTitulo.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(38, Byte), Integer))
        Me._lblTitulo.Appearance.Options.UseFont = True
        Me._lblTitulo.Appearance.Options.UseForeColor = True
        Me._lblTitulo.Location = New System.Drawing.Point(34, 52)
        Me._lblTitulo.Name = "_lblTitulo"
        Me._lblTitulo.Size = New System.Drawing.Size(232, 30)
        Me._lblTitulo.TabIndex = 0
        Me._lblTitulo.Text = "Iniciando plataforma..."
        '
        '_progress
        '
        Me._progress.EditValue = 0
        Me._progress.Location = New System.Drawing.Point(36, 122)
        Me._progress.Name = "_progress"
        Me._progress.Properties.MarqueeAnimationSpeed = 26
        Me._progress.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid
        Me._progress.Size = New System.Drawing.Size(390, 14)
        Me._progress.TabIndex = 1
        '
        '_lblStatus
        '
        Me._lblStatus.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me._lblStatus.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me._lblStatus.Appearance.Options.UseFont = True
        Me._lblStatus.Appearance.Options.UseForeColor = True
        Me._lblStatus.Location = New System.Drawing.Point(36, 146)
        Me._lblStatus.Name = "_lblStatus"
        Me._lblStatus.Size = New System.Drawing.Size(79, 17)
        Me._lblStatus.TabIndex = 2
        Me._lblStatus.Text = "Inicializando..."
        '
        '_lblVersion
        '
        Me._lblVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me._lblVersion.Appearance.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me._lblVersion.Appearance.ForeColor = System.Drawing.Color.Gray
        Me._lblVersion.Appearance.Options.UseFont = True
        Me._lblVersion.Appearance.Options.UseForeColor = True
        Me._lblVersion.Location = New System.Drawing.Point(36, 288)
        Me._lblVersion.Name = "_lblVersion"
        Me._lblVersion.Size = New System.Drawing.Size(141, 15)
        Me._lblVersion.TabIndex = 3
        Me._lblVersion.Text = "Version cliente: -- | Tema: --"
        '
        'FrmSplash
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(760, 340)
        Me.Controls.Add(Me._contentPanel)
        Me.Controls.Add(Me._brandPanel)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmSplash"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Secure Platform ERP"
        Me.TopMost = True
        CType(Me._brandPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me._brandPanel.ResumeLayout(False)
        Me._brandPanel.PerformLayout()
        CType(Me._contentPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me._contentPanel.ResumeLayout(False)
        Me._contentPanel.PerformLayout()
        CType(Me._progress.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
