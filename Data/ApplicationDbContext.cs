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
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        // Dentro de la clase ApplicationDbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Llama al método base si estás usando Identity u otras configuraciones
            // base.OnModelCreating(modelBuilder); 

            // Configuración para la propiedad 'Total' de la entidad 'Pedido'
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(18, 2); // 👈 18 dígitos en total, 2 decimales.         
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2); // 👈 18 dígitos en total, 2 decimales.         
        }
    }

}