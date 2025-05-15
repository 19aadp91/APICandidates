# APICandidates

Aplicación ASP.NET Core MVC con arquitectura en capas para la gestión de candidatos y sus experiencias laborales. Incluye integración con Entity Framework Core y servicios separados por dominio para candidatos y experiencias laborales.

---

## 🚀 Características

- CRUD de candidatos con sus experiencias laborales.
- Interfaz web para la gestión y edición.
- Backend modular con servicios y DTOs.
- Base de datos en SQL Server.
- AutoMapper para mapeo entre entidades y DTOs.
- Inyección de dependencias configurada con `IServiceCollection`.

---

## 🧱 Requisitos

- .NET 7.0 o superior
- SQL Server Express (u otra versión de SQL Server)
- Visual Studio 2022 o superior

---

## ⚙️ Configuración del proyecto

### 1. Clona el repositorio

- git clone https://github.com/19aadp91/APICandidates.git
- cd APICandidates

### 2. Configura la cadena de conexión

- Abre el archivo appsettings.json y ajusta la conexión a tu instancia de SQL Server:
"ConnectionStrings": {
  "DefaultConnection": "Server=LAPTOP-6PTE51VD\\SQLEXPRESS;Database=CandidatesDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

---

## 🛠️ Migraciones de Base de Datos

Para crear la base de datos usando Entity Framework Core, sigue estos pasos desde Visual Studio:

- Abre el Administrador de paquetes de NuGet (Tools > NuGet Package Manager > Package Manager Console)
- Selecciona el proyecto de inicio (por ejemplo APICandidates) si es necesario.
- Ejecuta los siguientes comandos:
-- Add-Migration InitialCreate
-- Update-Database
