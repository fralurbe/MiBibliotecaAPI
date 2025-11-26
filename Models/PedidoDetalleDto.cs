// Models/PedidoDetalleDto.cs
using MiBibliotecaAPI;

namespace MiBibliotecaAPI.Models {
    public class PedidoDetalleDto {
        public int Id { get; set; }
        public decimal TotalPedido { get; set; } // 👈 USAMOS ESTA

        // Lista de productos proyectados
        public ICollection<ProductoResumenDto> Productos { get; set; } = new List<ProductoResumenDto>();
    }
}