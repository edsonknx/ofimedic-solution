Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema

Public Class AppDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=OfimedicDB")
    End Sub

    Public Property Albums As DbSet(Of Album)
    Public Property Photos As DbSet(Of Photo)

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)

        ' Mapear la entidad Album a la tabla "Albums"
        modelBuilder.Entity(Of Album)() _
            .ToTable("Albums")

        ' Configurar la clave primaria
        modelBuilder.Entity(Of Album)() _
            .HasKey(Function(a) a.Id)

        ' Configurar la columna Id
        modelBuilder.Entity(Of Album)() _
            .Property(Function(a) a.Id) _
            .HasColumnName("Id") _
            .IsRequired() _
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None) '

        ' Configurar la columna UserId
        modelBuilder.Entity(Of Album)() _
            .Property(Function(a) a.UserId) _
            .HasColumnName("UserId") _
            .IsRequired()

        ' Configurar la columna Title
        modelBuilder.Entity(Of Album)() _
            .Property(Function(a) a.Title) _
            .HasColumnName("Title") _
            .IsRequired() _
            .HasMaxLength(200)

        ' Mapear la entidad Photo a la tabla "Photos"
        modelBuilder.Entity(Of Photo)() _
            .ToTable("Photos")

        ' Configurar la clave primaria
        modelBuilder.Entity(Of Photo)() _
            .HasKey(Function(p) p.Id)

        ' Configurar la columna Id
        modelBuilder.Entity(Of Photo)() _
            .Property(Function(p) p.Id) _
            .HasColumnName("Id") _
            .IsRequired() _
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None) ' ← TAMPOCO ES IDENTITY

        ' Configurar la columna AlbumId (clave foránea)
        modelBuilder.Entity(Of Photo)() _
            .Property(Function(p) p.AlbumId) _
            .HasColumnName("AlbumId") _
            .IsRequired()

        ' Configurar la columna Title
        modelBuilder.Entity(Of Photo)() _
            .Property(Function(p) p.Title) _
            .HasColumnName("Title") _
            .IsRequired() _
            .HasMaxLength(200)

        ' Configurar la columna Url
        modelBuilder.Entity(Of Photo)() _
            .Property(Function(p) p.Url) _
            .HasColumnName("Url") _
            .IsOptional() _
            .HasMaxLength(500)

        ' Configurar la columna ThumbnailUrl
        modelBuilder.Entity(Of Photo)() _
            .Property(Function(p) p.ThumbnailUrl) _
            .HasColumnName("ThumbnailUrl") _
            .IsOptional() _
            .HasMaxLength(500)


        ' Un Album tiene muchas Photos
        ' Una Photo pertenece a un Album
        modelBuilder.Entity(Of Photo)() _
            .HasRequired(Function(p) p.Album) _
            .WithMany(Function(a) a.Photos) _
            .HasForeignKey(Function(p) p.AlbumId)

        MyBase.OnModelCreating(modelBuilder)
    End Sub
End Class