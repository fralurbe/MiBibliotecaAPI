namespace MiBibliotecaAPI.Models {
    public class Libro {
        public int Id { get; set; }
        public string Titulo { get; set; }

        // Relación: Un libro pertenece a un autor
        public int AutorId { get; set; } // Foreign Key
        public Autor Autor { get; set; }
    }
}