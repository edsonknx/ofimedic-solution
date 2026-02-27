Public Class Photo
    Public Property Id As Integer
    Public Property AlbumId As Integer
    Public Property Title As String
    Public Property Url As String
    Public Property ThumbnailUrl As String

    ' Propiedad de navegación 
    Public Overridable Property Album As Album
End Class