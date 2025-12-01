using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Models;

namespace MiBibliotecaAPI.Data {
    // Esta es la clase puente para interactuar con la base de datos
    public class ApplicationDbContext : DbContext {
        // Constructor: Necesario para la inyección de dependencias
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        // DbSets: Representan las tablas de tu base de datos
        // Entidades adicionales del usuario
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Entidades del módulo de pedidos/inventario
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        // El DbSet para la tabla pivote (Muchos a Muchos)
        public DbSet<PedidoDetalle> PedidosDetalle { get; set; } // Nombre del DbSet conservado

        // Dentro de la clase ApplicationDbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Llama al método base: Importante para cualquier configuración interna de EF Core
            base.OnModelCreating(modelBuilder);

            // --- CONFIGURACIONES DE PRECISIÓN PARA DINERO ---

            // Configuración para la propiedad 'Total' de la entidad 'Pedido'
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(18, 2); // 18 dígitos en total, 2 decimales.

            // Configuración para el 'Precio' de la entidad 'Producto'
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2); // 18 dígitos en total, 2 decimales.

            // --- CONFIGURACIÓN CRUCIAL DE LA TABLA PIVOTE (PedidoDetalle) ---

            // 1. Configura la clave primaria compuesta: (PedidoId, ProductoId)
            modelBuilder.Entity<PedidoDetalle>()
                .HasKey(pd => new { pd.PedidoId, pd.ProductoId });

            // 2. Configura la precisión del campo 'PrecioUnitario' en PedidoDetalle
            // (Esto resuelve la advertencia azul que viste)
            modelBuilder.Entity<PedidoDetalle>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            // 3. Define la relación: Un PedidoDetalle tiene un solo Producto
            modelBuilder.Entity<PedidoDetalle>()
                .HasOne(pd => pd.Producto)
                .WithMany() // No necesitamos navegar desde Producto a Detalles, por ahora.
                .HasForeignKey(pd => pd.ProductoId);

            // 4. Define la relación: Un PedidoDetalle tiene un solo Pedido
            modelBuilder.Entity<PedidoDetalle>()
                .HasOne(pd => pd.Pedido)
                .WithMany(p => p.Detalles) // Mapea a la colección 'Detalles' en la entidad Pedido.
                .HasForeignKey(pd => pd.PedidoId);
        }
    }
}