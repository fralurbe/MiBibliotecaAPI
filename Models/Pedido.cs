namespace MiBibliotecaAPI.Models {
    public class Pedido {
        public int Id { get; set; }        
        public decimal Total { get; set; }
        // Relación muchos a uno: Muchos pedidos pueden pertenecer a un cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}