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

```bash
git clone https://github.com/tuusuario/APICandidates.git
cd APICandidates
```

### 2. Configura la cadena de conexión

Abre el archivo `appsettings.json` y ajusta la conexión a tu instancia de SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=LAPTOP-6PTE51VD\SQLEXPRESS;Database=CandidatesDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 🧩 Servicios registrados

En `ServiceCollectionExtensions.cs` se registran los servicios:

```csharp
public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString, b => b.MigrationsAssembly("APICandidates")));

    services.AddScoped<IUnitOfWork<AppDbContext>, UnitOfWork<AppDbContext>>();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddScoped<ICandidateServices, CandidateServices>();
    services.AddScoped<ICandidateExperienceServices, CandidateExperienceServices>();

    return services;
}
```

---

## 🛠️ Migraciones de Base de Datos

Para crear la base de datos usando Entity Framework Core, sigue estos pasos desde **Visual Studio**:

1. Abre el **Administrador de paquetes de NuGet** (`Tools > NuGet Package Manager > Package Manager Console`)
2. Selecciona el proyecto de inicio (por ejemplo `APICandidates`) si es necesario.
3. Ejecuta los siguientes comandos:

```powershell
Add-Migration InitialCreate
Update-Database
```

Esto generará las tablas necesarias y creará la base de datos `CandidatesDB`.

---

## ▶️ Ejecución de la aplicación

En Visual Studio:

- Pulsa **F5** o haz clic en **"Start Debugging"**
- La aplicación abrirá una interfaz web para administrar candidatos y experiencias laborales.

---

## 📌 Notas

- La propiedad `Email` en edición es de solo lectura para evitar cambios accidentales.
- Si eliminas una experiencia desde el formulario, asegúrate de manejar correctamente los índices para mantener la integridad del modelo (ver lógica en JavaScript).

---

## 📧 Contacto

Si tienes dudas o sugerencias, contacta al desarrollador principal del proyecto.

---
