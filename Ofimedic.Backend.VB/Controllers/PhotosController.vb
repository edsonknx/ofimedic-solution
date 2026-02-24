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

            ' Proyectamos solo las propiedades necesarias, sin incluir el álbum
            Dim fotos = query.Select(Function(p) New With {
                .Id = p.Id,
                .AlbumId = p.AlbumId,
                .Title = p.Title,
                .Url = p.Url,
                .ThumbnailUrl = p.ThumbnailUrl
            }).ToList()

            Return Ok(fotos)

        Catch ex As Exception
            Return Content(HttpStatusCode.InternalServerError, New With {
                .error = ex.Message,
                .innerError = If(ex.InnerException IsNot Nothing, ex.InnerException.Message, "")
            })
        End Try
    End Function

    ' GET api/photos/5
    Public Function GetValue(id As Integer) As IHttpActionResult
        Try
            Dim foto = db.Photos.Find(id)

            If foto Is Nothing Then
                Return NotFound()
            End If

            ' También proyectamos aquí
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
End Class