using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase {
    private readonly ApplicationDbContext _context;
    
    public ProductosController(ApplicationDbContext context) {
        _context = context;
    }

    ////GET api/Productos (Incluyendo la Categoria)
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    //{
    //    //EF Core: Traer todos los productos e incluir la info de la categoria
    //    return await _context.Productos
    //                    .Include(p => p.Categoria) // 👈 ESTE ES EL INCLUDE
    //                    .ToListAsync();
    //}

    //GET: api/Productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDetalleDto>>> GetProductosDto() {
        return await _context.Productos                
                .Select(p => new ProductoDetalleDto // 👈 PROYECCIÓN: Transforma el Producto en DTO
                {
                    Id = p.Id,
                    NombreProducto = p.Nombre,
                    Precio = p.Precio,
                    NombreCategoria = p.Categoria.Nombre
                }).ToListAsync();
    }

    //GET: api/Productos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetProducto(int id) {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) {
            return NotFound();
        }

        return producto;
    }

    //// POST: api/Productos
    ///post sin dto
    //[HttpPost]
    //public async Task<ActionResult<Producto>> PostProducto(Producto producto) {
    //    _context.Productos.Add(producto);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(nameof(GetProducto), new { producto.Id }, producto);
    //}

    //PUT. api/productos/5
    [HttpPut]
    public async Task<ActionResult<Producto>> PutProducto(Producto producto)
    {
        _context.Entry(producto).State = EntityState.Modified;
        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!_context.Productos.Any(e => e.Id == producto.Id))
                return NotFound();
            else
                throw;
        }
        return NoContent();
    }

    //DELETE /api/Productos/5
    [HttpDelete]
    public async Task<IActionResult> DeleteProducto(int id) {
        //Buscamos el producto a borrar
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) {
            // Si no existe devolvemos notfound
            return NotFound();
        }
        //intentamos borrar  el producto
        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        //Devuelve un código HTTP 204, que es un código de éxito estándar.
        return NoContent();
    }

    // GET: api/Productos/Buscar?nombre=leche&precioMax=5.00&categoriaId=3
    [HttpGet("Buscar")]
    public async Task<ActionResult<IEnumerable<Producto>>> BuscarProductos(
            [FromQuery] string? nombre, // el nombre es opcional
            [FromQuery] decimal? precioMax, //el precion máximo es opcional
            [FromQuery] int? categoriaId) //el id de la categoria es opcional
        {
        //1. Inicia la consulta con todos los productos
        IQueryable<Producto> consulta = _context.Productos.Include(p => p.Categoria);

        //2. Aplica filtros condicionales:

        //FILTRO POR  NOMBRE
        if (!string.IsNullOrEmpty(nombre)){
            consulta = consulta.Where(p => p.Nombre.Contains(nombre));
        }

        //FILTRO POR PRECIO MÁXIMO
        if (precioMax.HasValue) {
            consulta = consulta.Where(p => p.Precio <= precioMax.Value);
        }

        //FILTRO POR CATEGORIA
        if (categoriaId.HasValue) {
            consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);
        }

        //3. Ejecuta la consulta SQL y devuelve los resultados.
        return await consulta.ToListAsync();
    }

    //POST: api/Productos
    [HttpPost]
    public async Task<ActionResult<Producto>> PostProductoDto(CrearProductoDto productoDto) {
        var productoEntidad = new Producto {
            Nombre = productoDto.Nombre,
            Precio = productoDto.Precio,
            CategoriaId = productoDto.CategoriaId
        };
        //Se usa la entidad con EF Core
        _context.Productos.Add(productoEntidad);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducto), new { id = productoEntidad.Id}, productoEntidad);
    }

}