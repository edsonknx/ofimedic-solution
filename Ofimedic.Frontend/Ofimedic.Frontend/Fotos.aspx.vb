Imports System.Data
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Fotos
    Inherits System.Web.UI.Page

    Protected Async Sub Page_Load() Handles Me.Load
        If Not IsPostBack Then
            Dim albumId = Request.QueryString("albumId")
            If Not String.IsNullOrEmpty(albumId) Then
                Await CargarFotosAsync(CInt(albumId))
            End If
        End If
    End Sub

    Private Async Function CargarFotosAsync(albumId As Integer) As Task
        Try
            Dim json = Await ApiHelper.GetPhotosAsync(albumId)
            Dim fotos = JArray.Parse(json)

            If fotos.Count > 0 Then
                rptFotos.DataSource = fotos
                rptFotos.DataBind()
                lblNoFotos.Visible = False
            Else
                rptFotos.Visible = False
                lblNoFotos.Visible = True
            End If

        Catch ex As Exception
            litMensaje.Text = "<p style='color:red'>Error: " & ex.Message & "</p>"
        End Try
    End Function
End Class