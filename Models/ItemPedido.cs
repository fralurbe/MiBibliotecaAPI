using System.ComponentModel.DataAnnotations;

namespace MiBibliotecaAPI.Models {
    // Esta entidad representa la línea de detalle dentro de un pedido.
    // Es el enlace entre un Pedido y un Producto, y almacena la cantidad comprada.
    public class ItemPedido {
        public int Id { get; set; }

        // Datos del Pedido (FK)
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        // Datos del Producto (FK)
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        // La información de la transacción:
        // La cantidad comprada que debemos revertir al cancelar
        [Required]
        public int Cantidad { get; set; }

        // El precio al que se vendió en ese momento
        public decimal PrecioUnitario { get; set; }
    }
}