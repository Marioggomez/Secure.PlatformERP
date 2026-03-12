Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Tercero
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Terceros
    ''' <summary>
    ''' Pantalla de busqueda para tercero.tercero_rol.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmTerceroRolBuscar
        Inherits BaseSearchForm

        Public Sub New()
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            MyBase.New(apiClient, sessionContext)
        End Sub

        Protected Overrides Function BuildEndpoint() As String
            Return "api/v1/tercero/tercero_rol"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Roles de Tercero - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_tercero_rol", "Id").Width = 80
            gridView.Columns.AddVisible("id_tercero", "Id tercero").Width = 100
            gridView.Columns.AddVisible("id_rol_tercero", "Id rol").Width = 90
            gridView.Columns.AddVisible("id_empresa", "Id empresa").Width = 90
            gridView.Columns.AddVisible("activo", "Activo").Width = 90
        End Sub

        Protected Overrides Function BuildSampleData() As DataTable
            Dim table = CreateTable()

            Try
                If ApiClient IsNot Nothing Then
                    Dim items = ApiClient.GetAsync(Of List(Of TerceroRolDto))(BuildEndpoint()).GetAwaiter().GetResult()
                    If items IsNot Nothing Then
                        For Each item In items
                            table.Rows.Add(
                                item.IdTerceroRol.GetValueOrDefault(0),
                                item.IdTercero.GetValueOrDefault(0),
                                item.IdRolTercero.GetValueOrDefault(0),
                                item.IdEmpresa.GetValueOrDefault(0),
                                item.Activo.GetValueOrDefault(False))
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
            table.Columns.Add("id_tercero_rol", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_rol_tercero", GetType(Integer))
            table.Columns.Add("id_empresa", GetType(Long))
            table.Columns.Add("activo", GetType(Boolean))
            Return table
        End Function
    End Class
End Namespace
