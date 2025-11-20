using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase {
    private readonly ApplicationDbContext _context;
    public PedidosController(ApplicationDbContext context) {
        _context = context;
    }

    //GET: api/Pedidos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos() {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .ToListAsync();
        //Eager Loading, utilizas el método .Include() en tu consulta LINQ:
        //Se utiliza para evitar el problema de rendimiento conocido como "N+1 Query Problem".
    }

    //GET: api/Pedidos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Pedido>> GetPedido(int id) {
        var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null) return NotFound();
        return pedido;
    }

    //POST. api/Pedidos
    [HttpPost]
    public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido){
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        //Cargar el cliente para que aparezca en la respuesta 201 created
        await _context.Entry(pedido).Reference(p => p.Cliente).LoadAsync();
        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        /*
         Resumen del Flujo
            POST: Cliente envía datos.
            SaveChangesAsync: Datos guardados en la BD (BD tiene el cliente, el objeto en memoria no).
            LoadAsync: Se ejecuta una segunda consulta SQL para traer los datos del cliente recién enlazado.
            CreatedAtAction: La respuesta es enviada al cliente con el código 201 Created y el objeto Pedido
            completo, incluyendo la información anidada del Cliente.
         */
    }
}