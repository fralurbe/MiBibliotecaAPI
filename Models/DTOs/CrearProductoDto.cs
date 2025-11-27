namespace MiBibliotecaAPI.Models.DTOs {
    public class CrearProductoDto {
        // NO incluimos 'Id' porque la BD lo genera.
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; } // La FK que el cliente debe proporcionar.
    }
}