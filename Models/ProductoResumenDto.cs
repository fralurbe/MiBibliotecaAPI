namespace MiBibliotecaAPI.Models {
    //DTO Interno
    public class ProductoResumenDto {
        public string NombreProducto { get; set; } = string.Empty;
        public decimal PrecioUnidad { get; set; }
        public string NombreDeCategoria { get; set; } = string.Empty;
    }
}