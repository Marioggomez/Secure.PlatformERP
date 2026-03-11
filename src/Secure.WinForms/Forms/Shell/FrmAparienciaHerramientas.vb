Imports DevExpress.Skins
Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Shell
    ''' <summary>
    ''' Modulo de apariencia y herramientas visuales del ERP.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmAparienciaHerramientas
        Inherits XtraForm

        Private ReadOnly _listTemas As ListBoxControl
        Private ReadOnly _btnAplicar As SimpleButton
        Private ReadOnly _lblTemaActual As LabelControl

        Public Sub New()
            _listTemas = New ListBoxControl()
            _btnAplicar = New SimpleButton()
            _lblTemaActual = New LabelControl()

            InitializeComponent()
            LoadThemes()
            WireEvents()
        End Sub

        Private Sub InitializeComponent()
            Text = "Apariencia y herramientas"
            FormBorderStyle = FormBorderStyle.None
            Dock = DockStyle.Fill

            Dim mainLayout As New LayoutControl() With {
                .Dock = DockStyle.Fill
            }
            Controls.Add(mainLayout)

            Dim lblTitulo As New LabelControl() With {
                .Text = "Personalizacion visual del ERP"
            }
            lblTitulo.Appearance.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)

            Dim lblHelp As New LabelControl() With {
                .Text = "Seleccione un tema para todo el cliente. El ajuste se guarda automaticamente por usuario."
            }
            lblHelp.Appearance.Font = New Font("Segoe UI", 10.0F)
            lblHelp.Appearance.ForeColor = Color.DimGray

            _listTemas.Height = 260

            _lblTemaActual.Appearance.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            _lblTemaActual.Appearance.ForeColor = Color.FromArgb(40, 80, 120)
            _lblTemaActual.Text = "Tema actual: "

            _btnAplicar.Text = "Aplicar tema"
            _btnAplicar.Height = 40

            mainLayout.Controls.Add(lblTitulo)
            mainLayout.Controls.Add(lblHelp)
            mainLayout.Controls.Add(_listTemas)
            mainLayout.Controls.Add(_lblTemaActual)
            mainLayout.Controls.Add(_btnAplicar)

            Dim root As New LayoutControlGroup()
            root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            root.GroupBordersVisible = False
            mainLayout.Root = root

            Dim itemTitulo = root.AddItem(String.Empty, lblTitulo)
            itemTitulo.TextVisible = False
            itemTitulo.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 18, 4)

            Dim itemHelp = root.AddItem(String.Empty, lblHelp)
            itemHelp.TextVisible = False
            itemHelp.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 12)

            Dim itemTemaActual = root.AddItem(String.Empty, _lblTemaActual)
            itemTemaActual.TextVisible = False
            itemTemaActual.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 8)

            Dim itemLista = root.AddItem("Temas", _listTemas)
            itemLista.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 8)

            Dim itemAplicar = root.AddItem(String.Empty, _btnAplicar)
            itemAplicar.TextVisible = False
            itemAplicar.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 8, 8)
        End Sub

        Private Sub WireEvents()
            AddHandler _btnAplicar.Click, AddressOf OnAplicarClick
            AddHandler _listTemas.DoubleClick, AddressOf OnAplicarClick
        End Sub

        Private Sub LoadThemes()
            _listTemas.Items.Clear()

            For Each container As SkinContainer In SkinManager.Default.Skins
                _listTemas.Items.Add(container.SkinName)
            Next

            Dim actual = ThemeService.GetCurrentTheme()
            _lblTemaActual.Text = $"Tema actual: {actual}"

            If Not String.IsNullOrWhiteSpace(actual) Then
                _listTemas.SelectedItem = actual
            End If
        End Sub

        Private Sub OnAplicarClick(ByVal sender As Object, ByVal e As EventArgs)
            If _listTemas.SelectedItem Is Nothing Then
                XtraMessageBox.Show(Me, "Seleccione un tema para aplicar.", "Apariencia", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim selectedTheme = _listTemas.SelectedItem.ToString()
            ThemeService.ApplyTheme(selectedTheme)
            _lblTemaActual.Text = $"Tema actual: {ThemeService.GetCurrentTheme()}"
        End Sub
    End Class
End Namespace
