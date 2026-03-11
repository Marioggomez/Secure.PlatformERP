Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout

Namespace Forms.Auth
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class FrmRecuperarContrasena
        Private _txtTenant As TextEdit
        Private _txtUsuarioCorreo As TextEdit
        Private _cmbMetodo As ComboBoxEdit
        Private _btnEnviar As SimpleButton
        Private _btnCompletar As SimpleButton
        Private _btnCerrar As SimpleButton
        Private _txtToken As TextEdit
        Private _txtNuevaClave As TextEdit
        Private _txtConfirmarClave As TextEdit
        Private _lblEstado As LabelControl
        Private _grpCompletar As GroupControl

        Private Sub InitializeComponent()
            _txtTenant = New TextEdit()
            _txtUsuarioCorreo = New TextEdit()
            _cmbMetodo = New ComboBoxEdit()
            _btnEnviar = New SimpleButton()
            _btnCompletar = New SimpleButton()
            _btnCerrar = New SimpleButton()
            _txtToken = New TextEdit()
            _txtNuevaClave = New TextEdit()
            _txtConfirmarClave = New TextEdit()
            _lblEstado = New LabelControl()
            _grpCompletar = New GroupControl()

            SuspendLayout()

            Text = "Recuperar contrasena"
            StartPosition = FormStartPosition.CenterParent
            FormBorderStyle = FormBorderStyle.Sizable
            MaximizeBox = False
            MinimizeBox = False
            Width = 760
            Height = 520
            MinimumSize = New Size(680, 480)
            AutoScaleMode = AutoScaleMode.Dpi

            Dim layout As New LayoutControl() With {
                .Dock = DockStyle.Fill
            }
            Controls.Add(layout)

            Dim lblTitulo As New LabelControl() With {
                .Text = "Recuperacion de acceso"
            }
            lblTitulo.Appearance.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)

            Dim lblAyuda As New LabelControl() With {
                .Text = "Inicie el flujo y luego confirme con token para definir una nueva contrasena."
            }
            lblAyuda.Appearance.Font = New Font("Segoe UI", 9.5F)
            lblAyuda.Appearance.ForeColor = Color.DimGray

            _txtTenant.Properties.NullValuePrompt = "Tenant"
            _txtTenant.Properties.NullValuePromptShowForEmptyValue = True

            _txtUsuarioCorreo.Properties.NullValuePrompt = "Usuario o correo"
            _txtUsuarioCorreo.Properties.NullValuePromptShowForEmptyValue = True

            _cmbMetodo.Properties.Items.AddRange(New Object() {
                "Correo corporativo",
                "SMS corporativo",
                "Mesa de ayuda"
            })
            _cmbMetodo.SelectedIndex = 0

            _btnEnviar.Text = "Iniciar flujo"
            _btnEnviar.Height = 38

            _grpCompletar.Text = "Completar restablecimiento"

            Dim layoutCompletar As New LayoutControl() With {
                .Dock = DockStyle.Fill
            }
            _grpCompletar.Controls.Add(layoutCompletar)

            _txtToken.Properties.NullValuePrompt = "Token recibido"
            _txtToken.Properties.NullValuePromptShowForEmptyValue = True

            _txtNuevaClave.Properties.UseSystemPasswordChar = True
            _txtNuevaClave.Properties.NullValuePrompt = "Nueva contrasena"
            _txtNuevaClave.Properties.NullValuePromptShowForEmptyValue = True

            _txtConfirmarClave.Properties.UseSystemPasswordChar = True
            _txtConfirmarClave.Properties.NullValuePrompt = "Confirmar contrasena"
            _txtConfirmarClave.Properties.NullValuePromptShowForEmptyValue = True

            _btnCompletar.Text = "Completar restablecimiento"
            _btnCompletar.Height = 38

            layoutCompletar.Controls.Add(_txtToken)
            layoutCompletar.Controls.Add(_txtNuevaClave)
            layoutCompletar.Controls.Add(_txtConfirmarClave)
            layoutCompletar.Controls.Add(_btnCompletar)

            Dim rootCompletar As New LayoutControlGroup()
            rootCompletar.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            rootCompletar.GroupBordersVisible = False
            layoutCompletar.Root = rootCompletar

            Dim itemToken = rootCompletar.AddItem("Token", _txtToken)
            itemToken.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 4)

            Dim itemNueva = rootCompletar.AddItem("Nueva clave", _txtNuevaClave)
            itemNueva.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 4, 4)

            Dim itemConfirm = rootCompletar.AddItem("Confirmar", _txtConfirmarClave)
            itemConfirm.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 4, 4)

            Dim itemCompletar = rootCompletar.AddItem(String.Empty, _btnCompletar)
            itemCompletar.TextVisible = False
            itemCompletar.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 12, 6)

            _lblEstado.Text = "Esperando inicio del flujo..."
            _lblEstado.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)
            _lblEstado.Appearance.ForeColor = Color.DimGray

            _btnCerrar.Text = "Cerrar"
            _btnCerrar.Height = 38

            layout.Controls.Add(lblTitulo)
            layout.Controls.Add(lblAyuda)
            layout.Controls.Add(_txtTenant)
            layout.Controls.Add(_txtUsuarioCorreo)
            layout.Controls.Add(_cmbMetodo)
            layout.Controls.Add(_btnEnviar)
            layout.Controls.Add(_grpCompletar)
            layout.Controls.Add(_lblEstado)
            layout.Controls.Add(_btnCerrar)

            Dim root As New LayoutControlGroup()
            root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            root.GroupBordersVisible = False
            layout.Root = root

            Dim itemTitulo = root.AddItem(String.Empty, lblTitulo)
            itemTitulo.TextVisible = False
            itemTitulo.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 10, 2)

            Dim itemAyuda = root.AddItem(String.Empty, lblAyuda)
            itemAyuda.TextVisible = False
            itemAyuda.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 14)

            Dim itemTenant = root.AddItem("Tenant", _txtTenant)
            itemTenant.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 2)

            Dim itemUsuario = root.AddItem("Usuario/Correo", _txtUsuarioCorreo)
            itemUsuario.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 2)

            Dim itemMetodo = root.AddItem("Metodo", _cmbMetodo)
            itemMetodo.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 2)

            Dim itemEnviar = root.AddItem(String.Empty, _btnEnviar)
            itemEnviar.TextVisible = False
            itemEnviar.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 14, 4)

            Dim itemGrp = root.AddItem(String.Empty, _grpCompletar)
            itemGrp.TextVisible = False
            itemGrp.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 6, 4)

            Dim itemEstado = root.AddItem(String.Empty, _lblEstado)
            itemEstado.TextVisible = False
            itemEstado.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 8, 4)

            Dim itemCerrar = root.AddItem(String.Empty, _btnCerrar)
            itemCerrar.TextVisible = False
            itemCerrar.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 4, 8)

            ResumeLayout(False)
        End Sub
    End Class
End Namespace
