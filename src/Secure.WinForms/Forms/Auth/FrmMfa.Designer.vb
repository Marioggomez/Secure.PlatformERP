Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout

Namespace Forms.Auth
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class FrmMfa
        Inherits XtraForm

        Private _txtCodigo As TextEdit
        Private _btnValidar As SimpleButton
        Private _btnReenviar As SimpleButton
        Private _btnCancelar As SimpleButton
        Private _lblUsuario As LabelControl
        Private _lblTenant As LabelControl
        Private _lblCodigoPrueba As LabelControl

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMfa))
            _txtCodigo = New TextEdit()
            _layoutMain = New LayoutControl()
            lblTitulo = New LabelControl()
            lblDetalle = New LabelControl()
            _lblUsuario = New LabelControl()
            _lblTenant = New LabelControl()
            _lblCodigoPrueba = New LabelControl()
            _btnValidar = New SimpleButton()
            _btnReenviar = New SimpleButton()
            _btnCancelar = New SimpleButton()
            root = New LayoutControlGroup()
            LayoutControlItem1 = New LayoutControlItem()
            LayoutControlItem2 = New LayoutControlItem()
            LayoutControlItem3 = New LayoutControlItem()
            LayoutControlItem4 = New LayoutControlItem()
            LayoutControlItem5 = New LayoutControlItem()
            LayoutControlItem6 = New LayoutControlItem()
            LayoutControlItem7 = New LayoutControlItem()
            LayoutControlItem8 = New LayoutControlItem()
            LayoutControlItem9 = New LayoutControlItem()
            CType(_txtCodigo.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_layoutMain, ComponentModel.ISupportInitialize).BeginInit()
            _layoutMain.SuspendLayout()
            CType(root, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem2, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem3, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem4, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem5, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem6, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem7, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem8, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem9, ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' _txtCodigo
            ' 
            _txtCodigo.Location = New Point(12, 75)
            _txtCodigo.Name = "_txtCodigo"
            _txtCodigo.Properties.Appearance.Font = New Font("Segoe UI", 18F, FontStyle.Bold)
            _txtCodigo.Properties.Appearance.Options.UseFont = True
            _txtCodigo.Properties.Appearance.Options.UseTextOptions = True
            _txtCodigo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            _txtCodigo.Properties.MaxLength = 6
            _txtCodigo.Properties.NullValuePrompt = "Codigo MFA"
            _txtCodigo.Size = New Size(276, 38)
            _txtCodigo.StyleController = _layoutMain
            _txtCodigo.TabIndex = 9
            ' 
            ' _layoutMain
            ' 
            _layoutMain.Controls.Add(lblTitulo)
            _layoutMain.Controls.Add(lblDetalle)
            _layoutMain.Controls.Add(_lblUsuario)
            _layoutMain.Controls.Add(_lblTenant)
            _layoutMain.Controls.Add(_lblCodigoPrueba)
            _layoutMain.Controls.Add(_txtCodigo)
            _layoutMain.Controls.Add(_btnValidar)
            _layoutMain.Controls.Add(_btnReenviar)
            _layoutMain.Controls.Add(_btnCancelar)
            _layoutMain.Dock = DockStyle.Fill
            _layoutMain.HiddenItems.AddRange(New BaseLayoutItem() {LayoutControlItem2, LayoutControlItem1})
            _layoutMain.Location = New Point(0, 0)
            _layoutMain.Name = "_layoutMain"
            _layoutMain.Root = root
            _layoutMain.Size = New Size(300, 207)
            _layoutMain.TabIndex = 0
            ' 
            ' lblTitulo
            ' 
            lblTitulo.Appearance.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
            lblTitulo.Appearance.Options.UseFont = True
            lblTitulo.Location = New Point(12, 12)
            lblTitulo.Name = "lblTitulo"
            lblTitulo.Size = New Size(276, 30)
            lblTitulo.StyleController = _layoutMain
            lblTitulo.TabIndex = 4
            ' 
            ' lblDetalle
            ' 
            lblDetalle.Appearance.Font = New Font("Segoe UI", 10F)
            lblDetalle.Appearance.ForeColor = Color.DimGray
            lblDetalle.Appearance.Options.UseFont = True
            lblDetalle.Appearance.Options.UseForeColor = True
            lblDetalle.Location = New Point(12, 46)
            lblDetalle.Name = "lblDetalle"
            lblDetalle.Size = New Size(276, 17)
            lblDetalle.StyleController = _layoutMain
            lblDetalle.TabIndex = 5
            ' 
            ' _lblUsuario
            ' 
            _lblUsuario.Appearance.Font = New Font("Segoe UI", 9.5F)
            _lblUsuario.Appearance.ForeColor = Color.FromArgb(CByte(65), CByte(65), CByte(65))
            _lblUsuario.Appearance.Options.UseFont = True
            _lblUsuario.Appearance.Options.UseForeColor = True
            _lblUsuario.Location = New Point(12, 12)
            _lblUsuario.Name = "_lblUsuario"
            _lblUsuario.Size = New Size(276, 17)
            _lblUsuario.StyleController = _layoutMain
            _lblUsuario.TabIndex = 6
            ' 
            ' _lblTenant
            ' 
            _lblTenant.Appearance.Font = New Font("Segoe UI", 9.5F)
            _lblTenant.Appearance.ForeColor = Color.FromArgb(CByte(65), CByte(65), CByte(65))
            _lblTenant.Appearance.Options.UseFont = True
            _lblTenant.Appearance.Options.UseForeColor = True
            _lblTenant.Location = New Point(12, 33)
            _lblTenant.Name = "_lblTenant"
            _lblTenant.Size = New Size(276, 17)
            _lblTenant.StyleController = _layoutMain
            _lblTenant.TabIndex = 7
            ' 
            ' _lblCodigoPrueba
            ' 
            _lblCodigoPrueba.Appearance.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            _lblCodigoPrueba.Appearance.ForeColor = Color.FromArgb(CByte(196), CByte(62), CByte(22))
            _lblCodigoPrueba.Appearance.Options.UseFont = True
            _lblCodigoPrueba.Appearance.Options.UseForeColor = True
            _lblCodigoPrueba.Location = New Point(12, 54)
            _lblCodigoPrueba.Name = "_lblCodigoPrueba"
            _lblCodigoPrueba.Size = New Size(276, 17)
            _lblCodigoPrueba.StyleController = _layoutMain
            _lblCodigoPrueba.TabIndex = 8
            _lblCodigoPrueba.Visible = False
            ' 
            ' _btnValidar
            ' 
            _btnValidar.Location = New Point(12, 117)
            _btnValidar.Name = "_btnValidar"
            _btnValidar.Size = New Size(276, 22)
            _btnValidar.StyleController = _layoutMain
            _btnValidar.TabIndex = 10
            _btnValidar.Text = "Validar"
            ' 
            ' _btnReenviar
            ' 
            _btnReenviar.Location = New Point(12, 143)
            _btnReenviar.Name = "_btnReenviar"
            _btnReenviar.Size = New Size(276, 22)
            _btnReenviar.StyleController = _layoutMain
            _btnReenviar.TabIndex = 11
            _btnReenviar.Text = "Reenviar codigo"
            ' 
            ' _btnCancelar
            ' 
            _btnCancelar.Location = New Point(12, 169)
            _btnCancelar.Name = "_btnCancelar"
            _btnCancelar.Size = New Size(276, 22)
            _btnCancelar.StyleController = _layoutMain
            _btnCancelar.TabIndex = 12
            _btnCancelar.Text = "Cancelar"
            ' 
            ' root
            ' 
            root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            root.GroupBordersVisible = False
            root.Items.AddRange(New BaseLayoutItem() {LayoutControlItem3, LayoutControlItem4, LayoutControlItem5, LayoutControlItem6, LayoutControlItem7, LayoutControlItem8, LayoutControlItem9})
            root.Name = "root"
            root.Size = New Size(300, 207)
            ' 
            ' LayoutControlItem1
            ' 
            LayoutControlItem1.Control = lblTitulo
            LayoutControlItem1.Location = New Point(0, 0)
            LayoutControlItem1.Name = "LayoutControlItem1"
            LayoutControlItem1.Size = New Size(280, 34)
            LayoutControlItem1.TextVisible = False
            ' 
            ' LayoutControlItem2
            ' 
            LayoutControlItem2.Control = lblDetalle
            LayoutControlItem2.Location = New Point(0, 34)
            LayoutControlItem2.Name = "LayoutControlItem2"
            LayoutControlItem2.Size = New Size(280, 21)
            LayoutControlItem2.TextVisible = False
            ' 
            ' LayoutControlItem3
            ' 
            LayoutControlItem3.Control = _lblUsuario
            LayoutControlItem3.Location = New Point(0, 0)
            LayoutControlItem3.Name = "LayoutControlItem3"
            LayoutControlItem3.Size = New Size(280, 21)
            LayoutControlItem3.TextVisible = False
            ' 
            ' LayoutControlItem4
            ' 
            LayoutControlItem4.Control = _lblTenant
            LayoutControlItem4.Location = New Point(0, 21)
            LayoutControlItem4.Name = "LayoutControlItem4"
            LayoutControlItem4.Size = New Size(280, 21)
            LayoutControlItem4.TextVisible = False
            ' 
            ' LayoutControlItem5
            ' 
            LayoutControlItem5.Control = _lblCodigoPrueba
            LayoutControlItem5.Location = New Point(0, 42)
            LayoutControlItem5.Name = "LayoutControlItem5"
            LayoutControlItem5.Size = New Size(280, 21)
            LayoutControlItem5.TextVisible = False
            ' 
            ' LayoutControlItem6
            ' 
            LayoutControlItem6.AppearanceItemCaption.Font = New Font("Segoe UI", 18F, FontStyle.Bold)
            LayoutControlItem6.AppearanceItemCaption.Options.UseFont = True
            LayoutControlItem6.Control = _txtCodigo
            LayoutControlItem6.Location = New Point(0, 63)
            LayoutControlItem6.Name = "LayoutControlItem6"
            LayoutControlItem6.Size = New Size(280, 42)
            LayoutControlItem6.Text = "Codigo"
            LayoutControlItem6.TextVisible = False
            ' 
            ' LayoutControlItem7
            ' 
            LayoutControlItem7.Control = _btnValidar
            LayoutControlItem7.Location = New Point(0, 105)
            LayoutControlItem7.Name = "LayoutControlItem7"
            LayoutControlItem7.Size = New Size(280, 26)
            LayoutControlItem7.TextVisible = False
            ' 
            ' LayoutControlItem8
            ' 
            LayoutControlItem8.Control = _btnReenviar
            LayoutControlItem8.Location = New Point(0, 131)
            LayoutControlItem8.Name = "LayoutControlItem8"
            LayoutControlItem8.Size = New Size(280, 26)
            LayoutControlItem8.TextVisible = False
            ' 
            ' LayoutControlItem9
            ' 
            LayoutControlItem9.Control = _btnCancelar
            LayoutControlItem9.Location = New Point(0, 157)
            LayoutControlItem9.Name = "LayoutControlItem9"
            LayoutControlItem9.Size = New Size(280, 30)
            LayoutControlItem9.TextVisible = False
            ' 
            ' FrmMfa
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(300, 207)
            Controls.Add(_layoutMain)
            IconOptions.Image = CType(resources.GetObject("FrmMfa.IconOptions.Image"), Image)
            MaximizeBox = False
            MinimizeBox = False
            Name = "FrmMfa"
            StartPosition = FormStartPosition.CenterScreen
            Text = "Validación MFA"
            CType(_txtCodigo.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(_layoutMain, ComponentModel.ISupportInitialize).EndInit()
            _layoutMain.ResumeLayout(False)
            CType(root, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem2, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem3, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem4, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem5, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem6, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem7, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem8, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem9, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
        End Sub

        Friend WithEvents _layoutMain As LayoutControl
        Friend WithEvents lblTitulo As LabelControl
        Friend WithEvents lblDetalle As LabelControl
        Friend WithEvents root As LayoutControlGroup
        Friend WithEvents LayoutControlItem1 As LayoutControlItem
        Friend WithEvents LayoutControlItem2 As LayoutControlItem
        Friend WithEvents LayoutControlItem3 As LayoutControlItem
        Friend WithEvents LayoutControlItem4 As LayoutControlItem
        Friend WithEvents LayoutControlItem5 As LayoutControlItem
        Friend WithEvents LayoutControlItem6 As LayoutControlItem
        Friend WithEvents LayoutControlItem7 As LayoutControlItem
        Friend WithEvents LayoutControlItem8 As LayoutControlItem
        Friend WithEvents LayoutControlItem9 As LayoutControlItem
    End Class
End Namespace
