using System.Collections.Generic;

namespace MiBibliotecaAPI.Models {
    public class Autor {
        public int Id { get; set; } // EF Core lo detecta como Primary Key
        public string Nombre { get; set; }
        // Relación: Un autor puede tener muchos libros
        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}