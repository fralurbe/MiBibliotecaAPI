using MiBibliotecaAPI;
using MiBibliotecaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EditorialesController:ControllerBase {
    private readonly ApplicationDbContext _context;

    public EditorialesController(ApplicationDbContext context) {
        _context = context;
    }

    // GET: api/Editoriales
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Editorial>>> GetEditoriales() {
        return await _context.Editoriales.ToListAsync();
    }

    //GET: api/Editoriales/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Editorial>> GetEditorial(int id) {
        // PATRÓN 1: buscar por ID
        var editorial = await _context.Editoriales.FindAsync(id);
        if (editorial == null) {
            return NotFound();
        }
        return editorial;
    }

    //POST: api/Editoriales
    [HttpPost]
    public async Task<ActionResult<Editorial>> PostEditorial(Editorial editorial) {
        //PATRÓN 2: Marcar y ejecutar la transacción
        _context.Editoriales.Add(editorial);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEditorial), new { id = editorial.Id });
    }

    //PUT: api/Editoriales/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEditorial(int id, Editorial editorial) {
        if (id != editorial.Id) return BadRequest();

        //PATRON 2: Marcar como modificado
        _context.Entry(editorial).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!_context.Editoriales.Any(e => e.Id == id)) return NotFound();
            else throw;
        }
        return NoContent();
    }

    //DELETE: api/Editoriales/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEditorial(int id) {
        //PATRÓN 2: Eliminar y ejecutar la transaciión
        var editorial = await _context.Editoriales.FindAsync(id);
        if (editorial == null) return NotFound();

        _context.Editoriales.Remove(editorial);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}