Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Json
Imports System.Linq
Imports System.Text
Imports System.Text.Json

Namespace Infrastructure
    ''' <summary>
    ''' Cliente API para comunicacion con Secure.Api.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class ApiClient
        Private ReadOnly _httpClient As HttpClient
        Private Shared ReadOnly _jsonOptions As JsonSerializerOptions = New JsonSerializerOptions With {
            .PropertyNameCaseInsensitive = True
        }

        Public Sub New(ByVal baseAddress As String)
            _httpClient = New HttpClient() With {
                .BaseAddress = New Uri(baseAddress, UriKind.Absolute),
                .Timeout = TimeSpan.FromSeconds(60)
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        End Sub

        Public ReadOnly Property BaseAddress As String
            Get
                Return If(_httpClient.BaseAddress IsNot Nothing, _httpClient.BaseAddress.ToString(), String.Empty)
            End Get
        End Property

        Public Sub SetBearerToken(ByVal token As String)
            _httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", token)
        End Sub

        Public Async Function GetAsync(Of TResponse)(ByVal endpoint As String) As Task(Of TResponse)
            Dim normalizedEndpoint = NormalizeEndpoint(endpoint, HttpMethod.Get)
            Dim request = New HttpRequestMessage(HttpMethod.Get, normalizedEndpoint)
            Return Await SendAsync(Of TResponse)(request).ConfigureAwait(False)
        End Function

        Public Async Function PostAsync(Of TRequest, TResponse)(ByVal endpoint As String, ByVal payload As TRequest) As Task(Of TResponse)
            Dim normalizedEndpoint = NormalizeEndpoint(endpoint, HttpMethod.Post)
            Dim request = New HttpRequestMessage(HttpMethod.Post, normalizedEndpoint) With {
                .Content = JsonContent.Create(payload)
            }
            Return Await SendAsync(Of TResponse)(request).ConfigureAwait(False)
        End Function

        Public Async Function PutAsync(Of TRequest)(ByVal endpoint As String, ByVal payload As TRequest) As Task
            Dim normalizedEndpoint = NormalizeEndpoint(endpoint, HttpMethod.Put)
            Dim request = New HttpRequestMessage(HttpMethod.Put, normalizedEndpoint) With {
                .Content = JsonContent.Create(payload)
            }
            Await SendAsync(Of Object)(request).ConfigureAwait(False)
        End Function

        Public Async Function DeleteAsync(ByVal endpoint As String) As Task
            Dim normalizedEndpoint = NormalizeEndpoint(endpoint, HttpMethod.Delete)
            Dim request = New HttpRequestMessage(HttpMethod.Delete, normalizedEndpoint)
            Await SendAsync(Of Object)(request).ConfigureAwait(False)
        End Function

        Private Shared Function NormalizeEndpoint(ByVal endpoint As String, ByVal method As HttpMethod) As String
            If String.IsNullOrWhiteSpace(endpoint) Then Return endpoint

            Dim raw = endpoint.Trim()
            If raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) OrElse
               raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                Return raw
            End If

            Dim queryStart = raw.IndexOf("?"c)
            Dim pathPart = If(queryStart >= 0, raw.Substring(0, queryStart), raw)
            Dim queryPart = If(queryStart >= 0, raw.Substring(queryStart), String.Empty)

            Dim normalizedPath = pathPart.TrimStart("/"c)
            If Not normalizedPath.StartsWith("api/v1/", StringComparison.OrdinalIgnoreCase) Then
                Return raw
            End If

            Dim segments = normalizedPath.Split("/"c, StringSplitOptions.RemoveEmptyEntries)
            If segments.Length < 4 Then
                Return raw
            End If

            Dim actionSegment As String = If(segments.Length >= 5, segments(4), String.Empty)
            If IsExplicitAction(actionSegment) Then
                Return raw
            End If

            If method Is HttpMethod.Get AndAlso
               segments.Length = 5 AndAlso
               String.Equals(segments(4), "paginado", StringComparison.OrdinalIgnoreCase) Then
                segments(4) = "listar"
                ReDim Preserve segments(5)
                segments(5) = "paginado"
                Return String.Join("/", segments) & queryPart
            End If

            If method Is HttpMethod.Get Then
                If segments.Length = 4 AndAlso String.IsNullOrEmpty(queryPart) Then
                    Return String.Join("/", segments) & "/listar"
                End If

                If segments.Length = 5 AndAlso Not String.IsNullOrWhiteSpace(segments(4)) Then
                    Return String.Join("/", segments.Take(4)) & "/obtener/" & segments(4) & queryPart
                End If
            ElseIf method Is HttpMethod.Post Then
                If segments.Length = 4 Then
                    Return String.Join("/", segments) & "/crear" & queryPart
                End If
            ElseIf method Is HttpMethod.Put Then
                If segments.Length = 5 Then
                    Return String.Join("/", segments.Take(4)) & "/actualizar/" & segments(4) & queryPart
                End If
            ElseIf method Is HttpMethod.Delete Then
                If segments.Length = 5 Then
                    Return String.Join("/", segments.Take(4)) & "/desactivar/" & segments(4) & queryPart
                End If
            End If

            Return raw
        End Function

        Private Shared Function IsExplicitAction(ByVal segment As String) As Boolean
            If String.IsNullOrWhiteSpace(segment) Then Return False

            Return String.Equals(segment, "listar", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(segment, "obtener", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(segment, "crear", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(segment, "actualizar", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(segment, "desactivar", StringComparison.OrdinalIgnoreCase)
        End Function

        Private Async Function SendAsync(Of TResponse)(ByVal request As HttpRequestMessage) As Task(Of TResponse)
            request.Headers.Add("X-Correlation-Id", Guid.NewGuid().ToString())

            Using response = Await _httpClient.SendAsync(request).ConfigureAwait(False)
                Dim responseText = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)

                If Not response.IsSuccessStatusCode Then
                    Throw New ApiClientException(
                        $"API devolvio {(CInt(response.StatusCode)).ToString()} {response.StatusCode}.",
                        response.StatusCode,
                        responseText)
                End If

                If String.IsNullOrWhiteSpace(responseText) Then
                    Return Nothing
                End If

                Dim result = JsonSerializer.Deserialize(Of TResponse)(responseText, _jsonOptions)
                Return result
            End Using
        End Function
    End Class

    ''' <summary>
    ''' Excepcion de integracion HTTP con Secure.Api.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class ApiClientException
        Inherits Exception

        Public Sub New(ByVal message As String, ByVal statusCode As HttpStatusCode, ByVal responseBody As String)
            MyBase.New(message)
            Me.StatusCode = statusCode
            Me.ResponseBody = responseBody
        End Sub

        Public ReadOnly Property StatusCode As HttpStatusCode
        Public ReadOnly Property ResponseBody As String
    End Class
End Namespace
