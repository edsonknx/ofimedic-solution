Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports Newtonsoft.Json

Public Class SyncController
    Inherits ApiController

    Private db As New AppDbContext()
    Private client As New HttpClient()

    Public Async Function PostSync() As Task(Of IHttpActionResult)
        Try

            Dim albumsUrl = "https://jsonplaceholder.typicode.com/albums"
            Dim albumsJson = Await client.GetStringAsync(albumsUrl)
            Dim albums = JsonConvert.DeserializeObject(Of List(Of Album))(albumsJson)

            Dim albumsCount = 0
            For Each a In albums
                Dim existe = Await db.Albums.FindAsync(a.Id)
                If existe Is Nothing Then
                    db.Albums.Add(a)
                    albumsCount += 1
                Else
                    ' Actualizar si cambie
                    existe.UserId = a.UserId
                    existe.Title = a.Title
                End If
            Next
            Dim photosUrl = "https://jsonplaceholder.typicode.com/photos"
            Dim photosJson = Await client.GetStringAsync(photosUrl)
            Dim photos = JsonConvert.DeserializeObject(Of List(Of Photo))(photosJson)

            Dim photosCount = 0
            For Each p In photos
                Dim existe = Await db.Photos.FindAsync(p.Id)
                If existe Is Nothing Then
                    db.Photos.Add(p)
                    photosCount += 1
                Else
                    ' Actualizar si cambió
                    existe.AlbumId = p.AlbumId
                    existe.Title = p.Title
                    existe.Url = p.Url
                    existe.ThumbnailUrl = p.ThumbnailUrl
                End If
            Next

            Await db.SaveChangesAsync()

            Return Ok("OK")

        Catch ex As Exception
            Return Content(HttpStatusCode.InternalServerError, "ERROR")
        End Try
    End Function
End Class