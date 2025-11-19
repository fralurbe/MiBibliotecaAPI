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
}