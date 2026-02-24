Imports System.Web.Http

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        WebApiConfig.Register(GlobalConfiguration.Configuration)
    End Sub
End Class