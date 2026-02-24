Imports Newtonsoft.Json
Imports System.Data

Public Class Fotos
    Inherits System.Web.UI.Page

    Protected Async Sub Page_Load() Handles Me.Load
        If Not IsPostBack Then
            Dim albumId = Request.QueryString("albumId")
            If Not String.IsNullOrEmpty(albumId) Then
                Dim json = Await ApiHelper.GetPhotosAsync(CInt(albumId))
                Dim dt = JsonConvert.DeserializeObject(Of DataTable)(json)

                If dt.Rows.Count > 0 Then
                    rptFotos.DataSource = dt
                    rptFotos.DataBind()
                Else
                    lblNoFotos.Visible = True
                End If
            End If
        End If
    End Sub
End Class