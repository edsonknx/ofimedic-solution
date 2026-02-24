Imports System.Web.Http
Imports Newtonsoft.Json
Imports System.Threading.Tasks
Imports System.Net.Http

Public Class SyncController
    Inherits ApiController

    Private db As New AppDbContext()
    Private client As New HttpClient()

    Public Async Function PostSync() As Task(Of IHttpActionResult)
        Try
            ' Sincronizar albums
            Dim albumsResponse = Await client.GetStringAsync("https://jsonplaceholder.typicode.com/albums")
            Dim albums = JsonConvert.DeserializeObject(Of List(Of Album))(albumsResponse)

            For Each a In albums
                Dim exists = db.Albums.Find(a.Id)
                If exists Is Nothing Then
                    db.Albums.Add(a)
                Else
                    exists.UserId = a.UserId
                    exists.Title = a.Title
                End If
            Next

            ' Sincronizar photos
            Dim photosResponse = Await client.GetStringAsync("https://jsonplaceholder.typicode.com/photos")
            Dim photos = JsonConvert.DeserializeObject(Of List(Of Photo))(photosResponse)

            For Each p In photos
                Dim exists = db.Photos.Find(p.Id)
                If exists Is Nothing Then
                    db.Photos.Add(p)
                Else
                    exists.AlbumId = p.AlbumId
                    exists.Title = p.Title
                    exists.Url = p.Url
                    exists.ThumbnailUrl = p.ThumbnailUrl
                End If
            Next

            db.SaveChanges()
            Return Ok(New With {.message = "Sincronización completada"})

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function
End Class