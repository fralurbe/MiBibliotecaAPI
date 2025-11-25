using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public CategoriaController(ApplicationDbContext context) {
        _context = context;
    }

    //1. GET. api/Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias() {
        //EF Core: Traer todos los registros de Categoria
        return await _context.Categorias.ToListAsync();
    }

    // GET: api/Categorias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoria(int id) {
        // EF Core: Buscar y cargar un objeto.
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null) {
            return NotFound(); // REST: 404
        }
        return categoria;
    }

    //POST: api/Categorias
    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria) {
        //EF Core: Marcar como nuevo.
        _context.Categorias.Add(categoria);
        //EF Core: Guardar cambios (INSERT SQL).
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
    }

    //PUT: api/Categorias/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoria(int id, Categoria categoria) {
        if (id != categoria.Id) return BadRequest();

        //EF Core: Marcar como modificado (el PUT que no busca)
        _context.Entry(categoria).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!_context.Categorias.Any(e => e.Id == id)) return NotFound();
            else throw;
        }
        return NoContent(); //REST: 204 No Content
    }

    //DELETE api/Categorias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id) {
        //EF Core: Buscar para poder eliminar (El DELETE que sí busca)
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}


