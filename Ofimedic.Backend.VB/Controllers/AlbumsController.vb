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

            ' Proyectamos solo las propiedades necesarias, sin incluir las fotos
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

            ' También proyectamos aquí si querés evitar problemas
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
End Class