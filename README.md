# Prueba Técnica Agroexport

Este proyecto es una aplicación de escritorio desarrollada en C# con WPF, diseñada para demostrar funcionalidades de conexión a base de datos, manejo de modelos y vistas, y operaciones CRUD.

## 📁 Estructura del Proyecto

```
prueba-tecnica-agroexport/
├── App.xaml                 # Archivo de aplicación WPF
├── App.xaml.cs             # Código de inicio de la aplicación
├── Assets/                 # Archivos estáticos o recursos gráficos
├── DbConnections/          # Archivos de conexión a la base de datos
├── Helpers/                # Clases auxiliares o utilitarias
├── MainWindow.xaml         # Interfaz principal
├── MainWindow.xaml.cs      # Lógica de la interfaz principal
├── Models/                 # Modelos de datos
├── Pages/                  # Vistas o páginas de la aplicación
├── bin/                    # Archivos compilados
├── obj/                    # Archivos generados por el compilador
├── prueba-tecnica-agroexport.csproj  # Archivo de proyecto C#
```

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

```
   DbConnectios/DbConnection.cs
```

## 🧱 Script SQL para Crear la Base de Datos

Ejecuta este script en SQL Server Management Studio o cualquier cliente compatible:

```
sql
Copy
Edit
-- Crear la base de datos
IF NOT EXISTS (
    SELECT name FROM sys.databases WHERE name = 'MublesAgroexport'
)
BEGIN
    CREATE DATABASE MublesAgroexport;
END;
GO

USE MublesAgroexport;
GO

-- Clientes
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(64) NOT NULL,
    Rfc VARCHAR(13) NOT NULL,
    Email VARCHAR(32) UNIQUE,
    Address VARCHAR(128)
);

-- Usuarios
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(64) NOT NULL,
    Email VARCHAR(32) NOT NULL UNIQUE,
    Password VARCHAR(128) NOT NULL,
    Role VARCHAR(16) NOT NULL
);

-- Productos
CREATE TABLE Items (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(64) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);

-- Pedidos
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ItemQuantity INT NOT NULL,
    CustomerId INT NOT NULL,
    ItemId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    DeliveryDate DATETIME,
    Status VARCHAR(16),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    FOREIGN KEY (ItemId) REFERENCES Items(Id)
);

-- Proveedores
CREATE TABLE Suppliers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(64) NOT NULL,
    SocialReason VARCHAR(128) NOT NULL
);

-- Tipos de madera
CREATE TABLE WoodTypes (
    Id INT PRIMARY KEY,
    Name VARCHAR(32)
);

-- Recepción de madera
CREATE TABLE WoodReception (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SupplierId INT NOT NULL,
    AverageWeight DECIMAL(10,2) NOT NULL,
    PieceCount INT NOT NULL,
    WoodTypeId INT NOT NULL,
    TotalWeight DECIMAL(10,2) NOT NULL,
    ReceptionDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id),
    FOREIGN KEY (WoodTypeId) REFERENCES WoodTypes(Id)
);

```

## 🧪 Insertar Datos de Prueba

sql
Copy
Edit

```
-- Clientes
INSERT INTO Customers (FullName, Rfc, Email, Address) VALUES
('Juan Pérez', 'PEJJ800101AB1', 'juan.perez@example.com', 'Av. Reforma 123, CDMX'),
('Ana Gómez', 'GOMA920215CD2', 'ana.gomez@example.com', 'Calle Hidalgo 456, Guadalajara'),
('Luis Martínez', 'MALU750303EF3', 'luis.martinez@example.com', 'Blvd. Colosio 789, Monterrey');

-- Productos
INSERT INTO Items (Name, Quantity, Price) VALUES
('Silla de madera', 15, 499.99),
('Mesa de comedor', 10, 1299.50),
('Escritorio compacto', 8, 899.00),
('Estantería moderna', 5, 650.75),
('Sofá 2 plazas', 3, 2199.90),
('Taburete alto', 20, 299.99),
('Librero vertical', 7, 799.95),
('Mesa de centro', 12, 599.00),
('Cama individual', 4, 2599.99),
('Banco plegable', 18, 199.50);

-- Pedidos
INSERT INTO Orders (ItemQuantity, CustomerId, ItemId, CreatedAt, DeliveryDate, Status) VALUES
(1, 1, 2, GETDATE(), NULL, 'Pendiente'),
(2, 2, 5, GETDATE(), NULL, 'Pendiente'),
(1, 3, 1, GETDATE(), NULL, 'Pendiente');

-- Usuario
INSERT INTO Users (FullName, Email, Password, Role) VALUES
('Carlos Antonio Reyes Bello', 'carlos@mail.com', '123456', 'Super Admin');

-- Proveedores
INSERT INTO Suppliers (FullName, SocialReason) VALUES
('María Torres', 'Bosques de Occidente SA de CV'),
('Jorge Sánchez', 'Distribuidora Forestal La Sierra'),
('Elena Ríos', 'Maderas Selectas del Bajío SA');

-- Tipos de madera
INSERT INTO WoodTypes (Id, Name) VALUES
(1, 'Pino'),
(2, 'Cedro'),
(3, 'Encino');

-- Recepción de madera
INSERT INTO WoodReception (SupplierId, AverageWeight, PieceCount, WoodTypeId, TotalWeight, ReceptionDate) VALUES
(1, 12.50, 40, 1, 500.00, '2025-06-22 08:00:00'),
(2, 15.00, 30, 2, 450.00, '2025-06-21 10:30:00'),
(3, 20.00, 25, 3, 500.00, '2025-06-20 15:45:00');
```

### Archivo relevante:

Revisa el archivo dentro de esa carpeta que contenga código similar a:

```csharp
string connectionString = "Server=localhost;Database=NombreBD;User Id=usuario;Password=contraseña;";


🔧 Modifica esta cadena para reflejar tus parámetros reales de base de datos:

Server: tu servidor SQL (por ejemplo, localhost o IP remota)

Database: nombre de tu base de datos

User Id y Password: credenciales de acceso

💡 Asegúrate de que el servidor de base de datos esté activo antes de ejecutar la aplicación.
```
