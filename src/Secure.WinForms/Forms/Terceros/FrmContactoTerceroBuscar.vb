Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Tercero
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Terceros
    ''' <summary>
    ''' Pantalla de busqueda para tercero.contacto_tercero.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmContactoTerceroBuscar
        Inherits BaseSearchForm

        Public Sub New()
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            MyBase.New(apiClient, sessionContext)
        End Sub

        Protected Overrides Function BuildEndpoint() As String
            Return "api/v1/tercero/contacto_tercero/listar"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Contactos de Tercero - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_contacto_tercero", "Id").Width = 80
            gridView.Columns.AddVisible("id_tercero", "Id tercero").Width = 100
            gridView.Columns.AddVisible("id_tipo_contacto", "Tipo").Width = 90
            gridView.Columns.AddVisible("valor", "Valor").Width = 260
            gridView.Columns.AddVisible("principal", "Principal").Width = 90
        End Sub

        Protected Overrides Function BuildSampleData() As DataTable
            Dim table = CreateTable()

            Try
                If ApiClient IsNot Nothing Then
                    Dim items = ApiClient.GetAsync(Of List(Of ContactoTerceroDto))(BuildEndpoint()).GetAwaiter().GetResult()
                    If items IsNot Nothing Then
                        For Each item In items
                            table.Rows.Add(
                                item.IdContactoTercero.GetValueOrDefault(0),
                                item.IdTercero.GetValueOrDefault(0),
                                item.IdTipoContacto.GetValueOrDefault(0),
                                item.Valor,
                                item.Principal.GetValueOrDefault(False))
                        Next
                        Return table
                    End If
                End If
            Catch
                ' Fallback a data local de diseno.
            End Try

            Return table
        End Function

        Private Sub InitializeComponent()
            Root = New DevExpress.XtraLayout.LayoutControlGroup()
            LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
            LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
            LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
            CType(Ribbon, ComponentModel.ISupportInitialize).BeginInit()
            CType(MainLayout, ComponentModel.ISupportInitialize).BeginInit()
            CType(TxtFiltro.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(CmbCampoFiltro.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(SpnTamanoPagina.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(Root, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlGroup1, ComponentModel.ISupportInitialize).BeginInit()
            CType(LayoutControlItem2, ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' Ribbon
            ' 
            Ribbon.ExpandCollapseItem.Id = 0
            ' 
            ' TxtFiltro
            ' 
            ' 
            ' CmbCampoFiltro
            ' 
            ' 
            ' SpnTamanoPagina
            ' 
            SpnTamanoPagina.Properties.MaskSettings.Set("mask", "N00")
            ' 
            ' Root
            ' 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            Root.GroupBordersVisible = False
            Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {LayoutControlItem1})
            Root.Location = New Point(0, 0)
            Root.Name = "Root"
            Root.Size = New Size(1218, 575)
            ' 
            ' LayoutControlItem1
            ' 
            LayoutControlItem1.Control = Grid
            LayoutControlItem1.Location = New Point(0, 0)
            LayoutControlItem1.Name = "LayoutControlItem1"
            LayoutControlItem1.Size = New Size(1198, 555)
            LayoutControlItem1.TextVisible = False
            ' 
            ' LayoutControlGroup1
            ' 
            LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            LayoutControlGroup1.GroupBordersVisible = False
            LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {LayoutControlItem2})
            LayoutControlGroup1.Name = "LayoutControlGroup1"
            LayoutControlGroup1.Size = New Size(1218, 575)
            ' 
            ' LayoutControlItem2
            ' 
            LayoutControlItem2.Control = Grid
            LayoutControlItem2.Location = New Point(0, 0)
            LayoutControlItem2.Name = "LayoutControlItem2"
            LayoutControlItem2.Size = New Size(1198, 555)
            LayoutControlItem2.TextVisible = False
            ' 
            ' FrmContactoTerceroBuscar
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            ClientSize = New Size(1218, 728)
            Name = "FrmContactoTerceroBuscar"
            Text = "Búsqueda"
            CType(Ribbon, ComponentModel.ISupportInitialize).EndInit()
            CType(MainLayout, ComponentModel.ISupportInitialize).EndInit()
            CType(TxtFiltro.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(CmbCampoFiltro.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(SpnTamanoPagina.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(Root, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem1, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlGroup1, ComponentModel.ISupportInitialize).EndInit()
            CType(LayoutControlItem2, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
            PerformLayout()

        End Sub

        Private Shared Function CreateTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_contacto_tercero", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_tipo_contacto", GetType(Integer))
            table.Columns.Add("valor", GetType(String))
            table.Columns.Add("principal", GetType(Boolean))
            Return table
        End Function

        Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    End Class
End Namespace

