using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public UsuariosController(ApplicationDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios() {
        //Esto le dice a EF Core que lea todos los usuarios.
        return await _context.Usuarios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id) {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null) {
            return NotFound();
        }
        return usuario;
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario) {
        //1. Marcar el objeto para la inserción.
        _context.Usuarios.Add(usuario);

        //2. Ejecutar la transacción en la base de datos
        await _context.SaveChangesAsync();

        // Devolver la respuesta 201 Created
        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

    //PUT, Actualiza o reemplaza /api/Usuarios/5
    [HttpPut]
    public async Task<ActionResult> PutUsuario(int id, Usuario usuario) {
        _context.Entry(usuario).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!_context.Usuarios.Any(e => e.Id == id)) {
                return NotFound(); // 404:Recurso no encontrado
            }
            else {
                throw;
            }
        }
        return NoContent();
    }

    // DELETE: /api/Usuarios/5
    [HttpDelete("{id}")] // ?? Necesitas el parámetro de ruta en el atributo
    public async Task<IActionResult> DeleteUsuario(int id) {
        // 1. ?? BUSCAR Y CARGAR EL OBJETO COMPLETO (NECESARIO PARA ELIMINAR)
        var usuario = await _context.Usuarios.FindAsync(id);

        // 2. ?? COMPROBAR SI EXISTE (Si no, devuelve 404)
        if (usuario == null) {
            return NotFound(); // Código HTTP 404
        }

        // 3. ??? MARCAR PARA ELIMINACIÓN (Método simple)
        _context.Usuarios.Remove(usuario);
        // Opcional, pero más complejo: _context.Entry(usuario).State = EntityState.Deleted;

        // 4. ?? GUARDAR LOS CAMBIOS (Ejecuta la sentencia DELETE SQL)
        await _context.SaveChangesAsync();

        // 5. ?? RESPUESTA CORRECTA (Éxito sin contenido)
        return NoContent(); // Código HTTP 204
    }
}