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
            _txtCodigo = New TextEdit()
            _btnValidar = New SimpleButton()
            _btnReenviar = New SimpleButton()
            _btnCancelar = New SimpleButton()
            _lblUsuario = New LabelControl()
            _lblTenant = New LabelControl()
            _lblCodigoPrueba = New LabelControl()

            SuspendLayout()

            Text = "Validacion MFA"
            StartPosition = FormStartPosition.CenterScreen
            FormBorderStyle = FormBorderStyle.Sizable
            MaximizeBox = False
            MinimizeBox = False
            Width = 700
            Height = 420
            MinimumSize = New Size(620, 380)
            AutoScaleMode = AutoScaleMode.Dpi

            Dim layout As New LayoutControl() With {
                .Dock = DockStyle.Fill
            }
            Controls.Add(layout)

            Dim lblTitulo As New LabelControl() With {
                .Text = "Segundo factor de autenticacion"
            }
            lblTitulo.Appearance.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)

            Dim lblDetalle As New LabelControl() With {
                .Text = "Ingrese el codigo OTP enviado por su canal seguro."
            }
            lblDetalle.Appearance.Font = New Font("Segoe UI", 10.0F)
            lblDetalle.Appearance.ForeColor = Color.DimGray

            _lblUsuario.Appearance.Font = New Font("Segoe UI", 9.5F)
            _lblTenant.Appearance.Font = New Font("Segoe UI", 9.5F)
            _lblUsuario.Appearance.ForeColor = Color.FromArgb(65, 65, 65)
            _lblTenant.Appearance.ForeColor = Color.FromArgb(65, 65, 65)

            _lblCodigoPrueba.Appearance.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            _lblCodigoPrueba.Appearance.ForeColor = Color.FromArgb(196, 62, 22)
            _lblCodigoPrueba.Visible = False

            _txtCodigo.Properties.MaxLength = 6
            _txtCodigo.Properties.NullValuePrompt = "Codigo MFA"
            _txtCodigo.Properties.NullValuePromptShowForEmptyValue = True
            _txtCodigo.Properties.Appearance.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)
            _txtCodigo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

            _btnValidar.Text = "Validar"
            _btnValidar.Height = 40
            _btnReenviar.Text = "Reenviar codigo"
            _btnReenviar.Height = 40
            _btnCancelar.Text = "Cancelar"
            _btnCancelar.Height = 40

            layout.Controls.Add(lblTitulo)
            layout.Controls.Add(lblDetalle)
            layout.Controls.Add(_lblUsuario)
            layout.Controls.Add(_lblTenant)
            layout.Controls.Add(_lblCodigoPrueba)
            layout.Controls.Add(_txtCodigo)
            layout.Controls.Add(_btnValidar)
            layout.Controls.Add(_btnReenviar)
            layout.Controls.Add(_btnCancelar)

            Dim root As New LayoutControlGroup()
            root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            root.GroupBordersVisible = False
            layout.Root = root

            Dim itemTitulo = root.AddItem(String.Empty, lblTitulo)
            itemTitulo.TextVisible = False
            itemTitulo.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 14, 2)

            Dim itemDetalle = root.AddItem(String.Empty, lblDetalle)
            itemDetalle.TextVisible = False
            itemDetalle.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 10)

            Dim itemUsuario = root.AddItem(String.Empty, _lblUsuario)
            itemUsuario.TextVisible = False
            itemUsuario.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 2)

            Dim itemTenant = root.AddItem(String.Empty, _lblTenant)
            itemTenant.TextVisible = False
            itemTenant.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 8)

            Dim itemCodigoPrueba = root.AddItem(String.Empty, _lblCodigoPrueba)
            itemCodigoPrueba.TextVisible = False
            itemCodigoPrueba.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 8)

            Dim itemCodigo = root.AddItem("Codigo", _txtCodigo)
            itemCodigo.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 4, 4)

            Dim itemValidar = root.AddItem(String.Empty, _btnValidar)
            itemValidar.TextVisible = False
            itemValidar.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 14, 4)

            Dim itemReenviar = root.AddItem(String.Empty, _btnReenviar)
            itemReenviar.TextVisible = False
            itemReenviar.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 4, 4)

            Dim itemCancelar = root.AddItem(String.Empty, _btnCancelar)
            itemCancelar.TextVisible = False
            itemCancelar.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 4, 4)

            ResumeLayout(False)
        End Sub
    End Class
End Namespace
