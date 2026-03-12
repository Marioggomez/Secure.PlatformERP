Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Tercero
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Terceros
    ''' <summary>
    ''' Pantalla de busqueda para catalogo tercero.tipo_persona.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmTipoPersonaBuscar
        Inherits BaseSearchForm

        Public Sub New()
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            MyBase.New(apiClient, sessionContext)
        End Sub

        Protected Overrides Function BuildEndpoint() As String
            Return "api/v1/tercero/tipo_persona"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Tipos de Persona - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_tipo_persona", "Id").Width = 90
            gridView.Columns.AddVisible("codigo", "Codigo").Width = 140
            gridView.Columns.AddVisible("nombre", "Nombre").Width = 260
        End Sub

        Protected Overrides Function BuildSampleData() As DataTable
            Dim table = CreateTable()

            Try
                If ApiClient IsNot Nothing Then
                    Dim items = ApiClient.GetAsync(Of List(Of TipoPersonaDto))(BuildEndpoint()).GetAwaiter().GetResult()
                    If items IsNot Nothing Then
                        For Each item In items
                            table.Rows.Add(item.IdTipoPersona.GetValueOrDefault(0), item.Codigo, item.Nombre)
                        Next
                        Return table
                    End If
                End If
            Catch
                ' Fallback a data local de diseno.
            End Try

            table.Rows.Add(1, "NATURAL", "Persona Natural")
            table.Rows.Add(2, "JURIDICA", "Persona Juridica")
            Return table
        End Function

        Private Shared Function CreateTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_tipo_persona", GetType(Integer))
            table.Columns.Add("codigo", GetType(String))
            table.Columns.Add("nombre", GetType(String))
            Return table
        End Function
    End Class
End Namespace
