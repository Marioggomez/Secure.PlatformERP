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
            _panelBrand = New PanelControl()
            lblMarca = New LabelControl()
            lblSubtitulo = New LabelControl()
            lblEntorno = New LabelControl()
            _layout = New LayoutControl()
            lblTitulo = New LabelControl()
            lblDescripcion = New LabelControl()
            _txtTenant = New TextEdit()
            _txtUsuario = New TextEdit()
            _txtContrasena = New TextEdit()
            _chkRecordar = New CheckEdit()
            _btnIngresar = New SimpleButton()
            _btnCancelar = New SimpleButton()
            _lnkRecuperar = New HyperlinkLabelControl()
            _lblApiBase = New LabelControl()
            root = New LayoutControlGroup()
            spacer = New EmptySpaceItem()
            LayoutControlItem1 = New LayoutControlItem()
            LayoutControlItem2 = New LayoutControlItem()
            LayoutControlItem3 = New LayoutControlItem()
            LayoutControlItem4 = New LayoutControlItem()
            LayoutControlItem5 = New LayoutControlItem()
            LayoutControlItem6 = New LayoutControlItem()
            LayoutControlItem7 = New LayoutControlItem()
            LayoutControlItem8 = New LayoutControlItem()
            LayoutControlItem9 = New LayoutControlItem()
            LayoutControlItem10 = New LayoutControlItem()
            CType(_panelBrand, ComponentModel.ISupportInitialize).BeginInit()
            _panelBrand.SuspendLayout()
            CType(_layout, ComponentModel.ISupportInitialize).BeginInit()
            _layout.SuspendLayout()
            CType(_txtTenant.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtUsuario.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_txtContrasena.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_chkRecordar.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(root, ComponentModel.ISupportInitialize).BeginInit()
            CType(spacer, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem2, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem3, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem4, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem5, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem6, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem7, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem8, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem9, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem10, ComponentModel.ISupportInitialize).BeginInit()
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
            _panelBrand.Size = New Size(340, 488)
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
            _layout.Controls.Add(lblTitulo)
            _layout.Controls.Add(lblDescripcion)
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
            _layout.OptionsView.UseDefaultDragAndDropRendering = False
            _layout.Root = root
            _layout.Size = New Size(518, 488)
            _layout.TabIndex = 0
            ' 
            ' lblTitulo
            ' 
            lblTitulo.Appearance.Font = New Font("Segoe UI", 20F, FontStyle.Bold)
            lblTitulo.Appearance.Options.UseFont = True
            lblTitulo.Location = New Point(12, 236)
            lblTitulo.Name = "lblTitulo"
            lblTitulo.Size = New Size(494, 37)
            lblTitulo.StyleController = _layout
            lblTitulo.TabIndex = 4
            ' 
            ' lblDescripcion
            ' 
            lblDescripcion.Appearance.Font = New Font("Segoe UI", 10F)
            lblDescripcion.Appearance.ForeColor = Color.FromArgb(CByte(95), CByte(95), CByte(95))
            lblDescripcion.Appearance.Options.UseFont = True
            lblDescripcion.Appearance.Options.UseForeColor = True
            lblDescripcion.Location = New Point(12, 277)
            lblDescripcion.Name = "lblDescripcion"
            lblDescripcion.Size = New Size(494, 17)
            lblDescripcion.StyleController = _layout
            lblDescripcion.TabIndex = 5
            ' 
            ' _txtTenant
            ' 
            _txtTenant.EditValue = "SEED"
            _txtTenant.Location = New Point(80, 298)
            _txtTenant.Name = "_txtTenant"
            _txtTenant.Properties.NullValuePrompt = "Codigo de tenant"
            _txtTenant.Size = New Size(426, 20)
            _txtTenant.StyleController = _layout
            _txtTenant.TabIndex = 6
            ' 
            ' _txtUsuario
            ' 
            _txtUsuario.EditValue = "admin.seed"
            _txtUsuario.Location = New Point(80, 322)
            _txtUsuario.Name = "_txtUsuario"
            _txtUsuario.Properties.NullValuePrompt = "Usuario o correo"
            _txtUsuario.Size = New Size(426, 20)
            _txtUsuario.StyleController = _layout
            _txtUsuario.TabIndex = 7
            ' 
            ' _txtContrasena
            ' 
            _txtContrasena.Location = New Point(80, 346)
            _txtContrasena.Name = "_txtContrasena"
            _txtContrasena.Properties.NullValuePrompt = "Contrasena"
            _txtContrasena.Properties.UseSystemPasswordChar = True
            _txtContrasena.Size = New Size(426, 20)
            _txtContrasena.StyleController = _layout
            _txtContrasena.TabIndex = 8
            ' 
            ' _chkRecordar
            ' 
            _chkRecordar.EditValue = True
            _chkRecordar.Location = New Point(12, 370)
            _chkRecordar.Name = "_chkRecordar"
            _chkRecordar.Properties.Caption = "Recordar sesion en este equipo"
            _chkRecordar.Size = New Size(494, 20)
            _chkRecordar.StyleController = _layout
            _chkRecordar.TabIndex = 9
            ' 
            ' _btnIngresar
            ' 
            _btnIngresar.Location = New Point(12, 394)
            _btnIngresar.Name = "_btnIngresar"
            _btnIngresar.Size = New Size(494, 22)
            _btnIngresar.StyleController = _layout
            _btnIngresar.TabIndex = 10
            _btnIngresar.Text = "Ingresar"
            ' 
            ' _btnCancelar
            ' 
            _btnCancelar.Location = New Point(12, 420)
            _btnCancelar.Name = "_btnCancelar"
            _btnCancelar.Size = New Size(494, 22)
            _btnCancelar.StyleController = _layout
            _btnCancelar.TabIndex = 11
            _btnCancelar.Text = "Cancelar"
            ' 
            ' _lnkRecuperar
            ' 
            _lnkRecuperar.Location = New Point(12, 446)
            _lnkRecuperar.Name = "_lnkRecuperar"
            _lnkRecuperar.Size = New Size(107, 13)
            _lnkRecuperar.StyleController = _layout
            _lnkRecuperar.TabIndex = 12
            _lnkRecuperar.Text = "Recuperar contrasena"
            ' 
            ' _lblApiBase
            ' 
            _lblApiBase.Appearance.Font = New Font("Segoe UI", 8.5F)
            _lblApiBase.Appearance.ForeColor = Color.Gray
            _lblApiBase.Appearance.Options.UseFont = True
            _lblApiBase.Appearance.Options.UseForeColor = True
            _lblApiBase.Location = New Point(12, 463)
            _lblApiBase.Name = "_lblApiBase"
            _lblApiBase.Size = New Size(85, 13)
            _lblApiBase.StyleController = _layout
            _lblApiBase.TabIndex = 13
            _lblApiBase.Text = "API: configurada"
            ' 
            ' root
            ' 
            root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            root.GroupBordersVisible = False
            root.Items.AddRange(New BaseLayoutItem() {spacer, LayoutControlItem1, LayoutControlItem2, LayoutControlItem3, LayoutControlItem4, LayoutControlItem5, LayoutControlItem6, LayoutControlItem7, LayoutControlItem8, LayoutControlItem9, LayoutControlItem10})
            root.Name = "root"
            root.Size = New Size(518, 488)
            ' 
            ' spacer
            ' 
            spacer.Location = New Point(0, 0)
            spacer.Name = "spacer"
            spacer.Size = New Size(498, 224)
            ' 
            ' LayoutControlItem1
            ' 
            LayoutControlItem1.Control = lblTitulo
            LayoutControlItem1.Location = New Point(0, 224)
            LayoutControlItem1.Name = "LayoutControlItem1"
            LayoutControlItem1.Size = New Size(498, 41)
            LayoutControlItem1.TextVisible = False
            ' 
            ' LayoutControlItem2
            ' 
            LayoutControlItem2.Control = lblDescripcion
            LayoutControlItem2.Location = New Point(0, 265)
            LayoutControlItem2.Name = "LayoutControlItem2"
            LayoutControlItem2.Size = New Size(498, 21)
            LayoutControlItem2.TextVisible = False
            ' 
            ' LayoutControlItem3
            ' 
            LayoutControlItem3.Control = _txtTenant
            LayoutControlItem3.Location = New Point(0, 286)
            LayoutControlItem3.Name = "LayoutControlItem3"
            LayoutControlItem3.Size = New Size(498, 24)
            LayoutControlItem3.Text = "Tenant"
            LayoutControlItem3.TextSize = New Size(56, 13)
            ' 
            ' LayoutControlItem4
            ' 
            LayoutControlItem4.Control = _txtUsuario
            LayoutControlItem4.Location = New Point(0, 310)
            LayoutControlItem4.Name = "LayoutControlItem4"
            LayoutControlItem4.Size = New Size(498, 24)
            LayoutControlItem4.Text = "Usuario"
            LayoutControlItem4.TextSize = New Size(56, 13)
            ' 
            ' LayoutControlItem5
            ' 
            LayoutControlItem5.Control = _txtContrasena
            LayoutControlItem5.Location = New Point(0, 334)
            LayoutControlItem5.Name = "LayoutControlItem5"
            LayoutControlItem5.Size = New Size(498, 24)
            LayoutControlItem5.Text = "Contrasena"
            LayoutControlItem5.TextSize = New Size(56, 13)
            ' 
            ' LayoutControlItem6
            ' 
            LayoutControlItem6.Control = _chkRecordar
            LayoutControlItem6.Location = New Point(0, 358)
            LayoutControlItem6.Name = "LayoutControlItem6"
            LayoutControlItem6.Size = New Size(498, 24)
            LayoutControlItem6.TextVisible = False
            ' 
            ' LayoutControlItem7
            ' 
            LayoutControlItem7.Control = _btnIngresar
            LayoutControlItem7.Location = New Point(0, 382)
            LayoutControlItem7.Name = "LayoutControlItem7"
            LayoutControlItem7.Size = New Size(498, 26)
            LayoutControlItem7.TextVisible = False
            ' 
            ' LayoutControlItem8
            ' 
            LayoutControlItem8.Control = _btnCancelar
            LayoutControlItem8.Location = New Point(0, 408)
            LayoutControlItem8.Name = "LayoutControlItem8"
            LayoutControlItem8.Size = New Size(498, 26)
            LayoutControlItem8.TextVisible = False
            ' 
            ' LayoutControlItem9
            ' 
            LayoutControlItem9.Control = _lnkRecuperar
            LayoutControlItem9.Location = New Point(0, 434)
            LayoutControlItem9.Name = "LayoutControlItem9"
            LayoutControlItem9.Size = New Size(498, 17)
            LayoutControlItem9.TextVisible = False
            ' 
            ' LayoutControlItem10
            ' 
            LayoutControlItem10.Control = _lblApiBase
            LayoutControlItem10.Location = New Point(0, 451)
            LayoutControlItem10.Name = "LayoutControlItem10"
            LayoutControlItem10.Size = New Size(498, 17)
            LayoutControlItem10.TextVisible = False
            ' 
            ' FrmLogin
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(858, 488)
            Controls.Add(_layout)
            Controls.Add(_panelBrand)
            MaximizeBox = False
            MinimizeBox = False
            MinimumSize = New Size(860, 520)
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
            CType(spacer, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem2, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem3, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem4, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem5, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem6, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem7, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem8, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem9, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem10, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
        End Sub

        Friend WithEvents lblMarca As LabelControl
        Friend WithEvents lblSubtitulo As LabelControl
        Friend WithEvents lblEntorno As LabelControl
        Friend WithEvents lblTitulo As LabelControl
        Friend WithEvents lblDescripcion As LabelControl
        Friend WithEvents root As LayoutControlGroup
        Friend WithEvents spacer As EmptySpaceItem
        Friend WithEvents LayoutControlItem1 As LayoutControlItem
        Friend WithEvents LayoutControlItem2 As LayoutControlItem
        Friend WithEvents LayoutControlItem3 As LayoutControlItem
        Friend WithEvents LayoutControlItem4 As LayoutControlItem
        Friend WithEvents LayoutControlItem5 As LayoutControlItem
        Friend WithEvents LayoutControlItem6 As LayoutControlItem
        Friend WithEvents LayoutControlItem7 As LayoutControlItem
        Friend WithEvents LayoutControlItem8 As LayoutControlItem
        Friend WithEvents LayoutControlItem9 As LayoutControlItem
        Friend WithEvents LayoutControlItem10 As LayoutControlItem
    End Class
End Namespace
