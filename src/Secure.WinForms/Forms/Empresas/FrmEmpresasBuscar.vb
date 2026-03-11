Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Common
Imports Secure.Platform.Contracts.Dtos.Organizacion
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Empresas
    ''' <summary>
    ''' Formulario de busqueda de empresas.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmEmpresasBuscar
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
            Return "api/v1/organizacion/empresa"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Empresas - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_empresa", "Id").Width = 80
            gridView.Columns.AddVisible("codigo", "Codigo").Width = 140
            gridView.Columns.AddVisible("nombre", "Nombre comercial").Width = 240
            gridView.Columns.AddVisible("nombre_legal", "Razon social").Width = 260
            gridView.Columns.AddVisible("identificacion_fiscal", "NIT").Width = 120
            gridView.Columns.AddVisible("estado", "Estado").Width = 100
        End Sub

        Protected Overrides Function CargarPaginaServidor(ByVal request As SearchPageRequest) As SearchPageResult
            Dim endpoint = BuildPagedEndpoint(request)
            Dim response = ApiClient.GetAsync(Of PaginacionResultadoDto(Of EmpresaListadoDto))(endpoint).GetAwaiter().GetResult()
            If response Is Nothing Then
                Throw New InvalidOperationException("La API no devolvio respuesta de empresas paginadas.")
            End If

            Dim table = CreateTable()
            For Each item In response.Items
                table.Rows.Add(item.IdEmpresa, item.Codigo, item.Nombre, item.NombreLegal, item.IdentificacionFiscal, item.Estado)
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
            Dim estados = New String() {"ACTIVA", "SUSPENDIDA", "EN_REVISION"}
            For index = 1 To 120
                Dim codigo = $"EMP-{index:0000}"
                Dim nombre = $"Empresa {index:0000}"
                Dim nombreLegal = $"{nombre} S.A."
                Dim nit = $"GT-NIT-{index:0000}"
                Dim estado = estados(index Mod estados.Length)
                table.Rows.Add(CLng(index), codigo, nombre, nombreLegal, nit, estado)
            Next

            Return table
        End Function

        Private Shared Function CreateTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_empresa", GetType(Long))
            table.Columns.Add("codigo", GetType(String))
            table.Columns.Add("nombre", GetType(String))
            table.Columns.Add("nombre_legal", GetType(String))
            table.Columns.Add("identificacion_fiscal", GetType(String))
            table.Columns.Add("estado", GetType(String))
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

            If request.IdTenant.HasValue Then
                query.Add($"idTenant={request.IdTenant.Value}")
            End If

            Return $"api/v1/organizacion/empresa/paginado?{String.Join("&", query)}"
        End Function
    End Class
End Namespace
