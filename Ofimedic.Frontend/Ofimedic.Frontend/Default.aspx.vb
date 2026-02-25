Imports System.Data
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load() Handles Me.Load
        If Not IsPostBack Then
            RegisterAsyncTask(New PageAsyncTask(AddressOf CargarAlbumes))
        End If
    End Sub

    Private Async Function CargarAlbumes() As Threading.Tasks.Task
        Try
            Dim json = Await ApiHelper.GetAlbumsAsync(txtFiltro.Text)
            Dim dt = JsonConvert.DeserializeObject(Of DataTable)(json)
            gvAlbumes.DataSource = dt
            gvAlbumes.DataBind()
        Catch ex As Exception
            litMensaje.Text = "<p style='color:red'>Error: " & ex.Message & "</p>"
        End Try
    End Function

    Protected Sub btnFiltrar_Click() Handles btnFiltrar.Click
        RegisterAsyncTask(New PageAsyncTask(AddressOf CargarAlbumes))
    End Sub

    Protected Sub btnSync_Click() Handles btnSync.Click
        RegisterAsyncTask(New PageAsyncTask(AddressOf SincronizarAsync))
    End Sub

    Private Async Function SincronizarAsync() As Task
        Try

            Dim resultado = Await ApiHelper.SyncDataAsync()
            Dim resultadoLimpio = resultado.Trim().Trim(""""c)

            If resultadoLimpio = "OK" Then
                litMensaje.Text = "<p style='color:green'>Sincronización OK</p>"
                Await CargarAlbumes()
            Else
                litMensaje.Text = "<p style='color:red'>ERROR al sincronizar</p>"
            End If

        Catch ex As Exception
            litMensaje.Text = $"<p style='color:red'>Error: {ex.Message}</p>"
        End Try
    End Function

    Protected Sub gvAlbumes_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "VerFotos" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim id = gvAlbumes.DataKeys(index).Value
            Response.Redirect("Fotos.aspx?albumId=" & id)
        End If
    End Sub
End Class