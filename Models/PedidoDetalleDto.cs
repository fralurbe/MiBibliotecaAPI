// Models/PedidoDetalleDto.cs

using MiBliotecaAPI;
namespace MiBliotecaAPI.Models {
    public class PedidoDetalleDto {
        public int Id { get; set; }
        public decimal TotalPedido { get; set; } // 👈 USAMOS ESTA

        // Lista de productos proyectados
        public ICollection<ProductoResumenDto> Productos { get; set; } = new List<ProductoResumenDto>();
    }
}