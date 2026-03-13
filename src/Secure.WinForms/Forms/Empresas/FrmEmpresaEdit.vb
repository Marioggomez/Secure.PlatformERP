Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports Secure.Platform.WinForms.Forms.Base

Namespace Forms.Empresas
    ''' <summary>
    ''' Formulario de mantenimiento de empresa.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmEmpresaEdit
        Inherits BaseEditForm

        Protected Overrides Function BuildSaveEndpoint() As String
            Return "api/v1/organizacion/empresa/listar"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Empresas - Edicion"
        End Function

        Protected Overrides Sub BuildEditor(ByVal panel As PanelControl)
            Dim layout As New LayoutControl() With {.Dock = DockStyle.Fill}
            panel.Controls.Add(layout)

            Dim txtCodigo As New TextEdit()
            Dim txtNombre As New TextEdit()
            Dim txtNombreLegal As New TextEdit()
            Dim txtNit As New TextEdit()
            Dim cmbEstado As New ComboBoxEdit()
            cmbEstado.Properties.Items.AddRange(New Object() {"ACTIVA", "INACTIVA"})
            cmbEstado.SelectedIndex = 0

            layout.Controls.Add(txtCodigo)
            layout.Controls.Add(txtNombre)
            layout.Controls.Add(txtNombreLegal)
            layout.Controls.Add(txtNit)
            layout.Controls.Add(cmbEstado)

            Dim root As New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }
            layout.Root = root

            root.AddItem("Codigo", txtCodigo)
            root.AddItem("Nombre", txtNombre)
            root.AddItem("Razon social", txtNombreLegal)
            root.AddItem("Identificacion fiscal", txtNit)
            root.AddItem("Estado", cmbEstado)
        End Sub
    End Class
End Namespace

