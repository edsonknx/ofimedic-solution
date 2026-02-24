Public Class Album
    Public Property Id As Integer
    Public Property UserId As Integer
    Public Property Title As String
    Public Overridable Property Photos As ICollection(Of Photo)

    Public Sub New()
        Photos = New HashSet(Of Photo)()
    End Sub
End Class