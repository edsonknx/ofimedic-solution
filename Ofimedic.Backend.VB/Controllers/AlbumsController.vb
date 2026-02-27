Imports System.Data.Entity
Imports System.Net
Imports System.Web.Http

Public Class AlbumsController
    Inherits ApiController

    Private db As New AppDbContext()

    ' GET api/albums
    Public Function GetValues(Optional title As String = Nothing) As IHttpActionResult
        Try
            Dim query = db.Albums.AsQueryable()

            If Not String.IsNullOrEmpty(title) Then
                query = query.Where(Function(a) a.Title.Contains(title))
            End If

            ' Pasamos solo las propiedades necesarias, sin incluir las fotos
            Dim albums = query.Select(Function(a) New With {
                .Id = a.Id,
                .UserId = a.UserId,
                .Title = a.Title
            }).ToList()

            Return Ok(albums)

        Catch ex As Exception
            Return Content(HttpStatusCode.InternalServerError, New With {
                .error = ex.Message,
                .innerError = If(ex.InnerException IsNot Nothing, ex.InnerException.Message, "")
            })
        End Try
    End Function

    ' GET api/albums/5
    Public Function GetValue(id As Integer) As IHttpActionResult
        Try
            Dim album = db.Albums.Find(id)

            If album Is Nothing Then
                Return NotFound()
            End If

            ' Pasamos propiedades 
            Dim resultado = New With {
                .Id = album.Id,
                .UserId = album.UserId,
                .Title = album.Title
            }

            Return Ok(resultado)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ' POST: api/albums
    Public Function PostValue(album As Album) As IHttpActionResult
        Try
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            ' Verificar si ya existe
            Dim existe = db.Albums.Find(album.Id)
            If existe IsNot Nothing Then
                Return Conflict()  ' 409 Conflict
            End If

            db.Albums.Add(album)
            db.SaveChanges()

            Return Created(New Uri(Request.RequestUri.ToString() & "/" & album.Id), album)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ' PUT: api/albums/5
    Public Function PutValue(id As Integer, album As Album) As IHttpActionResult
        Try
            If id <> album.Id Then
                Return BadRequest("El ID no coincide")
            End If

            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim existe = db.Albums.Find(id)
            If existe Is Nothing Then
                Return NotFound()
            End If

            ' Actualizar propiedades
            existe.UserId = album.UserId
            existe.Title = album.Title

            db.Entry(existe).State = EntityState.Modified
            db.SaveChanges()

            Return StatusCode(HttpStatusCode.NoContent)  ' 204 No Content

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ' DELETE: api/albums/5
    Public Function DeleteValue(id As Integer) As IHttpActionResult
        Try
            Dim album = db.Albums.Find(id)
            If album Is Nothing Then
                Return NotFound()
            End If

            ' Primero eliminar fotos relacionadas (por FK)
            Dim fotos = db.Photos.Where(Function(p) p.AlbumId = id).ToList()
            db.Photos.RemoveRange(fotos)

            ' Eliminar álbum
            db.Albums.Remove(album)
            db.SaveChanges()

            Return StatusCode(HttpStatusCode.NoContent)  ' 204 No Content

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function
End Class