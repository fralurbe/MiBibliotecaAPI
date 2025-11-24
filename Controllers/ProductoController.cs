using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MiBibliotecaAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase {
    private readonly ApplicationDbContext _context;
    
    public ProductosController(ApplicationDbContext context) {
        _context = context;
    }

    //GET api/Productos (Incluyendo la Categoria)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        //EF Core: Traer todos los productos e incluir la info de la categoria
        return await _context.Productos
                        .Include(p => p.Categoria) // 👈 ESTE ES EL INCLUDE
                        .ToListAsync();
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

    // POST: api/Productos
    [HttpPost]
    public async Task<ActionResult<Producto>> PostProducto(Producto producto) {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducto), new { producto.Id }, producto);
    }

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
}