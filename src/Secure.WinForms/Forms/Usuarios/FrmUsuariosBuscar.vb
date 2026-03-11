Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Common
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Usuarios
    ''' <summary>
    ''' Formulario de busqueda de usuarios.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmUsuariosBuscar
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
            Return "api/v1/seguridad/usuario"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Usuarios - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_usuario", "Id").Width = 80
            gridView.Columns.AddVisible("login_principal", "Login").Width = 160
            gridView.Columns.AddVisible("nombre_mostrar", "Nombre").Width = 220
            gridView.Columns.AddVisible("correo_electronico", "Correo").Width = 220
            gridView.Columns.AddVisible("estado", "Estado").Width = 120
            gridView.Columns.AddVisible("ultimo_acceso", "Ultimo acceso").Width = 160
        End Sub

        Protected Overrides Function CargarPaginaServidor(ByVal request As SearchPageRequest) As SearchPageResult
            Dim endpoint = BuildPagedEndpoint(request)
            Dim response = ApiClient.GetAsync(Of PaginacionResultadoDto(Of UsuarioListadoDto))(endpoint).GetAwaiter().GetResult()
            If response Is Nothing Then
                Throw New InvalidOperationException("La API no devolvio respuesta de usuarios paginados.")
            End If

            Dim table = CreateTable()
            For Each item In response.Items
                table.Rows.Add(
                    item.IdUsuario,
                    item.LoginPrincipal,
                    item.NombreMostrar,
                    item.CorreoElectronico,
                    item.Estado,
                    If(item.UltimoAccesoUtc.HasValue, item.UltimoAccesoUtc.Value.ToString("yyyy-MM-dd HH:mm"), String.Empty))
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
            Dim estados = New String() {"ACTIVO", "BLOQUEADO", "INACTIVO"}
            For index = 1 To 180
                Dim login = $"usuario{index:000}"
                Dim nombre = $"Usuario {index:000}"
                Dim correo = $"{login}@seed.local"
                Dim estado = estados(index Mod estados.Length)
                Dim ultimoAcceso = DateTime.Now.AddMinutes(-(index * 7)).ToString("yyyy-MM-dd HH:mm")
                table.Rows.Add(CLng(index), login, nombre, correo, estado, ultimoAcceso)
            Next

            Return table
        End Function

        Private Shared Function CreateTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_usuario", GetType(Long))
            table.Columns.Add("login_principal", GetType(String))
            table.Columns.Add("nombre_mostrar", GetType(String))
            table.Columns.Add("correo_electronico", GetType(String))
            table.Columns.Add("estado", GetType(String))
            table.Columns.Add("ultimo_acceso", GetType(String))
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

            Return $"api/v1/seguridad/usuario/paginado?{String.Join("&", query)}"
        End Function
    End Class
End Namespace
