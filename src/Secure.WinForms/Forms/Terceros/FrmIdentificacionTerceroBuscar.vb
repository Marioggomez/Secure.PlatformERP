Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Tercero
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Terceros
    ''' <summary>
    ''' Pantalla de busqueda para tercero.identificacion_tercero.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmIdentificacionTerceroBuscar
        Inherits BaseSearchForm

        Public Sub New()
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            MyBase.New(apiClient, sessionContext)
        End Sub

        Protected Overrides Function BuildEndpoint() As String
            Return "api/v1/tercero/identificacion_tercero/listar"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Identificaciones de Tercero - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_identificacion_tercero", "Id").Width = 80
            gridView.Columns.AddVisible("id_tercero", "Id tercero").Width = 100
            gridView.Columns.AddVisible("id_tipo_identificacion", "Tipo").Width = 90
            gridView.Columns.AddVisible("numero_identificacion", "Numero").Width = 180
            gridView.Columns.AddVisible("fecha_emision", "Emision").Width = 120
            gridView.Columns.AddVisible("fecha_vencimiento", "Vencimiento").Width = 120
            gridView.Columns.AddVisible("principal", "Principal").Width = 90
        End Sub

        Protected Overrides Function BuildSampleData() As DataTable
            Dim table = CreateTable()

            Try
                If ApiClient IsNot Nothing Then
                    Dim items = ApiClient.GetAsync(Of List(Of IdentificacionTerceroDto))(BuildEndpoint()).GetAwaiter().GetResult()
                    If items IsNot Nothing Then
                        For Each item In items
                            table.Rows.Add(
                                item.IdIdentificacionTercero.GetValueOrDefault(0),
                                item.IdTercero.GetValueOrDefault(0),
                                item.IdTipoIdentificacion.GetValueOrDefault(0),
                                item.NumeroIdentificacion,
                                If(item.FechaEmision.HasValue, item.FechaEmision.Value.ToString("yyyy-MM-dd"), String.Empty),
                                If(item.FechaVencimiento.HasValue, item.FechaVencimiento.Value.ToString("yyyy-MM-dd"), String.Empty),
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

        Private Shared Function CreateTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_identificacion_tercero", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_tipo_identificacion", GetType(Integer))
            table.Columns.Add("numero_identificacion", GetType(String))
            table.Columns.Add("fecha_emision", GetType(String))
            table.Columns.Add("fecha_vencimiento", GetType(String))
            table.Columns.Add("principal", GetType(Boolean))
            Return table
        End Function
    End Class
End Namespace

