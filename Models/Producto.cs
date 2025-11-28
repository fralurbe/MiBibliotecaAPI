namespace MiBibliotecaAPI.Models {
    public class Producto {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        //clave Foránea (FK)
        public int CategoriaId { get; set; }

        //Propiedades de navegación: Referencia a la Categoria del producto.
        public Categoria Categoria { get; set; } = null!;

        //Lo hacemos anulable porque un producto puede no estar en ningun pedido.
        public int? PedidoId { get; set; }
        public Pedido? Pedido { get; set; } = null;

        public int Stock { get; set; }
    }
}