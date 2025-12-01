using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;

// El atributo [ApiController] habilita el comportamiento específico de API (validación, etc.)
[Route("api/[controller]")]
[ApiController]
public class AutoresController : ControllerBase {
    // 1. Campo privado para el DbContext
    private readonly ApplicationDbContext _context;

    // 2. Constructor con Inyección de Dependencias
    // El sistema de DI inyecta la instancia configurada de ApplicationDbContext
    public AutoresController(ApplicationDbContext context) {
        _context = context;
    }

    // GET: api/Autores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Autor>>> GetAutores() {
        // Consulta LINQ: Selecciona todos los autores de la tabla 'Autores'
        return await _context.Autores.ToListAsync();
    }

    // GET: api/Autores/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Autor>> GetAutor(int id) {
        // Consulta LINQ: Busca un autor por su ID
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null) {
            return NotFound(); // Retorna 404 si no se encuentra
        }
        return autor;        
    }

    // POST: api/Autores
    // Crea un nuevo autor
    [HttpPost]
    public async Task<ActionResult<Autor>> PostAutor(Autor autor) {
        // Añade el autor al DbContext para su seguimiento
        _context.Autores.Add(autor);

        // Guarda los cambios en la base de datos (Ejecuta el INSERT INTO)
        await _context.SaveChangesAsync();

        // Retorna 201 Created y la URI para acceder al nuevo recurso
        return CreatedAtAction(nameof(GetAutor), new { id = autor.Id }, autor);
    }

    // PUT: api/Autores/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAutor(int id, Autor autor) {
        // Verifica que el ID de la URL y el ID del objeto coincidan
        if (id != autor.Id) {
            return BadRequest(); // Error 400
        }

        // PASO 1: Informar a EF Core que el objeto está modificado.
        _context.Entry(autor).State = EntityState.Modified;

        try {
            // PASO 2: Guardar los cambios (Ejecuta el UPDATE SQL)
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            // ... manejo de errores si el autor ya no existe
            if (!_context.Autores.Any(e => e.Id == id)) {
                return NotFound(); // Error 404
            }
            else {
                throw;
            }
        }
        // Retorna 204 No Content (Éxito sin devolver datos)
        return NoContent();
    }

    // DELETE: api/Autores/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAutor(int id) {
        // PASO 1: Encontrar el autor por ID
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null) {
            return NotFound(); // Error 404 si no se encuentra
        }

        // PASO 2: Informar a EF Core que el objeto debe ser eliminado
        _context.Autores.Remove(autor);

        // PASO 3: Guardar los cambios (Ejecuta el DELETE SQL)
        await _context.SaveChangesAsync();

        return NoContent(); // Retorna 204 No Content
    }
}