using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Models;

namespace MiBibliotecaAPI.Data {
    // Esta es la clase puente
    public class ApplicationDbContext : DbContext {
        // Constructor: Necesario para la inyección de dependencias
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }

        // DbSets: Representan las tablas de tu base de datos
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
    }
}