namespace MiBibliotecaAPI.Models {
    public class Libro {
        public int Id { get; set; }
        public string Titulo { get; set; }

        // Relación: Un libro pertenece a un autor
        public int AutorId { get; set; } // Clave foranea
        public Autor Autor { get; set; }
        public int EditorialId { get; set; }// Clave foranea
        public Editorial editorial { get; set; }
    }
}