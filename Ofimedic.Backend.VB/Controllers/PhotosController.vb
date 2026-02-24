Imports System.Web.Http
Imports System.Data.Entity

Public Class PhotosController
    Inherits ApiController

    Private db As New AppDbContext()

    ' GET api/photos?albumId=1&title=xxx
    Public Function GetValues(Optional albumId As Integer? = Nothing, Optional title As String = Nothing) As IHttpActionResult
        Dim query = db.Photos.AsQueryable()

        If albumId.HasValue Then
            query = query.Where(Function(p) p.AlbumId = albumId.Value)
        End If

        If Not String.IsNullOrEmpty(title) Then
            query = query.Where(Function(p) p.Title.Contains(title))
        End If

        Return Ok(query.ToList())
    End Function

    ' GET api/photos/5
    Public Function GetValue(id As Integer) As IHttpActionResult
        Dim photo = db.Photos.Find(id)
        If photo Is Nothing Then Return NotFound()
        Return Ok(photo)
    End Function

    ' POST api/photos
    Public Function PostValue(photo As Photo) As IHttpActionResult
        db.Photos.Add(photo)
        db.SaveChanges()
        Return Ok(photo)
    End Function

    ' PUT api/photos/5
    Public Function PutValue(id As Integer, photo As Photo) As IHttpActionResult
        If id <> photo.Id Then Return BadRequest()
        db.Entry(photo).State = EntityState.Modified
        db.SaveChanges()
        Return Ok(photo)
    End Function

    ' DELETE api/photos/5
    Public Function DeleteValue(id As Integer) As IHttpActionResult
        Dim photo = db.Photos.Find(id)
        If photo Is Nothing Then Return NotFound()
        db.Photos.Remove(photo)
        db.SaveChanges()
        Return StatusCode(HttpStatusCode.NoContent)
    End Function
End Class