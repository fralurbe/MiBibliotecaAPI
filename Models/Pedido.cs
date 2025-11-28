namespace MiBibliotecaAPI.Models {
    public class Pedido {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        public List<PedidoDetalle> Detalles { get; set; } = new();
    }
}