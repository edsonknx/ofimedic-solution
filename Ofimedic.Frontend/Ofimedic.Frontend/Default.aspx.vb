Imports Newtonsoft.Json
Imports System.Data

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load() Handles Me.Load
        If Not IsPostBack Then
            CargarAlbumes()
        End If
    End Sub

    Private Async Sub CargarAlbumes()
        Try
            Dim json = Await ApiHelper.GetAlbumsAsync(txtFiltro.Text)
            Dim dt = JsonConvert.DeserializeObject(Of DataTable)(json)
            gvAlbumes.DataSource = dt
            gvAlbumes.DataBind()
        Catch ex As Exception
            litMensaje.Text = "<p style='color:red'>Error: " & ex.Message & "</p>"
        End Try
    End Sub

    Protected Sub btnFiltrar_Click() Handles btnFiltrar.Click
        CargarAlbumes()
    End Sub

    Protected Async Sub btnSync_Click() Handles btnSync.Click
        Try
            Dim resultado = Await ApiHelper.SyncDataAsync()
            litMensaje.Text = "<p style='color:green'>Sincronización OK</p>"
            CargarAlbumes()
        Catch ex As Exception
            litMensaje.Text = "<p style='color:red'>Error: " & ex.Message & "</p>"
        End Try
    End Sub

    Protected Sub gvAlbumes_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "VerFotos" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim id = gvAlbumes.DataKeys(index).Value
            Response.Redirect("Fotos.aspx?albumId=" & id)
        End If
    End Sub
End Class