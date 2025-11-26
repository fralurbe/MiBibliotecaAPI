using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Mappers;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE SERVICIOS (builder.Services) ---

// Configuración de Entity Framework Core (Conexión a la BD)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Habilitar la compatibilidad con controladores (para AutoresController, etc.)
builder.Services.AddControllers();

// Habilitar servicios de Swagger/OpenAPI para la documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Añadir el soporte para CORS (necesario si el front-end está en otro puerto)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

// --- 2. CONFIGURACIÓN DEL MIDDLEWARE (app.Use) ---

// Configurar el HTTP request pipeline (Swagger solo en entorno de desarrollo)
if (app.Environment.IsDevelopment()) {
    // Las dos líneas que cargan la documentación y la interfaz visual
    app.UseSwagger();
    app.UseSwaggerUI(); // ¡Esto resuelve el 404 Not Found!
}

app.UseHttpsRedirection();

// Usar el middleware de CORS (debe ir antes de MapControllers)
app.UseCors("CorsPolicy");

app.UseAuthorization();

// Mapear los controladores (AutoresController, LibrosController, etc.)
app.MapControllers();

app.Run();