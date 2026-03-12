Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout

Namespace Forms.Auth
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class FrmLogin
        Inherits XtraForm

        Private _panelBrand As PanelControl
        Private _layout As LayoutControl
        Private _txtTenant As TextEdit
        Private _txtUsuario As TextEdit
        Private _txtContrasena As TextEdit
        Private _chkRecordar As CheckEdit
        Private _btnIngresar As SimpleButton
        Private _btnCancelar As SimpleButton
        Private _lnkRecuperar As HyperlinkLabelControl
        Private _lblApiBase As LabelControl

        Private Sub InitializeComponent()
            components = New ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmLogin))
            _panelBrand = New PanelControl()
            lblMarca = New LabelControl()
            lblSubtitulo = New LabelControl()
            lblEntorno = New LabelControl()
            _layout = New LayoutControl()
            _txtTenant = New TextEdit()
            _txtUsuario = New TextEdit()
            _txtContrasena = New TextEdit()
            _chkRecordar = New CheckEdit()
            _btnIngresar = New SimpleButton()
            _btnCancelar = New SimpleButton()
            _lnkRecuperar = New HyperlinkLabelControl()
            _lblApiBase = New LabelControl()
            root = New LayoutControlGroup()
            LayoutControlItem3 = New LayoutControlItem()
            LayoutControlItem4 = New LayoutControlItem()
            LayoutControlItem5 = New LayoutControlItem()
            LayoutControlItem6 = New LayoutControlItem()
            LayoutControlItem7 = New LayoutControlItem()
            LayoutControlItem8 = New LayoutControlItem()
            LayoutControlItem9 = New LayoutControlItem()
            LayoutControlItem10 = New LayoutControlItem()
            PictureEdit1 = New PictureEdit()
            LayoutControlItem1 = New LayoutControlItem()
            BehaviorManager1 = New DevExpress.Utils.Behaviors.BehaviorManager(components)
            CType(_panelBrand, ComponentModel.ISupportInitialize).BeginInit()
            _panelBrand.SuspendLayout()
            CType(_layout, ComponentModel.ISupportInitialize).BeginInit()
            _layout.SuspendLayout()
            CType(_txtTenant.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuario.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtContrasena.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkRecordar.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(root, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem3, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem4, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem5, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem6, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem7, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem8, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem9, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem10, ComponentModel.ISupportInitialize).BeginInit()
            CType(PictureEdit1.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).BeginInit()
            CType(BehaviorManager1, ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' _panelBrand
            ' 
            _panelBrand.Appearance.BackColor = Color.FromArgb(CByte(12), CByte(58), CByte(98))
            _panelBrand.Appearance.Options.UseBackColor = True
            _panelBrand.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            _panelBrand.Controls.Add(lblMarca)
            _panelBrand.Controls.Add(lblSubtitulo)
            _panelBrand.Controls.Add(lblEntorno)
            _panelBrand.Dock = DockStyle.Left
            _panelBrand.Location = New Point(0, 0)
            _panelBrand.Name = "_panelBrand"
            _panelBrand.Size = New Size(340, 357)
            _panelBrand.TabIndex = 1
            ' 
            ' lblMarca
            ' 
            lblMarca.Appearance.Font = New Font("Segoe UI Semibold", 24F, FontStyle.Bold)
            lblMarca.Appearance.ForeColor = Color.White
            lblMarca.Appearance.Options.UseFont = True
            lblMarca.Appearance.Options.UseForeColor = True
            lblMarca.Location = New Point(30, 150)
            lblMarca.Name = "lblMarca"
            lblMarca.Size = New Size(0, 45)
            lblMarca.TabIndex = 0
            ' 
            ' lblSubtitulo
            ' 
            lblSubtitulo.Appearance.Font = New Font("Segoe UI", 11F)
            lblSubtitulo.Appearance.ForeColor = Color.FromArgb(CByte(214), CByte(230), CByte(245))
            lblSubtitulo.Appearance.Options.UseFont = True
            lblSubtitulo.Appearance.Options.UseForeColor = True
            lblSubtitulo.Location = New Point(33, 198)
            lblSubtitulo.Name = "lblSubtitulo"
            lblSubtitulo.Size = New Size(0, 20)
            lblSubtitulo.TabIndex = 1
            ' 
            ' lblEntorno
            ' 
            lblEntorno.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Italic)
            lblEntorno.Appearance.ForeColor = Color.FromArgb(CByte(190), CByte(218), CByte(240))
            lblEntorno.Appearance.Options.UseFont = True
            lblEntorno.Appearance.Options.UseForeColor = True
            lblEntorno.Location = New Point(34, 468)
            lblEntorno.Name = "lblEntorno"
            lblEntorno.Size = New Size(0, 15)
            lblEntorno.TabIndex = 2
            ' 
            ' _layout
            ' 
            _layout.Controls.Add(PictureEdit1)
            _layout.Controls.Add(_txtTenant)
            _layout.Controls.Add(_txtUsuario)
            _layout.Controls.Add(_txtContrasena)
            _layout.Controls.Add(_chkRecordar)
            _layout.Controls.Add(_btnIngresar)
            _layout.Controls.Add(_btnCancelar)
            _layout.Controls.Add(_lnkRecuperar)
            _layout.Controls.Add(_lblApiBase)
            _layout.Dock = DockStyle.Fill
            _layout.Location = New Point(340, 0)
            _layout.Name = "_layout"
            _layout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New Rectangle(-776, 134, 650, 400)
            _layout.OptionsView.UseDefaultDragAndDropRendering = False
            _layout.Root = root
            _layout.Size = New Size(320, 357)
            _layout.TabIndex = 0
            ' 
            ' _txtTenant
            ' 
            _txtTenant.EditValue = "SEED"
            _txtTenant.Location = New Point(80, 172)
            _txtTenant.Name = "_txtTenant"
            _txtTenant.Properties.NullValuePrompt = "Codigo de tenant"
            _txtTenant.Size = New Size(228, 20)
            _txtTenant.StyleController = _layout
            _txtTenant.TabIndex = 0
            ' 
            ' _txtUsuario
            ' 
            _txtUsuario.EditValue = "admin.seed"
            _txtUsuario.Location = New Point(80, 196)
            _txtUsuario.Name = "_txtUsuario"
            _txtUsuario.Properties.NullValuePrompt = "Usuario o correo"
            _txtUsuario.Size = New Size(228, 20)
            _txtUsuario.StyleController = _layout
            _txtUsuario.TabIndex = 2
            ' 
            ' _txtContrasena
            ' 
            _txtContrasena.Location = New Point(80, 220)
            _txtContrasena.Name = "_txtContrasena"
            _txtContrasena.Properties.NullValuePrompt = "Contrasena"
            _txtContrasena.Properties.UseSystemPasswordChar = True
            _txtContrasena.Size = New Size(228, 20)
            _txtContrasena.StyleController = _layout
            _txtContrasena.TabIndex = 3
            ' 
            ' _chkRecordar
            ' 
            _chkRecordar.EditValue = True
            _chkRecordar.Location = New Point(12, 244)
            _chkRecordar.Name = "_chkRecordar"
            _chkRecordar.Properties.Caption = "Recordar sesion en este equipo"
            _chkRecordar.Size = New Size(296, 20)
            _chkRecordar.StyleController = _layout
            _chkRecordar.TabIndex = 4
            ' 
            ' _btnIngresar
            ' 
            _btnIngresar.ImageOptions.SvgImage = CType(resources.GetObject("_btnIngresar.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
            _btnIngresar.Location = New Point(12, 268)
            _btnIngresar.Name = "_btnIngresar"
            _btnIngresar.Size = New Size(146, 36)
            _btnIngresar.StyleController = _layout
            _btnIngresar.TabIndex = 5
            _btnIngresar.Text = "Ingresar"
            ' 
            ' _btnCancelar
            ' 
            _btnCancelar.ImageOptions.SvgImage = CType(resources.GetObject("_btnCancelar.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
            _btnCancelar.Location = New Point(162, 268)
            _btnCancelar.Name = "_btnCancelar"
            _btnCancelar.Size = New Size(146, 36)
            _btnCancelar.StyleController = _layout
            _btnCancelar.TabIndex = 6
            _btnCancelar.Text = "Cancelar"
            ' 
            ' _lnkRecuperar
            ' 
            _lnkRecuperar.ImageAlignToText = ImageAlignToText.LeftTop
            _lnkRecuperar.ImageOptions.Image = CType(resources.GetObject("_lnkRecuperar.ImageOptions.Image"), Image)
            _lnkRecuperar.Location = New Point(12, 308)
            _lnkRecuperar.Name = "_lnkRecuperar"
            _lnkRecuperar.Size = New Size(128, 20)
            _lnkRecuperar.StyleController = _layout
            _lnkRecuperar.TabIndex = 1
            _lnkRecuperar.Text = "Recuperar contrasena"
            ' 
            ' _lblApiBase
            ' 
            _lblApiBase.Appearance.Font = New Font("Segoe UI", 8.5F)
            _lblApiBase.Appearance.ForeColor = Color.Gray
            _lblApiBase.Appearance.Options.UseFont = True
            _lblApiBase.Appearance.Options.UseForeColor = True
            _lblApiBase.Location = New Point(12, 332)
            _lblApiBase.Name = "_lblApiBase"
            _lblApiBase.Size = New Size(85, 13)
            _lblApiBase.StyleController = _layout
            _lblApiBase.TabIndex = 1
            _lblApiBase.Text = "API: configurada"
            ' 
            ' root
            ' 
            root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            root.GroupBordersVisible = False
            root.Items.AddRange(New BaseLayoutItem() {LayoutControlItem3, LayoutControlItem4, LayoutControlItem5, LayoutControlItem6, LayoutControlItem7, LayoutControlItem8, LayoutControlItem9, LayoutControlItem10, LayoutControlItem1})
            root.Name = "Root"
            root.Size = New Size(320, 357)
            ' 
            ' LayoutControlItem3
            ' 
            LayoutControlItem3.Control = _txtTenant
            LayoutControlItem3.Location = New Point(0, 160)
            LayoutControlItem3.Name = "LayoutControlItem3"
            LayoutControlItem3.Size = New Size(300, 24)
            LayoutControlItem3.Text = "Tenant"
            LayoutControlItem3.TextSize = New Size(56, 13)
            ' 
            ' LayoutControlItem4
            ' 
            LayoutControlItem4.Control = _txtUsuario
            LayoutControlItem4.Location = New Point(0, 184)
            LayoutControlItem4.Name = "LayoutControlItem4"
            LayoutControlItem4.Size = New Size(300, 24)
            LayoutControlItem4.Text = "Usuario"
            LayoutControlItem4.TextSize = New Size(56, 13)
            ' 
            ' LayoutControlItem5
            ' 
            LayoutControlItem5.Control = _txtContrasena
            LayoutControlItem5.Location = New Point(0, 208)
            LayoutControlItem5.Name = "LayoutControlItem5"
            LayoutControlItem5.Size = New Size(300, 24)
            LayoutControlItem5.Text = "Contrasena"
            LayoutControlItem5.TextSize = New Size(56, 13)
            ' 
            ' LayoutControlItem6
            ' 
            LayoutControlItem6.Control = _chkRecordar
            LayoutControlItem6.Location = New Point(0, 232)
            LayoutControlItem6.Name = "LayoutControlItem6"
            LayoutControlItem6.Size = New Size(300, 24)
            LayoutControlItem6.TextVisible = False
            ' 
            ' LayoutControlItem7
            ' 
            LayoutControlItem7.Control = _btnIngresar
            LayoutControlItem7.Location = New Point(0, 256)
            LayoutControlItem7.Name = "LayoutControlItem7"
            LayoutControlItem7.Size = New Size(150, 40)
            LayoutControlItem7.TextVisible = False
            ' 
            ' LayoutControlItem8
            ' 
            LayoutControlItem8.Control = _btnCancelar
            LayoutControlItem8.Location = New Point(150, 256)
            LayoutControlItem8.Name = "LayoutControlItem8"
            LayoutControlItem8.Size = New Size(150, 40)
            LayoutControlItem8.TextVisible = False
            ' 
            ' LayoutControlItem9
            ' 
            LayoutControlItem9.Control = _lnkRecuperar
            LayoutControlItem9.Location = New Point(0, 296)
            LayoutControlItem9.Name = "LayoutControlItem9"
            LayoutControlItem9.Size = New Size(300, 24)
            LayoutControlItem9.TextVisible = False
            ' 
            ' LayoutControlItem10
            ' 
            LayoutControlItem10.Control = _lblApiBase
            LayoutControlItem10.Location = New Point(0, 320)
            LayoutControlItem10.Name = "LayoutControlItem10"
            LayoutControlItem10.Size = New Size(300, 17)
            LayoutControlItem10.TextVisible = False
            ' 
            ' PictureEdit1
            ' 
            PictureEdit1.EditValue = resources.GetObject("PictureEdit1.EditValue")
            PictureEdit1.Location = New Point(12, 12)
            PictureEdit1.Name = "PictureEdit1"
            PictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto
            PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
            PictureEdit1.Size = New Size(296, 156)
            PictureEdit1.StyleController = _layout
            PictureEdit1.TabIndex = 7
            ' 
            ' LayoutControlItem1
            ' 
            LayoutControlItem1.Control = PictureEdit1
            LayoutControlItem1.Location = New Point(0, 0)
            LayoutControlItem1.Name = "LayoutControlItem1"
            LayoutControlItem1.Size = New Size(300, 160)
            LayoutControlItem1.TextVisible = False
            ' 
            ' FrmLogin
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(660, 357)
            Controls.Add(_layout)
            Controls.Add(_panelBrand)
            IconOptions.SvgImage = CType(resources.GetObject("FrmLogin.IconOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
            MaximizeBox = False
            MinimizeBox = False
            Name = "FrmLogin"
            StartPosition = FormStartPosition.CenterScreen
            Text = "Secure Platform ERP - Login"
            CType(_panelBrand, ComponentModel.ISupportInitialize).EndInit()
            _panelBrand.ResumeLayout(False)
            _panelBrand.PerformLayout()
            CType(_layout, ComponentModel.ISupportInitialize).EndInit()
            _layout.ResumeLayout(False)
            CType(_txtTenant.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(_txtUsuario.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(_txtContrasena.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(_chkRecordar.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(root, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem3, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem4, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem5, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem6, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem7, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem8, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem9, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem10, ComponentModel.ISupportInitialize).EndInit()
            CType(PictureEdit1.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).EndInit()
            CType(BehaviorManager1, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
        End Sub

        Friend WithEvents lblMarca As LabelControl
        Friend WithEvents lblSubtitulo As LabelControl
        Friend WithEvents lblEntorno As LabelControl
        Friend WithEvents root As LayoutControlGroup
        Friend WithEvents LayoutControlItem3 As LayoutControlItem
        Friend WithEvents LayoutControlItem4 As LayoutControlItem
        Friend WithEvents LayoutControlItem5 As LayoutControlItem
        Friend WithEvents LayoutControlItem6 As LayoutControlItem
        Friend WithEvents LayoutControlItem7 As LayoutControlItem
        Friend WithEvents LayoutControlItem8 As LayoutControlItem
        Friend WithEvents LayoutControlItem9 As LayoutControlItem
        Friend WithEvents LayoutControlItem10 As LayoutControlItem
        Friend WithEvents PictureEdit1 As PictureEdit
        Friend WithEvents LayoutControlItem1 As LayoutControlItem
        Friend WithEvents BehaviorManager1 As DevExpress.Utils.Behaviors.BehaviorManager
        Private components As System.ComponentModel.IContainer
    End Class
End Namespace
