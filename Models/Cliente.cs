using System.Collections.Generic;

namespace MiBibliotecaAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        // Relación uno a muchos: Un cliente puede tener múltiples órdenes
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}