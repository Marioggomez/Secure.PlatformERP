Imports System.Windows.Forms
Imports System.Drawing
Imports DevExpress.XtraEditors

Namespace Forms.Common
    Public Class FrmApiErrorDetails
        Inherits XtraForm

        Private ReadOnly _tabs As TabControl
        Private ReadOnly _txtUser As MemoEdit
        Private ReadOnly _txtTech As MemoEdit
        Private ReadOnly _btnCopy As SimpleButton
        Private ReadOnly _btnClose As SimpleButton

        Public Sub New(ByVal userMessage As String, ByVal technicalDetails As String)
            _tabs = New TabControl()
            _txtUser = New MemoEdit()
            _txtTech = New MemoEdit()
            _btnCopy = New SimpleButton()
            _btnClose = New SimpleButton()

            InitializeComponent()

            _txtUser.Text = userMessage
            _txtTech.Text = technicalDetails
        End Sub

        Private Sub InitializeComponent()
            SuspendLayout()
            Text = "Detalle de error"
            StartPosition = FormStartPosition.CenterParent
            FormBorderStyle = FormBorderStyle.FixedDialog
            MinimizeBox = False
            MaximizeBox = False
            ShowInTaskbar = False
            ClientSize = New Size(760, 420)

            _tabs.Dock = DockStyle.Top
            _tabs.Height = 360

            Dim tabUser As New TabPage("Usuario")
            Dim tabTech As New TabPage("Detalle tecnico")

            _txtUser.Dock = DockStyle.Fill
            _txtUser.Properties.ReadOnly = True
            _txtUser.Properties.WordWrap = True
            tabUser.Controls.Add(_txtUser)

            _txtTech.Dock = DockStyle.Fill
            _txtTech.Properties.ReadOnly = True
            _txtTech.Properties.WordWrap = False
            tabTech.Controls.Add(_txtTech)

            _tabs.TabPages.Add(tabUser)
            _tabs.TabPages.Add(tabTech)

            _btnCopy.Text = "Copiar detalle"
            _btnCopy.Width = 130
            _btnCopy.Location = New Point(480, 372)
            AddHandler _btnCopy.Click, AddressOf OnCopyClick

            _btnClose.Text = "Cerrar"
            _btnClose.Width = 90
            _btnClose.Location = New Point(625, 372)
            AddHandler _btnClose.Click, AddressOf OnCloseClick

            Controls.Add(_tabs)
            Controls.Add(_btnCopy)
            Controls.Add(_btnClose)
            ResumeLayout(False)
        End Sub

        Private Sub OnCopyClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim payload = $"Usuario:{Environment.NewLine}{_txtUser.Text}{Environment.NewLine}{Environment.NewLine}Tecnico:{Environment.NewLine}{_txtTech.Text}"
            Clipboard.SetText(payload)
        End Sub

        Private Sub OnCloseClick(ByVal sender As Object, ByVal e As EventArgs)
            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End Namespace
