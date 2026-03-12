Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Json
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
            Dim request = New HttpRequestMessage(HttpMethod.Get, endpoint)
            Return Await SendAsync(Of TResponse)(request).ConfigureAwait(False)
        End Function

        Public Async Function PostAsync(Of TRequest, TResponse)(ByVal endpoint As String, ByVal payload As TRequest) As Task(Of TResponse)
            Dim request = New HttpRequestMessage(HttpMethod.Post, endpoint) With {
                .Content = JsonContent.Create(payload)
            }
            Return Await SendAsync(Of TResponse)(request).ConfigureAwait(False)
        End Function

        Public Async Function PutAsync(Of TRequest)(ByVal endpoint As String, ByVal payload As TRequest) As Task
            Dim request = New HttpRequestMessage(HttpMethod.Put, endpoint) With {
                .Content = JsonContent.Create(payload)
            }
            Await SendAsync(Of Object)(request).ConfigureAwait(False)
        End Function

        Public Async Function DeleteAsync(ByVal endpoint As String) As Task
            Dim request = New HttpRequestMessage(HttpMethod.Delete, endpoint)
            Await SendAsync(Of Object)(request).ConfigureAwait(False)
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
