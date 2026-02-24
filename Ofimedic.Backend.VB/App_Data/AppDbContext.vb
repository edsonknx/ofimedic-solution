Imports System.Data.Entity

Public Class AppDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=OfimedicDB")
    End Sub

    Public Property Albums As DbSet(Of Album)
    Public Property Photos As DbSet(Of Photo)
End Class