Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports Secure.Platform.WinForms.Forms.Base

Namespace Forms.Usuarios
    ''' <summary>
    ''' Formulario de mantenimiento de usuario.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmUsuarioEdit
        Inherits BaseEditForm

        Protected Overrides Function BuildSaveEndpoint() As String
            Return "api/v1/seguridad/usuario"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Usuarios - Edicion"
        End Function

        Protected Overrides Sub BuildEditor(ByVal panel As PanelControl)
            Dim layout As New LayoutControl() With {.Dock = DockStyle.Fill}
            panel.Controls.Add(layout)

            Dim txtLogin As New TextEdit()
            Dim txtNombre As New TextEdit()
            Dim txtApellido As New TextEdit()
            Dim txtCorreo As New TextEdit()
            Dim cmbEstado As New ComboBoxEdit()
            cmbEstado.Properties.Items.AddRange(New Object() {"ACTIVO", "BLOQUEADO", "INACTIVO"})
            cmbEstado.SelectedIndex = 0

            layout.Controls.Add(txtLogin)
            layout.Controls.Add(txtNombre)
            layout.Controls.Add(txtApellido)
            layout.Controls.Add(txtCorreo)
            layout.Controls.Add(cmbEstado)

            Dim root As New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }
            layout.Root = root

            root.AddItem("Login", txtLogin)
            root.AddItem("Nombre", txtNombre)
            root.AddItem("Apellido", txtApellido)
            root.AddItem("Correo", txtCorreo)
            root.AddItem("Estado", cmbEstado)
        End Sub
    End Class
End Namespace
