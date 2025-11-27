# MiBibliotecaAPI - API REST con .NET 9 + EF Core 9 + SQL Server

API REST moderna para gestión de biblioteca con relación uno-a-muchos (Autor → Libros).  
Proyecto personal desarrollado con las tecnologías del ecosistema .NET.

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![EF Core 9](https://img.shields.io/badge/EF_Core-9.0.11-blue)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?logo=microsoftsqlserver&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green)

## Tecnologías utilizadas

- **.NET 9.0** (última versión estable - noviembre 2025)
- **Entity Framework Core 9.0.11** - Migrations Code-First
- **SQL Server 2022** (también compatible con LocalDB)
- **AutoMapper 13.0.1** + DTOs
- **ASP.NET Core Web API** con Minimal APIs y controladores
- **Swagger / OpenAPI** con Swashbuckle
- **Inyección de dependencias** nativa
- **Buenas prácticas**: AsNoTracking, paginación, validación FK, async/await

## Funcionalidades implementadas

- CRUD completo en `Autores` y `Libros`
- Relación 1-N con navegación (`Include`)
- Paginación y filtros en listado de libros  
  `GET /api/Libros?pagina=2&tamano=10&titulo=cien`
- Mapeo automático con AutoMapper
- Respuestas estructuradas con metadatos de paginación
- Migraciones automáticas con EF Core

## Cómo ejecutar el proyecto

```bash
# 1. Clonar el repositorio
git clone https://github.com/fralurbe/MiBibliotecaAPI.git
cd MiBibliotecaAPI

# 2. Restaurar paquetes
dotnet restore

# 3. Aplicar migraciones (crea la base de datos)
dotnet ef database update

# 4. Ejecutar
dotnet run

Ejemplos de uso:
# Crear autor
curl -X POST https://localhost:7xxx/api/Autores -H "Content-Type: application/json" -d "{\"nombre\":\"Gabriel García Márquez\"}"

# Crear libro
curl -X POST https://localhost:7xxx/api/Libros -H "Content-Type: application/json" -d "{\"titulo\":\"Cien años de soledad\",\"autorId\":1}"

# Listar libros con paginación
curl "https://localhost:7xxx/api/Libros?pagina=1&tamano=5"

Estructura del proyecto:
MiBibliotecaAPI/
├── Controllers/       → Controladores API
├── Data/              → ApplicationDbContext
├── Models/            → Entidades de dominio
├── DTOs/              → Data Transfer Objects
├── Mapping/           → Configuración AutoMapper
├── Migrations/        → Migraciones EF Core
└── Program.cs         → Configuración de servicios
