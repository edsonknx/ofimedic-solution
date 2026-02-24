Imports System.Net
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Module ApiHelper
    Private client As New WebClient()

    Public Function GetAlbumsAsync(filtro As String) As Task(Of String)
        Try
            Dim url = "https://localhost:44386/api/albums"
            If Not String.IsNullOrEmpty(filtro) Then
                url &= "?title=" & Uri.EscapeDataString(filtro)
            End If
            Return Task.Run(Function() client.DownloadString(url))
        Catch ex As Exception
            Throw New Exception("Error al obtener álbumes: " & ex.Message)
        End Try
    End Function

    Public Function GetPhotosAsync(albumId As Integer) As Task(Of String)
        Try
            Dim url = $"https://localhost:44386/api/photos?albumId={albumId}"
            Return Task.Run(Function() client.DownloadString(url))
        Catch ex As Exception
            Throw New Exception("Error al obtener fotos: " & ex.Message)
        End Try
    End Function

    Public Function SyncDataAsync() As Task(Of String)
        Try
            Dim url = "https://localhost:44386/api/sync"
            Return Task.Run(Function() client.UploadString(url, "POST", ""))
        Catch ex As Exception
            Throw New Exception("Error al sincronizar: " & ex.Message)
        End Try
    End Function
End Module