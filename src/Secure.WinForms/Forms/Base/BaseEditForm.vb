Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout

Namespace Forms.Base
    ''' <summary>
    ''' Formulario base reutilizable para mantenimiento/edicion.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class BaseEditForm
        Inherits XtraForm

        Protected ReadOnly Ribbon As RibbonControl
        Protected ReadOnly MainLayout As LayoutControl
        Protected ReadOnly EditorPanel As PanelControl

        Private ReadOnly _statusBar As RibbonStatusBar
        Private ReadOnly _statusInfo As BarStaticItem

        Protected Sub New()
            Ribbon = New RibbonControl()
            MainLayout = New LayoutControl()
            EditorPanel = New PanelControl()
            _statusBar = New RibbonStatusBar()
            _statusInfo = New BarStaticItem()

            InitializeComponent()
            BuildEditor(EditorPanel)
        End Sub

        ''' <summary>
        ''' Ribbon del modulo para integracion/merge con el shell.
        ''' </summary>
        Public ReadOnly Property ModuleRibbon As RibbonControl
            Get
                Return Ribbon
            End Get
        End Property

        ''' <summary>
        ''' Titulo de modulo para tab/documento.
        ''' </summary>
        Public Overridable ReadOnly Property ModuleTitle As String
            Get
                Return BuildFormTitle()
            End Get
        End Property

        ''' <summary>
        ''' Define modo hosteado: el ribbon del formulario se oculta si el shell realiza merge.
        ''' </summary>
        Public Overridable Sub SetRibbonHostMode(ByVal hosted As Boolean)
            Ribbon.Visible = Not hosted
        End Sub

        Private Sub InitializeComponent()
            Text = BuildFormTitle()
            Width = 980
            Height = 680
            StartPosition = FormStartPosition.CenterParent
            AutoScaleMode = AutoScaleMode.Dpi

            ConfigureRibbon()
            ConfigureLayout()
            ConfigureStatus()

            Controls.Add(MainLayout)
            Controls.Add(_statusBar)
            Controls.Add(Ribbon)
        End Sub

        Private Sub ConfigureRibbon()
            Ribbon.Dock = DockStyle.Top
            Ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            Ribbon.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden
            Ribbon.MdiMergeStyle = RibbonMdiMergeStyle.Always

            Dim btnGuardar As New BarButtonItem() With {.Caption = "Guardar"}
            Dim btnGuardarCerrar As New BarButtonItem() With {.Caption = "Guardar y cerrar"}
            Dim btnCancelar As New BarButtonItem() With {.Caption = "Cancelar"}

            Ribbon.Items.AddRange(New BarItem() {btnGuardar, btnGuardarCerrar, btnCancelar, _statusInfo})

            Dim page As New RibbonPage("Mantenimiento")
            Dim group As New RibbonPageGroup("Acciones")
            group.ItemLinks.Add(btnGuardar)
            group.ItemLinks.Add(btnGuardarCerrar)
            group.ItemLinks.Add(btnCancelar)
            page.Groups.Add(group)
            Ribbon.Pages.Add(page)

            AddHandler btnGuardar.ItemClick, AddressOf OnGuardarClick
            AddHandler btnGuardarCerrar.ItemClick, AddressOf OnGuardarCerrarClick
            AddHandler btnCancelar.ItemClick, AddressOf OnCancelarClick
        End Sub

        Private Sub ConfigureLayout()
            MainLayout.Dock = DockStyle.Fill
            MainLayout.Root = New LayoutControlGroup() With {
                .EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True,
                .GroupBordersVisible = False
            }

            EditorPanel.Dock = DockStyle.Fill
            EditorPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder

            MainLayout.Controls.Add(EditorPanel)
            Dim item = DirectCast(MainLayout.Root, LayoutControlGroup).AddItem(String.Empty, EditorPanel)
            item.TextVisible = False
        End Sub

        Private Sub ConfigureStatus()
            _statusBar.Dock = DockStyle.Bottom
            _statusBar.Ribbon = Ribbon
            _statusInfo.Caption = $"Endpoint: {BuildSaveEndpoint()}"
            _statusBar.ItemLinks.Add(_statusInfo)
        End Sub

        Protected Overridable Function BuildFormTitle() As String
            Return "Edicion"
        End Function

        Protected Overridable Sub BuildEditor(ByVal panel As PanelControl)
            Dim lbl As New LabelControl() With {
                .Text = "Defina controles de edicion en el formulario derivado.",
                .Location = New Point(18, 20)
            }
            panel.Controls.Add(lbl)
        End Sub

        Protected Overridable Sub OnGuardarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            XtraMessageBox.Show(Me, $"Guardar en {BuildSaveEndpoint()}", "Operacion", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Protected Overridable Sub OnGuardarCerrarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            OnGuardarClick(sender, e)
            DialogResult = DialogResult.OK
            Close()
        End Sub

        Protected Overridable Sub OnCancelarClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Protected Overridable Function BuildSaveEndpoint() As String
            Return "api/v1/base/edit"
        End Function
    End Class
End Namespace
