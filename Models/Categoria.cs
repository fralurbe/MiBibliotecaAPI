using System.Collections.Generic;

public class Categoria {
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    // Propiedad de Navegacion: Coleccion de Productos en esta Categoria
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
