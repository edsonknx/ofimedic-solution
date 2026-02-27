Imports System.Data.Entity
Imports System.Net
Imports System.Web.Http

Public Class PhotosController
    Inherits ApiController

    Private db As New AppDbContext()

    ' GET api/photos?albumId=1&title=xxx
    Public Function GetValues(Optional albumId As Integer? = Nothing, Optional title As String = Nothing) As IHttpActionResult
        Try
            Dim query = db.Photos.AsQueryable()

            If albumId.HasValue Then
                query = query.Where(Function(p) p.AlbumId = albumId.Value)
            End If

            If Not String.IsNullOrEmpty(title) Then
                query = query.Where(Function(p) p.Title.Contains(title))
            End If

            ' Pasamos solo las propiedades necesarias, sin incluir el álbum
            Dim fotos = query.Select(Function(p) New With {
                .Id = p.Id,
                .AlbumId = p.AlbumId,
                .Title = p.Title,
                .Url = p.Url,
                .ThumbnailUrl = p.ThumbnailUrl
            }).ToList()

            Return Ok(fotos)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ' GET api/photos/5
    Public Function GetValue(id As Integer) As IHttpActionResult
        Try
            Dim foto = db.Photos.Find(id)

            If foto Is Nothing Then
                Return NotFound()
            End If

            ' Pasamos propiedades
            Dim resultado = New With {
                .Id = foto.Id,
                .AlbumId = foto.AlbumId,
                .Title = foto.Title,
                .Url = foto.Url,
                .ThumbnailUrl = foto.ThumbnailUrl
            }

            Return Ok(resultado)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ' POST: api/photos
    Public Function PostValue(foto As Photo) As IHttpActionResult
        Try
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            ' Verificar que el álbum existe
            Dim album = db.Albums.Find(foto.AlbumId)
            If album Is Nothing Then
                Return BadRequest("El AlbumId especificado no existe")
            End If

            ' Verificar si la foto ya existe
            Dim existe = db.Photos.Find(foto.Id)
            If existe IsNot Nothing Then
                Return Conflict()
            End If

            db.Photos.Add(foto)
            db.SaveChanges()

            Return Created(New Uri(Request.RequestUri.ToString() & "/" & foto.Id), foto)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ' PUT: api/photos/5
    Public Function PutValue(id As Integer, foto As Photo) As IHttpActionResult
        Try
            If id <> foto.Id Then
                Return BadRequest("El ID no coincide")
            End If

            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim existe = db.Photos.Find(id)
            If existe Is Nothing Then
                Return NotFound()
            End If

            ' Verificar que AlbumId existe
            If existe.AlbumId <> foto.AlbumId Then
                Dim album = db.Albums.Find(foto.AlbumId)
                If album Is Nothing Then
                    Return BadRequest("El AlbumId especificado no existe")
                End If
            End If

            ' Actualizar propiedades
            existe.AlbumId = foto.AlbumId
            existe.Title = foto.Title
            existe.Url = foto.Url
            existe.ThumbnailUrl = foto.ThumbnailUrl

            db.Entry(existe).State = EntityState.Modified
            db.SaveChanges()

            Return StatusCode(HttpStatusCode.NoContent)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function

    ' DELETE: api/photos/5
    Public Function DeleteValue(id As Integer) As IHttpActionResult
        Try
            Dim foto = db.Photos.Find(id)
            If foto Is Nothing Then
                Return NotFound()
            End If

            db.Photos.Remove(foto)
            db.SaveChanges()

            Return StatusCode(HttpStatusCode.NoContent)

        Catch ex As Exception
            Return InternalServerError(ex)
        End Try
    End Function
End Class