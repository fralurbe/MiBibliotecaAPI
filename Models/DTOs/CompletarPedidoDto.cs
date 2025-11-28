using Microsoft.EntityFrameworkCore;

namespace MiBibliotecaAPI.Models.DTOs {
    public  class CompletarPedidoDto {
        public int ClienteId { get; set; }
        public ICollection<ItemPedidoDto> Items { get; set; } = new List<ItemPedidoDto>();
    }
}