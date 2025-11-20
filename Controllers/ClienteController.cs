using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public ClientesController(ApplicationDbContext context) {
        _context = context;
    }

    //GET. api/Clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes() {
        return await _context.Clientes.ToListAsync();
    }

    //GET. api/Clientes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id) {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        return cliente;
    }

    //POST. api/Clientes
    [HttpPost]
    public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente) {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        ///// <summary>
        ///// Creates a <see cref="CreatedAtActionResult"/> object that produces a <see cref="StatusCodes.Status201Created"/> response.
        ///// </summary>
        ///// <param name="actionName">The name of the action to use for generating the URL.</param>
        ///// <param name="routeValues">The route data to use for generating the URL.</param>
        ///// <param name="value">The content value to format in the entity body.</param>
        ///// <returns>The created <see cref="CreatedAtActionResult"/> for the response.</returns>
        //[NonAction]
        //public virtual CreatedAtActionResult CreatedAtAction(string? actionName, object? routeValues, [ActionResultObjectValue] object? value)
        //    => CreatedAtAction(actionName, controllerName: null, routeValues: routeValues, value: value);
    }

    //PUT. api/Clientes/5
    [HttpPut]
    public async Task<IActionResult> PutCliente(int id, Cliente cliente) {
        //1. Verificar que el Id de la url coincide con el id del Objeto
        if (id != cliente.Id) {
            return BadRequest(); // Código 400: Los datos enviados son incosistentes
        }

        //¿Qué es _context.Entry(cliente) ? 🔍

        //El método _context.Entry(objeto) te da acceso a la pista de auditoría o estado
        //de seguimiento(Change Tracker) de EF Core para un objeto específico.

        //🧠 Change Tracker(El Rastreador de Cambios)

        //EF Core usa un componente llamado Change Tracker que funciona como un policía
        //que vigila todos los objetos que has cargado desde la base de datos o que has
        //añadido.

        //El Change Tracker sabe:

        //Cuál era el valor original del objeto.

        //Si el objeto ha sido modificado.

        //Si el objeto es nuevo o debe ser eliminado.

        //🎯 Cuándo se usa

        //Usas _context.Entry(objeto) cuando tienes un objeto en la memoria(RAM) y
        //necesitas informar a EF Core sobre el estado de ese objeto antes de llamar a
        //SaveChangesAsync()./* CSS Document */

        //2. Marcar la entidad como modificada
        _context.Entry(cliente).State = EntityState.Modified;
        try {
            //3. Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Clientes.Any(e => e.Id == id)) {
                return NotFound();
                /*
                 * "Si NO es verdad que existe (Any) algún Cliente (e) 
                 * en la base de datos tal que (=>) el Id de ese Cliente (e.Id) 
                 * es igual al Id que me llegó (id), entonces devuelve 404 Not Found."
                 */
            }
            else
                throw;
        }
        return NoContent(); // Código 204: Éxito, pero no hay cuerpo de respuesta.
    }

    //DELETE: api/Clientes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(int id) {
        //1. Buscar el cliente por ID
        var cliente = await _context.Clientes.FindAsync(id);

        //2. Si no existe, devolver 404
        if (cliente == null) {
            return NotFound();
        }

        //3. Marcar la entidad para borrado
        _context.Clientes.Remove(cliente);
        
        //4.Ejecutar la eliminación en la base de datos.
        await _context.SaveChangesAsync();

        //5. Devolver una respuesta exitosa sin contenido
        return NoContent(); //Código 204: Éxito, pero no hay cuerpo de respuesta.
    }
}