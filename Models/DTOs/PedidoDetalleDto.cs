// Models/PedidoDetalleDto.cs
namespace MiBibliotecaAPI.Models.DTOs {
    public class PedidoDetalleDto {
        public int Id { get; set; }
        public decimal TotalPedido { get; set; }

        // Lista de productos proyectados
        public ICollection<ProductoResumenDto> Productos { get; set; } = new List<ProductoResumenDto>();
    }
}