using MiBibliotecaAPI.Models;

namespace MiBibliotecaAPI {
    public class Editorial {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string pais { get; set; }

        // relacion
        public ICollection<Libro> LibrosPublicados { get; set; }
    }
}