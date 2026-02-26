# Ofimedic - Prueba Técnica .NET (VB.NET + WebForms)

## Descripción
Solución completa para la prueba técnica de Ofimedic que cumple con todos los requisitos:
- Backend API REST en VB.NET con Entity Framework
- Frontend en ASP.NET WebForms
- Base de datos SQL Server
- Sincronización con JSONPlaceholder (100 álbumes + 5000 fotos)
- Filtros por título en álbumes y fotos

## GUÍA DE INSTALACIÓN PASO A PASO

### Paso 1: Obtener el proyecto
Opción A - Clonar con Git:
git clone https://github.com/edsonknx/ofimedic-solution.git
cd ofimedic-simple

### Paso 2: Configurar Base de Datos
Ejecutar el script SQL ubicado en /Database en SQL Server Management Studio (SSMS)

### Paso 3: Configurar Cadena de Conexión
Abrir Backend\Web.config y modifica:

```xml
<connectionStrings>
    <add name="OfimedicDB" 
         connectionString="Server=localhost;Database=OfimedicDB;Integrated Security=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>

Server=localhost debe configurarse con el nombre del SQL Server, ejemplo:
- SQL Express: Server=localhost\SQLEXPRESS;

### Paso 4: Configurar Puertos del Backend
1. En Visual Studio, seleccionar proyecto Ofimedic.Backend
2. Presionar propiedades
3. En Web, URL del proyecto pon: https://localhost:44386/
4. Guardar

### Paso 5: Configurar URL en Frontend
Abrir Frontend\Web.config y actualiza:

\```xml
<appSettings>
    <add key="ApiBaseUrl" value="https://localhost:44386" />
</appSettings>

Importante: El puerto debe coincidir con el paso anterior.

### Paso 6: Compilar
1. Abrir Ofimedic.Backend.VB.slnx en Visual Studio
2. Recompilar solucion

### Configurar Inicio Múltiple
1. Abrir propiedades de la solución
2. Configurar varios proyectos de inicio
3. Marcar los dos proyectos en "Inicio"
4. Aceptar

### Paso 8: Ejecutar
Presionar F5 o click en "Iniciar".
Se abrirán DOS ventanas:
- Backend: https://localhost:44386/ (error 403, es normal)
- Frontend: https://localhost:XXXXX/ (la aplicación)

### Paso 9: Sincronizar Datos
1. En el frontend, haz clic en "Sincronizar"
2. Esperar unos segundos
3. Aparecerá mensaje verde "Sincronización OK"
4. La tabla muestrará los albumes

Verificar en BD:
USE OfimedicDB;
SELECT COUNT(*) FROM Albums; 
SELECT COUNT(*) FROM Photos;

### Paso 10: Probar
- Ver fotos: Click en "Ver Fotos" de cualquier álbum
- Volver: Click en "Volver"
- Filtrar: Escribir "quia" y hacer click en "Filtrar"

## Propuestas de mejoras
- Inyección de dependencias
- Paginación
- Logging con Serilog
