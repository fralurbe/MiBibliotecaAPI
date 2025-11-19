using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public LibrosController(ApplicationDbContext context) {
        _context = context;
    }

    // GET: api/Libros
    // Lee todos los libros e incluye la información del autor (JOIN)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Libro>>> GetLibros() {
        // CÓDIGO CLAVE DE EF CORE
        // .Include(l => l.Autor) le dice a EF que haga un JOIN con la tabla Autores
        return await _context.Libros
            .Include(l => l.Autor)
            .ToListAsync();
    }

    // GET: api/Libros/5
    // Lee un libro específico e incluye el autor
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> GetLibro(int id) {
        var libro = await _context.Libros
            .Include(l => l.Autor) // Vuelve a incluir el Autor
            .FirstOrDefaultAsync(l => l.Id == id);

        if (libro == null) {
            return NotFound();
        }

        return libro;
    }

    // POST: api/Libros
    // Crea un nuevo libro (Verificación de Clave Foránea)
    [HttpPost]
    public async Task<ActionResult<Libro>> PostLibro(Libro libro) {
        // Lógica de EF Core para crear:
        _context.Libros.Add(libro);
        await _context.SaveChangesAsync();

        // Tras guardar, recuperamos el libro recién creado, incluyendo los datos del Autor
        // para devolver una respuesta completa al cliente.
        await _context.Entry(libro).Reference(l => l.Autor).LoadAsync();

        return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
    }

    // PUT: api/Libros/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLibro(int id, Libro libro) {
        // 1. Verificación de ID: La URL y el objeto deben coincidir.
        if (id != libro.Id) {
            return BadRequest(); // 400 Bad Request
        }
        // 2. PATRÓN EF CORE: Marca el objeto como modificado.
        // Esto hace que EF Core genere un comando SQL UPDATE.
        _context.Entry(libro).State = EntityState.Modified;
        try {
            // 3. EJECUCIÓN: Envía el UPDATE a la base de datos.
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            // Manejo de errores si el libro no existe para actualizar.
            if (!_context.Libros.Any(e => e.Id == id)) {
                return NotFound(); // 404 Not Found
            }
            else {
                throw;
            }
        }
        return NoContent(); // 204 No Content (Éxito sin devolver datos)
    }

    // DELETE: api/Libros/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLibro(int id) {
        // 1. PATRÓN EF CORE: Busca el libro que se desea eliminar.
        var libro = await _context.Libros.FindAsync(id);

        if (libro == null) {
            return NotFound(); // 404 Not Found
        }

        // 2. PATRÓN EF CORE: Marca el objeto como eliminado.
        _context.Libros.Remove(libro);

        // 3. EJECUCIÓN: Envía el DELETE a la base de datos.
        await _context.SaveChangesAsync();

        return NoContent(); // 204 No Content
    }
}