# Prueba Técnica Agroexport

Este proyecto es una aplicación de escritorio desarrollada en C# con WPF, diseñada para demostrar funcionalidades de conexión a base de datos, manejo de modelos y vistas, y operaciones CRUD.

## 📁 Estructura del Proyecto

prueba-tecnica-agroexport/
├── Assets/ # Archivos estáticos o recursos gráficos
├── bin/ # Archivos compilados
├── DbConnections/ # Archivos de conexión a la base de datos
├── Helpers/ # Clases auxiliares o utilitarias
├── Models/ # Modelos de datos
├── obj/ # Archivos generados por el compilador
├── Pages/ # Vistas o páginas de la aplicación
├── App.xaml # Archivo de aplicación WPF
├── App.xaml.cs # Código de inicio de la aplicación
├── MainWindow.xaml # Interfaz principal
├── MainWindow.xaml.cs # Lógica de la interfaz principal
├── prueba-tecnica-agroexport.csproj # Archivo de proyecto C#


## 🚀 Cómo Ejecutar el Proyecto

1. **Requisitos Previos**:
   - Visual Studio 2019 o superior
   - .NET Core SDK o .NET Framework (según configuración del proyecto)

2. **Clona el repositorio** o descomprime el archivo ZIP.

3. **Abre el archivo `.csproj`** (`prueba-tecnica-agroexport.csproj`) en Visual Studio.

4. **Restaura dependencias** (si aplica):
   - En Visual Studio: `Tools > NuGet Package Manager > Manage NuGet Packages for Solution...`
   - O bien en terminal: `dotnet restore`

5. **Compila y ejecuta**:
   - Pulsa `F5` o haz clic en "Start".

## ⚙️ Configuración de la Base de Datos

La conexión a la base de datos se define en la carpeta:


### Archivo relevante:
Revisa el archivo dentro de esa carpeta que contenga código similar a:

```csharp
string connectionString = "Server=localhost;Database=NombreBD;User Id=usuario;Password=contraseña;";


🔧 Modifica esta cadena para reflejar tus parámetros reales de base de datos:

Server: tu servidor SQL (por ejemplo, localhost o IP remota)

Database: nombre de tu base de datos

User Id y Password: credenciales de acceso

💡 Asegúrate de que el servidor de base de datos esté activo antes de ejecutar la aplicación.