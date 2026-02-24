Imports System.Web.Http

Public Module WebApiConfig
    Public Sub Register(config As HttpConfiguration)
        ' Configurar rutas
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )

        ' Quitar XML para que devuelva JSON por defecto
        config.Formatters.Remove(config.Formatters.XmlFormatter)
    End Sub
End Module