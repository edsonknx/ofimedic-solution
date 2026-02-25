Imports System.Data.Entity
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
            ' Traer todos los datos
            Dim tareaAlbumes = client.GetStringAsync("https://jsonplaceholder.typicode.com/albums")
            Dim tareaFotos = client.GetStringAsync("https://jsonplaceholder.typicode.com/photos")

            Await Task.WhenAll(tareaAlbumes, tareaFotos)

            Dim albumsJson = Await tareaAlbumes
            Dim photosJson = Await tareaFotos

            ' Deserializar
            Dim albums = JsonConvert.DeserializeObject(Of List(Of Album))(albumsJson)
            Dim photos = JsonConvert.DeserializeObject(Of List(Of Photo))(photosJson)

            ' Deshabilitar para optimizar
            db.Configuration.AutoDetectChangesEnabled = False
            db.Configuration.ValidateOnSaveEnabled = False

            ' Guardar albumes
            Dim albumsCount = 0
            For Each a In albums
                Dim existe = Await db.Albums.FindAsync(a.Id)
                If existe Is Nothing Then
                    db.Albums.Add(a)
                    albumsCount += 1
                Else
                    ' Actualizar propiedades (solo si cambiaron)
                    existe.UserId = a.UserId
                    existe.Title = a.Title
                End If
            Next

            ' Guardar fotos de forma óptima
            ' Obtener IDs existentes 
            Dim idsExistentes = New HashSet(Of Integer)(
                Await db.Photos.Select(Function(p) p.Id).ToListAsync()
            )

            ' Separar fotos nuevas y existentes
            Dim fotosNuevas = New List(Of Photo)()
            Dim fotosExistentes = New List(Of Photo)()

            For Each p In photos
                If idsExistentes.Contains(p.Id) Then
                    fotosExistentes.Add(p)
                Else
                    fotosNuevas.Add(p)
                End If
            Next

            ' Agregar solo fotos nuevas
            db.Photos.AddRange(fotosNuevas)

            ' Actualizar existentes 
            For Each p In fotosExistentes
                Dim existe = Await db.Photos.FindAsync(p.Id)
                If existe IsNot Nothing Then
                    existe.AlbumId = p.AlbumId
                    existe.Title = p.Title
                    existe.Url = p.Url
                    existe.ThumbnailUrl = p.ThumbnailUrl
                End If
            Next

            ' Guardar
            Await db.SaveChangesAsync()

            ' Restaurar configuración
            db.Configuration.AutoDetectChangesEnabled = True
            db.Configuration.ValidateOnSaveEnabled = True

            Return Ok("OK")

        Catch ex As Exception
            Return Content(HttpStatusCode.InternalServerError, "ERROR")
        End Try
    End Function
End Class