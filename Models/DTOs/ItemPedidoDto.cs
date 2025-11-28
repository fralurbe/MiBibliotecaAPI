namespace MiBibliotecaAPI.Models.DTOs {
    // Representa un producto y la cantidad que se compra
    public class ItemPedidoDto {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}