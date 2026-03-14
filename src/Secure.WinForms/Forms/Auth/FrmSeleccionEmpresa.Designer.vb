Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports System.ComponentModel

Namespace Forms.Auth
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class FrmSeleccionEmpresa
        Inherits XtraForm

        Friend WithEvents _layoutMain As LayoutControl
        Friend WithEvents _lblUsuario As LabelControl
        Friend WithEvents _lblTenant As LabelControl
        Friend WithEvents _lblDetalle As LabelControl
        Friend WithEvents _lookEmpresa As LookUpEdit
        Friend WithEvents _btnContinuar As SimpleButton
        Friend WithEvents _btnCancelar As SimpleButton
        Friend WithEvents _lblEstado As LabelControl
        Friend WithEvents _root As LayoutControlGroup

        Private Sub InitializeComponent()
            _layoutMain = New LayoutControl()
            _lblUsuario = New LabelControl()
            _lblTenant = New LabelControl()
            _lblDetalle = New LabelControl()
            _lookEmpresa = New LookUpEdit()
            _btnContinuar = New SimpleButton()
            _btnCancelar = New SimpleButton()
            _lblEstado = New LabelControl()
            _root = New LayoutControlGroup()
            CType(_layoutMain, ComponentModel.ISupportInitialize).BeginInit()
            _layoutMain.SuspendLayout()
            CType(_lookEmpresa.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(_root, ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            '
            ' _layoutMain
            '
            _layoutMain.Controls.Add(_lblUsuario)
            _layoutMain.Controls.Add(_lblTenant)
            _layoutMain.Controls.Add(_lblDetalle)
            _layoutMain.Controls.Add(_lookEmpresa)
            _layoutMain.Controls.Add(_btnContinuar)
            _layoutMain.Controls.Add(_btnCancelar)
            _layoutMain.Controls.Add(_lblEstado)
            _layoutMain.Dock = DockStyle.Fill
            _layoutMain.Location = New Point(0, 0)
            _layoutMain.Name = "_layoutMain"
            _layoutMain.Root = _root
            _layoutMain.Size = New Size(640, 250)
            _layoutMain.TabIndex = 0
            '
            ' _lblUsuario
            '
            _lblUsuario.Appearance.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
            _lblUsuario.Appearance.Options.UseFont = True
            _lblUsuario.Location = New Point(12, 12)
            _lblUsuario.Name = "_lblUsuario"
            _lblUsuario.Size = New Size(616, 20)
            _lblUsuario.StyleController = _layoutMain
            _lblUsuario.TabIndex = 4
            _lblUsuario.Text = "Usuario:"
            '
            ' _lblTenant
            '
            _lblTenant.Appearance.Font = New Font("Segoe UI", 10.0F)
            _lblTenant.Appearance.Options.UseFont = True
            _lblTenant.Location = New Point(12, 36)
            _lblTenant.Name = "_lblTenant"
            _lblTenant.Size = New Size(616, 17)
            _lblTenant.StyleController = _layoutMain
            _lblTenant.TabIndex = 5
            _lblTenant.Text = "Tenant:"
            '
            ' _lblDetalle
            '
            _lblDetalle.Appearance.Font = New Font("Segoe UI", 9.5F)
            _lblDetalle.Appearance.ForeColor = Color.DimGray
            _lblDetalle.Appearance.Options.UseFont = True
            _lblDetalle.Appearance.Options.UseForeColor = True
            _lblDetalle.Location = New Point(12, 57)
            _lblDetalle.Name = "_lblDetalle"
            _lblDetalle.Size = New Size(616, 17)
            _lblDetalle.StyleController = _layoutMain
            _lblDetalle.TabIndex = 6
            _lblDetalle.Text = "Seleccione la empresa en la que desea operar."
            '
            ' _lookEmpresa
            '
            _lookEmpresa.Location = New Point(12, 92)
            _lookEmpresa.Name = "_lookEmpresa"
            _lookEmpresa.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            _lookEmpresa.Properties.NullText = "Seleccione empresa..."
            _lookEmpresa.Size = New Size(616, 20)
            _lookEmpresa.StyleController = _layoutMain
            _lookEmpresa.TabIndex = 7
            '
            ' _btnContinuar
            '
            _btnContinuar.Location = New Point(12, 143)
            _btnContinuar.Name = "_btnContinuar"
            _btnContinuar.Size = New Size(302, 32)
            _btnContinuar.StyleController = _layoutMain
            _btnContinuar.TabIndex = 8
            _btnContinuar.Text = "Continuar"
            '
            ' _btnCancelar
            '
            _btnCancelar.Location = New Point(318, 143)
            _btnCancelar.Name = "_btnCancelar"
            _btnCancelar.Size = New Size(310, 32)
            _btnCancelar.StyleController = _layoutMain
            _btnCancelar.TabIndex = 9
            _btnCancelar.Text = "Cancelar"
            '
            ' _lblEstado
            '
            _lblEstado.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)
            _lblEstado.Appearance.ForeColor = Color.DimGray
            _lblEstado.Appearance.Options.UseFont = True
            _lblEstado.Appearance.Options.UseForeColor = True
            _lblEstado.Location = New Point(12, 116)
            _lblEstado.Name = "_lblEstado"
            _lblEstado.Size = New Size(616, 23)
            _lblEstado.StyleController = _layoutMain
            _lblEstado.TabIndex = 10
            _lblEstado.Text = "Validando empresa..."
            '
            ' _root
            '
            _root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            _root.GroupBordersVisible = False
            _root.Name = "_root"
            _root.Size = New Size(640, 250)
            _root.TextVisible = False

            Dim itemUsuario = _root.AddItem(String.Empty, _lblUsuario)
            itemUsuario.TextVisible = False
            itemUsuario.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 2)

            Dim itemTenant = _root.AddItem(String.Empty, _lblTenant)
            itemTenant.TextVisible = False
            itemTenant.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 2, 2)

            Dim itemDetalle = _root.AddItem(String.Empty, _lblDetalle)
            itemDetalle.TextVisible = False
            itemDetalle.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 2, 8)

            Dim itemEmpresa = _root.AddItem("Empresa", _lookEmpresa)
            itemEmpresa.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 8, 2)
            itemEmpresa.AppearanceItemCaption.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
            itemEmpresa.AppearanceItemCaption.Options.UseFont = True

            Dim itemEstado = _root.AddItem(String.Empty, _lblEstado)
            itemEstado.TextVisible = False
            itemEstado.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 6, 2, 2)

            Dim itemContinuar = _root.AddItem(String.Empty, _btnContinuar)
            itemContinuar.TextVisible = False
            itemContinuar.Padding = New DevExpress.XtraLayout.Utils.Padding(6, 3, 10, 8)

            Dim itemCancelar = _root.AddItem(String.Empty, _btnCancelar)
            itemCancelar.TextVisible = False
            itemCancelar.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 6, 10, 8)
            itemCancelar.Move(itemContinuar, DevExpress.XtraLayout.Utils.InsertType.Right)
            '
            ' FrmSeleccionEmpresa
            '
            AutoScaleDimensions = New SizeF(96.0F, 96.0F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(640, 250)
            Controls.Add(_layoutMain)
            MaximizeBox = False
            MinimizeBox = False
            MinimumSize = New Size(660, 290)
            Name = "FrmSeleccionEmpresa"
            StartPosition = FormStartPosition.CenterScreen
            Text = "Seleccion de empresa"
            CType(_layoutMain, ComponentModel.ISupportInitialize).EndInit()
            _layoutMain.ResumeLayout(False)
            CType(_lookEmpresa.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(_root, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
        End Sub
    End Class
End Namespace
