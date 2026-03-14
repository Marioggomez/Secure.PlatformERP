Namespace Forms.Base
    Partial Class BaseSearchForm
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BaseSearchForm))
            Ribbon = New DevExpress.XtraBars.Ribbon.RibbonControl()
            _btnNuevo = New DevExpress.XtraBars.BarButtonItem()
            _btnEditar = New DevExpress.XtraBars.BarButtonItem()
            _btnRefrescar = New DevExpress.XtraBars.BarButtonItem()
            _btnPrimero = New DevExpress.XtraBars.BarButtonItem()
            _btnAnterior = New DevExpress.XtraBars.BarButtonItem()
            _btnSiguiente = New DevExpress.XtraBars.BarButtonItem()
            _btnUltimo = New DevExpress.XtraBars.BarButtonItem()
            _btnGuardarVista = New DevExpress.XtraBars.BarButtonItem()
            _btnRestaurarVista = New DevExpress.XtraBars.BarButtonItem()
            _btnExportar = New DevExpress.XtraBars.BarButtonItem()
            _barCampoFiltro = New DevExpress.XtraBars.BarEditItem()
            _repoCampoFiltro = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
            _barBusqueda = New DevExpress.XtraBars.BarEditItem()
            _repoBusqueda = New DevExpress.XtraEditors.Repository.RepositoryItemSearchControl()
            _barBuscar = New DevExpress.XtraBars.BarButtonItem()
            _barLimpiar = New DevExpress.XtraBars.BarButtonItem()
            _barTamanoPagina = New DevExpress.XtraBars.BarEditItem()
            _repoTamanoPagina = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
            _barPaginaAnterior = New DevExpress.XtraBars.BarButtonItem()
            _barPaginaSiguiente = New DevExpress.XtraBars.BarButtonItem()
            _barResumenPagina = New DevExpress.XtraBars.BarStaticItem()
            _statusInfo = New DevExpress.XtraBars.BarStaticItem()
            _statusPaginacion = New DevExpress.XtraBars.BarStaticItem()
            _rpInicio = New DevExpress.XtraBars.Ribbon.RibbonPage()
            _rpgOperaciones = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            _rpgBusqueda = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            _rpgPaginacion = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            _rpgNavegacion = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            _rpgVistaReporte = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            _statusBar = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
            Grid = New DevExpress.XtraGrid.GridControl()
            View = New DevExpress.XtraGrid.Views.Grid.GridView()
            MainLayout = New DevExpress.XtraLayout.LayoutControl()
            _layoutRoot = New DevExpress.XtraLayout.LayoutControlGroup()
            _layoutGridItem = New DevExpress.XtraLayout.LayoutControlItem()
            TxtFiltro = New DevExpress.XtraEditors.TextEdit()
            CmbCampoFiltro = New DevExpress.XtraEditors.ComboBoxEdit()
            SpnTamanoPagina = New DevExpress.XtraEditors.SpinEdit()
            CType(Ribbon, ComponentModel.ISupportInitialize).BeginInit()
            CType(_repoCampoFiltro, ComponentModel.ISupportInitialize).BeginInit()
            CType(_repoBusqueda, ComponentModel.ISupportInitialize).BeginInit()
            CType(_repoTamanoPagina, ComponentModel.ISupportInitialize).BeginInit()
            CType(Grid, ComponentModel.ISupportInitialize).BeginInit()
            CType(View, ComponentModel.ISupportInitialize).BeginInit()
            CType(MainLayout, ComponentModel.ISupportInitialize).BeginInit()
            MainLayout.SuspendLayout()
            CType(_layoutRoot, ComponentModel.ISupportInitialize).BeginInit()
            CType(_layoutGridItem, ComponentModel.ISupportInitialize).BeginInit()
            CType(TxtFiltro.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(CmbCampoFiltro.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(SpnTamanoPagina.Properties, ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' Ribbon
            ' 
            Ribbon.ExpandCollapseItem.Id = 0
            Ribbon.Items.AddRange(New DevExpress.XtraBars.BarItem() {Ribbon.ExpandCollapseItem, _btnNuevo, _btnEditar, _btnRefrescar, _btnPrimero, _btnAnterior, _btnSiguiente, _btnUltimo, _btnGuardarVista, _btnRestaurarVista, _btnExportar, _barCampoFiltro, _barBusqueda, _barBuscar, _barLimpiar, _barTamanoPagina, _barPaginaAnterior, _barPaginaSiguiente, _barResumenPagina, _statusInfo, _statusPaginacion})
            Ribbon.Location = New Point(0, 0)
            Ribbon.MaxItemId = 100
            Ribbon.Name = "Ribbon"
            Ribbon.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {_rpInicio})
            Ribbon.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {_repoCampoFiltro, _repoBusqueda, _repoTamanoPagina})
            Ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
            Ribbon.Size = New Size(1495, 126)
            Ribbon.StatusBar = _statusBar
            Ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden
            ' 
            ' _btnNuevo
            ' 
            _btnNuevo.Caption = "Nuevo"
            _btnNuevo.Id = 2
            _btnNuevo.ImageOptions.Image = CType(resources.GetObject("_btnNuevo.ImageOptions.Image"), Image)
            _btnNuevo.ImageOptions.LargeImage = CType(resources.GetObject("_btnNuevo.ImageOptions.LargeImage"), Image)
            _btnNuevo.Name = "_btnNuevo"
            _btnNuevo.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText
            ' 
            ' _btnEditar
            ' 
            _btnEditar.Caption = "Editar"
            _btnEditar.Id = 3
            _btnEditar.ImageOptions.Image = CType(resources.GetObject("_btnEditar.ImageOptions.Image"), Image)
            _btnEditar.ImageOptions.LargeImage = CType(resources.GetObject("_btnEditar.ImageOptions.LargeImage"), Image)
            _btnEditar.Name = "_btnEditar"
            ' 
            ' _btnRefrescar
            ' 
            _btnRefrescar.Caption = "Refrescar"
            _btnRefrescar.Id = 4
            _btnRefrescar.ImageOptions.Image = CType(resources.GetObject("_btnRefrescar.ImageOptions.Image"), Image)
            _btnRefrescar.ImageOptions.LargeImage = CType(resources.GetObject("_btnRefrescar.ImageOptions.LargeImage"), Image)
            _btnRefrescar.Name = "_btnRefrescar"
            ' 
            ' _btnPrimero
            ' 
            _btnPrimero.Caption = "Primer registro"
            _btnPrimero.Id = 5
            _btnPrimero.ImageOptions.Image = CType(resources.GetObject("_btnPrimero.ImageOptions.Image"), Image)
            _btnPrimero.ImageOptions.LargeImage = CType(resources.GetObject("_btnPrimero.ImageOptions.LargeImage"), Image)
            _btnPrimero.Name = "_btnPrimero"
            ' 
            ' _btnAnterior
            ' 
            _btnAnterior.Caption = "Registro anterior"
            _btnAnterior.Id = 6
            _btnAnterior.ImageOptions.Image = CType(resources.GetObject("_btnAnterior.ImageOptions.Image"), Image)
            _btnAnterior.ImageOptions.LargeImage = CType(resources.GetObject("_btnAnterior.ImageOptions.LargeImage"), Image)
            _btnAnterior.Name = "_btnAnterior"
            ' 
            ' _btnSiguiente
            ' 
            _btnSiguiente.Caption = "Registro siguiente"
            _btnSiguiente.Id = 7
            _btnSiguiente.ImageOptions.Image = CType(resources.GetObject("_btnSiguiente.ImageOptions.Image"), Image)
            _btnSiguiente.ImageOptions.LargeImage = CType(resources.GetObject("_btnSiguiente.ImageOptions.LargeImage"), Image)
            _btnSiguiente.Name = "_btnSiguiente"
            ' 
            ' _btnUltimo
            ' 
            _btnUltimo.Caption = "Ultimo registro"
            _btnUltimo.Id = 8
            _btnUltimo.ImageOptions.Image = CType(resources.GetObject("_btnUltimo.ImageOptions.Image"), Image)
            _btnUltimo.ImageOptions.LargeImage = CType(resources.GetObject("_btnUltimo.ImageOptions.LargeImage"), Image)
            _btnUltimo.Name = "_btnUltimo"
            ' 
            ' _btnGuardarVista
            ' 
            _btnGuardarVista.Caption = "Guardar vista"
            _btnGuardarVista.Id = 9
            _btnGuardarVista.ImageOptions.Image = CType(resources.GetObject("_btnGuardarVista.ImageOptions.Image"), Image)
            _btnGuardarVista.ImageOptions.LargeImage = CType(resources.GetObject("_btnGuardarVista.ImageOptions.LargeImage"), Image)
            _btnGuardarVista.Name = "_btnGuardarVista"
            ' 
            ' _btnRestaurarVista
            ' 
            _btnRestaurarVista.Caption = "Restaurar vista"
            _btnRestaurarVista.Id = 10
            _btnRestaurarVista.ImageOptions.Image = CType(resources.GetObject("_btnRestaurarVista.ImageOptions.Image"), Image)
            _btnRestaurarVista.ImageOptions.LargeImage = CType(resources.GetObject("_btnRestaurarVista.ImageOptions.LargeImage"), Image)
            _btnRestaurarVista.Name = "_btnRestaurarVista"
            ' 
            ' _btnExportar
            ' 
            _btnExportar.Caption = "Exportar"
            _btnExportar.Id = 11
            _btnExportar.ImageOptions.Image = CType(resources.GetObject("_btnExportar.ImageOptions.Image"), Image)
            _btnExportar.ImageOptions.LargeImage = CType(resources.GetObject("_btnExportar.ImageOptions.LargeImage"), Image)
            _btnExportar.Name = "_btnExportar"
            ' 
            ' _barCampoFiltro
            ' 
            _barCampoFiltro.Caption = "Campo"
            _barCampoFiltro.Edit = _repoCampoFiltro
            _barCampoFiltro.EditWidth = 170
            _barCampoFiltro.Id = 12
            _barCampoFiltro.Name = "_barCampoFiltro"
            ' 
            ' _repoCampoFiltro
            ' 
            _repoCampoFiltro.AutoHeight = False
            _repoCampoFiltro.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            _repoCampoFiltro.Name = "_repoCampoFiltro"
            _repoCampoFiltro.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            ' 
            ' _barBusqueda
            ' 
            _barBusqueda.Caption = "Buscar"
            _barBusqueda.Edit = _repoBusqueda
            _barBusqueda.EditWidth = 280
            _barBusqueda.Id = 13
            _barBusqueda.Name = "_barBusqueda"
            ' 
            ' _repoBusqueda
            ' 
            _repoBusqueda.AutoHeight = False
            _repoBusqueda.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Repository.ClearButton(), New DevExpress.XtraEditors.Repository.SearchButton()})
            _repoBusqueda.Name = "_repoBusqueda"
            _repoBusqueda.NullValuePrompt = "Buscar en datos..."
            ' 
            ' _barBuscar
            ' 
            _barBuscar.Caption = "Buscar"
            _barBuscar.Id = 14
            _barBuscar.ImageOptions.Image = CType(resources.GetObject("_barBuscar.ImageOptions.Image"), Image)
            _barBuscar.ImageOptions.LargeImage = CType(resources.GetObject("_barBuscar.ImageOptions.LargeImage"), Image)
            _barBuscar.Name = "_barBuscar"
            ' 
            ' _barLimpiar
            ' 
            _barLimpiar.Caption = "Limpiar"
            _barLimpiar.Id = 15
            _barLimpiar.ImageOptions.Image = CType(resources.GetObject("_barLimpiar.ImageOptions.Image"), Image)
            _barLimpiar.ImageOptions.LargeImage = CType(resources.GetObject("_barLimpiar.ImageOptions.LargeImage"), Image)
            _barLimpiar.Name = "_barLimpiar"
            ' 
            ' _barTamanoPagina
            ' 
            _barTamanoPagina.Caption = "Tamano"
            _barTamanoPagina.Edit = _repoTamanoPagina
            _barTamanoPagina.EditValue = 25
            _barTamanoPagina.EditWidth = 70
            _barTamanoPagina.Id = 16
            _barTamanoPagina.Name = "_barTamanoPagina"
            ' 
            ' _repoTamanoPagina
            ' 
            _repoTamanoPagina.AutoHeight = False
            _repoTamanoPagina.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            _repoTamanoPagina.Increment = New Decimal(New Integer() {5, 0, 0, 0})
            _repoTamanoPagina.IsFloatValue = False
            _repoTamanoPagina.MaskSettings.Set("mask", "N00")
            _repoTamanoPagina.MaxValue = New Decimal(New Integer() {500, 0, 0, 0})
            _repoTamanoPagina.MinValue = New Decimal(New Integer() {5, 0, 0, 0})
            _repoTamanoPagina.Name = "_repoTamanoPagina"
            ' 
            ' _barPaginaAnterior
            ' 
            _barPaginaAnterior.Caption = "Pag. anterior"
            _barPaginaAnterior.Id = 17
            _barPaginaAnterior.ImageOptions.Image = CType(resources.GetObject("_barPaginaAnterior.ImageOptions.Image"), Image)
            _barPaginaAnterior.ImageOptions.LargeImage = CType(resources.GetObject("_barPaginaAnterior.ImageOptions.LargeImage"), Image)
            _barPaginaAnterior.Name = "_barPaginaAnterior"
            ' 
            ' _barPaginaSiguiente
            ' 
            _barPaginaSiguiente.Caption = "Pag. siguiente"
            _barPaginaSiguiente.Id = 18
            _barPaginaSiguiente.ImageOptions.Image = CType(resources.GetObject("_barPaginaSiguiente.ImageOptions.Image"), Image)
            _barPaginaSiguiente.ImageOptions.LargeImage = CType(resources.GetObject("_barPaginaSiguiente.ImageOptions.LargeImage"), Image)
            _barPaginaSiguiente.Name = "_barPaginaSiguiente"
            ' 
            ' _barResumenPagina
            ' 
            _barResumenPagina.Caption = "Pag 1/1"
            _barResumenPagina.Id = 19
            _barResumenPagina.Name = "_barResumenPagina"
            ' 
            ' _statusInfo
            ' 
            _statusInfo.Caption = "Origen:"
            _statusInfo.Id = 20
            _statusInfo.Name = "_statusInfo"
            ' 
            ' _statusPaginacion
            ' 
            _statusPaginacion.Caption = "Registros: 0"
            _statusPaginacion.Id = 21
            _statusPaginacion.Name = "_statusPaginacion"
            ' 
            ' _rpInicio
            ' 
            _rpInicio.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {_rpgOperaciones, _rpgBusqueda, _rpgPaginacion, _rpgNavegacion, _rpgVistaReporte})
            _rpInicio.Name = "_rpInicio"
            _rpInicio.Text = "Inicio"
            ' 
            ' _rpgOperaciones
            ' 
            _rpgOperaciones.ItemLinks.Add(_btnNuevo)
            _rpgOperaciones.ItemLinks.Add(_btnEditar)
            _rpgOperaciones.ItemLinks.Add(_btnRefrescar)
            _rpgOperaciones.Name = "_rpgOperaciones"
            _rpgOperaciones.Text = "Operaciones"
            ' 
            ' _rpgBusqueda
            ' 
            _rpgBusqueda.ItemLinks.Add(_barCampoFiltro)
            _rpgBusqueda.ItemLinks.Add(_barBusqueda)
            _rpgBusqueda.ItemLinks.Add(_barBuscar)
            _rpgBusqueda.ItemLinks.Add(_barLimpiar)
            _rpgBusqueda.Name = "_rpgBusqueda"
            _rpgBusqueda.Text = "Búsqueda"
            ' 
            ' _rpgPaginacion
            ' 
            _rpgPaginacion.ItemLinks.Add(_barTamanoPagina)
            _rpgPaginacion.ItemLinks.Add(_barPaginaAnterior)
            _rpgPaginacion.ItemLinks.Add(_barPaginaSiguiente)
            _rpgPaginacion.ItemLinks.Add(_barResumenPagina)
            _rpgPaginacion.Name = "_rpgPaginacion"
            _rpgPaginacion.Text = "Paginación"
            ' 
            ' _rpgNavegacion
            ' 
            _rpgNavegacion.AllowTextClipping = False
            _rpgNavegacion.ItemLinks.Add(_btnPrimero)
            _rpgNavegacion.ItemLinks.Add(_btnAnterior)
            _rpgNavegacion.ItemLinks.Add(_btnSiguiente)
            _rpgNavegacion.ItemLinks.Add(_btnUltimo)
            _rpgNavegacion.Name = "_rpgNavegacion"
            _rpgNavegacion.Text = "Navegación"
            ' 
            ' _rpgVistaReporte
            ' 
            _rpgVistaReporte.ItemLinks.Add(_btnGuardarVista)
            _rpgVistaReporte.ItemLinks.Add(_btnRestaurarVista)
            _rpgVistaReporte.ItemLinks.Add(_btnExportar)
            _rpgVistaReporte.Name = "_rpgVistaReporte"
            _rpgVistaReporte.Text = "Vista y Reporte"
            ' 
            ' _statusBar
            ' 
            _statusBar.Location = New Point(0, 730)
            _statusBar.Name = "_statusBar"
            _statusBar.Ribbon = Ribbon
            _statusBar.Size = New Size(1495, 27)
            ' 
            ' Grid
            ' 
            Grid.Location = New Point(12, 12)
            Grid.MainView = View
            Grid.Name = "Grid"
            Grid.Size = New Size(1471, 580)
            Grid.TabIndex = 1
            Grid.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {View})
            ' 
            ' View
            ' 
            View.GridControl = Grid
            View.Name = "View"
            View.OptionsFind.ShowSearchNavButtons = False
            ' 
            ' MainLayout
            ' 
            MainLayout.Controls.Add(Grid)
            MainLayout.Dock = DockStyle.Fill
            MainLayout.Location = New Point(0, 126)
            MainLayout.Name = "MainLayout"
            MainLayout.Root = _layoutRoot
            MainLayout.Size = New Size(1495, 604)
            MainLayout.TabIndex = 2
            MainLayout.Text = "MainLayout"
            ' 
            ' _layoutRoot
            ' 
            _layoutRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            _layoutRoot.GroupBordersVisible = False
            _layoutRoot.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {_layoutGridItem})
            _layoutRoot.Name = "_layoutRoot"
            _layoutRoot.Size = New Size(1495, 604)
            _layoutRoot.TextVisible = False
            ' 
            ' _layoutGridItem
            ' 
            _layoutGridItem.Control = Grid
            _layoutGridItem.Location = New Point(0, 0)
            _layoutGridItem.Name = "_layoutGridItem"
            _layoutGridItem.Size = New Size(1475, 584)
            _layoutGridItem.TextVisible = False
            ' 
            ' TxtFiltro
            ' 
            TxtFiltro.Location = New Point(0, 0)
            TxtFiltro.Name = "TxtFiltro"
            TxtFiltro.Size = New Size(100, 20)
            TxtFiltro.TabIndex = 0
            ' 
            ' CmbCampoFiltro
            ' 
            CmbCampoFiltro.Location = New Point(0, 0)
            CmbCampoFiltro.Name = "CmbCampoFiltro"
            CmbCampoFiltro.Size = New Size(100, 20)
            CmbCampoFiltro.TabIndex = 0
            ' 
            ' SpnTamanoPagina
            ' 
            SpnTamanoPagina.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
            SpnTamanoPagina.Location = New Point(0, 0)
            SpnTamanoPagina.Name = "SpnTamanoPagina"
            SpnTamanoPagina.Size = New Size(100, 20)
            SpnTamanoPagina.TabIndex = 0
            ' 
            ' BaseSearchForm
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            ClientSize = New Size(1495, 757)
            Controls.Add(MainLayout)
            Controls.Add(_statusBar)
            Controls.Add(Ribbon)
            Name = "BaseSearchForm"
            Text = "Búsqueda"
            CType(Ribbon, ComponentModel.ISupportInitialize).EndInit()
            CType(_repoCampoFiltro, ComponentModel.ISupportInitialize).EndInit()
            CType(_repoBusqueda, ComponentModel.ISupportInitialize).EndInit()
            CType(_repoTamanoPagina, ComponentModel.ISupportInitialize).EndInit()
            CType(Grid, ComponentModel.ISupportInitialize).EndInit()
            CType(View, ComponentModel.ISupportInitialize).EndInit()
            CType(MainLayout, ComponentModel.ISupportInitialize).EndInit()
            MainLayout.ResumeLayout(False)
            CType(_layoutRoot, ComponentModel.ISupportInitialize).EndInit()
            CType(_layoutGridItem, ComponentModel.ISupportInitialize).EndInit()
            CType(TxtFiltro.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(CmbCampoFiltro.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(SpnTamanoPagina.Properties, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
            PerformLayout()

        End Sub
    End Class
End Namespace
