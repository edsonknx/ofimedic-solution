Imports System.Web.Http
Imports System.Data.Entity

Public Class AlbumsController
    Inherits ApiController

    Private db As New AppDbContext()

    ' GET api/albums
    Public Function GetValues(Optional title As String = Nothing) As IHttpActionResult
        Dim query = db.Albums.AsQueryable()

        If Not String.IsNullOrEmpty(title) Then
            query = query.Where(Function(a) a.Title.Contains(title))
        End If

        Return Ok(query.ToList())
    End Function

    ' GET api/albums/5
    Public Function GetValue(id As Integer) As IHttpActionResult
        Dim album = db.Albums.Find(id)
        If album Is Nothing Then
            Return NotFound()
        End If
        Return Ok(album)
    End Function

    ' POST api/albums
    Public Function PostValue(album As Album) As IHttpActionResult
        db.Albums.Add(album)
        db.SaveChanges()
        Return Ok(album)
    End Function

    ' PUT api/albums/5
    Public Function PutValue(id As Integer, album As Album) As IHttpActionResult
        If id <> album.Id Then Return BadRequest()
        db.Entry(album).State = EntityState.Modified
        db.SaveChanges()
        Return Ok(album)
    End Function

    ' DELETE api/albums/5
    Public Function DeleteValue(id As Integer) As IHttpActionResult
        Dim album = db.Albums.Find(id)
        If album Is Nothing Then Return NotFound()
        db.Albums.Remove(album)
        db.SaveChanges()
        Return StatusCode(HttpStatusCode.NoContent)
    End Function
End Class