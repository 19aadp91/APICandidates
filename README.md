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
"ConnectionStrings": {
  "DefaultConnection": "Server=LAPTOP-6PTE51VD\\SQLEXPRESS;Database=CandidatesDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
