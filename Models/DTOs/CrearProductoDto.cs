using System.ComponentModel.DataAnnotations;

namespace MiBibliotecaAPI.Models.DTOs {
    // DTO utilizado para la creación (POST) de nuevos productos
    public class CrearProductoDto {
        // El nombre es obligatorio y debe tener un máximo de 100 caracteres.
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        // La descripción es obligatoria.
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        // El precio es obligatorio y debe estar entre 0.01 y 10,000.
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe ser un valor positivo y menor a 10,000.")]
        public decimal Precio { get; set; }

        // El stock es obligatorio y debe ser al menos 0 (permitiendo productos sin stock).
        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 10000, ErrorMessage = "El stock debe ser un número entero positivo.")]
        public int Stock { get; set; }

        // La clave foránea de Categoría es obligatoria para vincular el producto.
        [Required(ErrorMessage = "La Categoría (ID) es obligatoria.")]
        public int CategoriaId { get; set; }
    }
}
