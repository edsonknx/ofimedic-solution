Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Class ApiHelper
    Private Shared client As New HttpClient()
    Private Const BaseUrl As String = "https://localhost:44386"

    Shared Sub New()
        client.Timeout = TimeSpan.FromMinutes(3)
    End Sub

    Public Shared Async Function GetAlbumsAsync(filtro As String) As Task(Of String)
        Try
            Dim url = $"{BaseUrl}/api/albums"
            If Not String.IsNullOrEmpty(filtro) Then
                url &= "?title=" & Uri.EscapeDataString(filtro)
            End If

            Dim response = Await client.GetAsync(url)
            response.EnsureSuccessStatusCode()
            Return Await response.Content.ReadAsStringAsync()

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function


    Public Shared Async Function GetPhotosAsync(albumId As Integer, Optional filtro As String = Nothing) As Task(Of String)
        Try
            Dim url = $"{BaseUrl}/api/photos?albumId={albumId}"
            If Not String.IsNullOrEmpty(filtro) Then
                url &= "&title=" & Uri.EscapeDataString(filtro)
            End If

            Dim response = Await client.GetAsync(url)
            response.EnsureSuccessStatusCode()
            Return Await response.Content.ReadAsStringAsync()

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Shared Async Function SyncDataAsync() As Task(Of String)
        Try
            Dim url = $"{BaseUrl}/api/sync"
            Dim content = New StringContent("", Encoding.UTF8, "application/json")

            Dim response = Await client.PostAsync(url, content)

            Dim responseBody = Await response.Content.ReadAsStringAsync()

            Return responseBody.Trim()

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Class