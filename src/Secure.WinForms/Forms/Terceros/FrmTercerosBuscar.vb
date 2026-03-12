Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Common
Imports Secure.Platform.Contracts.Dtos.Tercero
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Terceros
    ''' <summary>
    ''' Pantalla de busqueda principal para terceros con paginacion server-side.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmTercerosBuscar
        Inherits BaseSearchForm

        Public Sub New()
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            MyBase.New(apiClient, sessionContext)
        End Sub

        Protected Overrides ReadOnly Property UsaPaginacionServidor As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides Function BuildEndpoint() As String
            Return "api/v1/tercero/tercero"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Terceros - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_tercero", "Id").Width = 80
            gridView.Columns.AddVisible("codigo", "Codigo").Width = 120
            gridView.Columns.AddVisible("tipo_persona", "Tipo persona").Width = 150
            gridView.Columns.AddVisible("nombre_principal", "Nombre principal").Width = 260
            gridView.Columns.AddVisible("activo", "Activo").Width = 80
            gridView.Columns.AddVisible("creado_utc", "Creado UTC").Width = 160
        End Sub

        Protected Overrides Function CargarPaginaServidor(ByVal request As SearchPageRequest) As SearchPageResult
            Dim endpoint = BuildPagedEndpoint(request)
            Dim response = ApiClient.GetAsync(Of PaginacionResultadoDto(Of TerceroListadoDto))(endpoint).GetAwaiter().GetResult()
            If response Is Nothing Then
                Throw New InvalidOperationException("La API no devolvio respuesta de terceros paginados.")
            End If

            Dim table = CreateTable()
            For Each item In response.Items
                table.Rows.Add(
                    item.IdTercero,
                    item.Codigo,
                    item.TipoPersona,
                    item.NombrePrincipal,
                    item.Activo,
                    item.CreadoUtc.ToString("yyyy-MM-dd HH:mm"))
            Next

            Return New SearchPageResult With {
                .Page = response.Page,
                .Size = response.Size,
                .Total = response.Total,
                .Data = table
            }
        End Function

        Protected Overrides Function BuildSampleData() As DataTable
            Dim table = CreateTable()
            For index = 1 To 80
                table.Rows.Add(index, $"T-{index:0000}", If(index Mod 2 = 0, "Juridica", "Natural"), $"Tercero {index:0000}", (index Mod 3 <> 0), DateTime.UtcNow.AddDays(-index).ToString("yyyy-MM-dd HH:mm"))
            Next
            Return table
        End Function

        Private Shared Function CreateTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("codigo", GetType(String))
            table.Columns.Add("tipo_persona", GetType(String))
            table.Columns.Add("nombre_principal", GetType(String))
            table.Columns.Add("activo", GetType(Boolean))
            table.Columns.Add("creado_utc", GetType(String))
            Return table
        End Function

        Private Function BuildPagedEndpoint(ByVal request As SearchPageRequest) As String
            Dim query As New List(Of String) From {
                $"page={request.Page}",
                $"size={request.Size}",
                $"sortBy={Uri.EscapeDataString(If(request.SortBy, String.Empty))}",
                $"sortDirection={Uri.EscapeDataString(If(request.SortDirection, "ASC"))}",
                $"filter={Uri.EscapeDataString(If(request.Filter, String.Empty))}",
                $"filterField={Uri.EscapeDataString(If(request.FilterField, String.Empty))}"
            }

            Return $"api/v1/tercero/tercero/paginado?{String.Join("&", query)}"
        End Function
    End Class
End Namespace
