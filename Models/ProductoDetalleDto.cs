using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Data;

namespace MiBibliotecaAPI.Models {
    public class ProductoDetalleDto {
        public int Id { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Precio { get; set; }

        // propiedad proyectada: Nombre de la Categoria
        public string NombreCategoria { get; set; } = string.Empty;
    }
}