using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

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
}