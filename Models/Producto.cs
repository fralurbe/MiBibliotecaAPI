using System.Collections.Generic;

public class Producto {
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    //clave Foránea (FK)
    public int CategoriaId { get; set; }

    //Propiedades de navegación: Referencia a la Categoria del producto
    public Categoria Categoria { get; set; } = null;

}