Imports System.Web.Http

Public Module WebApiConfig
    Public Sub Register(config As HttpConfiguration)
        ' Configurar rutas de atributos
        config.MapHttpAttributeRoutes()

        ' Ruta convencional
        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )

        ' Opcional: Forzar JSON
        config.Formatters.Remove(config.Formatters.XmlFormatter)
    End Sub
End Module