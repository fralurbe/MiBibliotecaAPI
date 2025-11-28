using MiBibliotecaAPI.Data;
using MiBibliotecaAPI.Models;
using MiBibliotecaAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase {
    private readonly ApplicationDbContext _context;
    public PedidosController(ApplicationDbContext context) {
        _context = context;
    }

    ////GET: api/Pedidos
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos() {
    //    return await _context.Pedidos
    //        .Include(p => p.Productos)
    //        .ThenInclude(p => p.Categoria)
    //        .ToListAsync();
    //    //Eager Loading, utilizas el método .Include() en tu consulta LINQ:
    //    //Se utiliza para evitar el problema de rendimiento conocido como "N+1 Query Problem".
    //}

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDetalleDto>>> GetPedidos() {
        // Usamos Include/ThenInclude para forzar a Entity Framework a traer todos los datos anidados
        // (Detalles -> Producto -> Categoria) en una sola consulta eficiente.
        return await _context.Pedidos
            .Include(p => p.Detalles)       // 1. Incluir la tabla pivote PedidoDetalle
                .ThenInclude(d => d.Producto)   // 2. Incluir el Producto asociado al detalle
                    .ThenInclude(prod => prod.Categoria) // 3. Incluir la Categoria del Producto
            .Select(
                // Proyección (Mapeo a DTO)
                p => new PedidoDetalleDto {
                    Id = p.Id,
                    TotalPedido = p.Total,

                    // Mapeamos la lista de detalles del pedido a una lista de resúmenes de productos.
                    Productos = p.Detalles.Select(
                        detalle => new ProductoResumenDto {
                            NombreProducto = detalle.Producto.Nombre,
                            PrecioUnidad = detalle.Producto.Precio,
                            // Accedemos a la categoría del producto a través del detalle.
                            NombreDeCategoria = detalle.Producto.Categoria.Nombre
                        }).ToList()
                }).ToListAsync();
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

    //POST,Crea un nuevo recurso. api/Pedidos
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

    //PUT. Actualiza o reemplaza. api/Pedidos/
    [HttpPut]
    public async Task<ActionResult> PutPedido(int id, Pedido pedido) {
        //1. Verificar si el ID de la URL coincide con el ID del objeto
        if (id != pedido.Id) {
            return BadRequest(); // 400: Datos inconsistentes.
        }

        //2. Marcar la entidad como modificada
        _context.Entry(pedido).State = EntityState.Modified;

        try {
            //3. Guardar los cambios
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            // Manejar el caso si el Pedido no existe (404)
            if (!_context.Pedidos.Any(pedido => pedido.Id == id)) {
                return NotFound(); // 404: No encontrado;
            }
            else {
                throw;
            }            
        }
        //5.Devolver 204 No Content
        return NoContent();
    }

    //DELETE. api/Pedidos/5
    [HttpDelete]
    public async Task<IActionResult> DeletePedido(int id) {
        //1. Buscar el pedido por ID
        var pedido = await _context.Pedidos.FindAsync(id);

        //2. Si no existe, devolver 404
        if (pedido == null) {
            return NotFound();
        }

        //3.Marcar la entidad para eliminacion
        _context.Pedidos.Remove(pedido);

        //4. Ejecutar la eliminacion en la base de datos
        await _context.SaveChangesAsync();

        //5. Devolver 204 No Content
        return NoContent();
    }

    // POST: api/Pedidos/Completar
    /// Procesa un pedido de forma segura. Usa una transaccion explicita para garantizar
    /// que la creación del pedido y la actualización del stock conjuntamente (atomicidad).
    [HttpPost("Completar")]
    public async Task<ActionResult> CompletarPedido(CompletarPedidoDto compra) {
        // 1.Crea un "Espacio Aparte"(Isolation)
        // La base de datos crea una versión temporal y privada de la tabla de Pedidos y
        // otras tablas afectadas.Este espacio se llama la zona de trabajo de la transacción.
        using var transaccion = await _context.Database.BeginTransactionAsync();
        try {
            //2. Crear el pedido inicial
            var nuevoPedido = new Pedido {
                ClienteId = compra.ClienteId,
                Total = 0,
            };
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            decimal totalCalculado = 0;

            //3. Procesar productos y validar/actualizar stock
            foreach (var item in compra.Items) {
                var producto = await _context.Productos.FirstOrDefaultAsync(prod =>
                                prod.Id == item.ProductoId);
                if (producto == null) {
                    throw new KeyNotFoundException($"Producto ID: {item.ProductoId} no encontrado");
                }

                //Stock insuficiente
                if (producto.Stock < item.Cantidad) {
                    //Stock insuficiente: Activa el rollback
                    throw new InvalidOperationException(
                        $"Stock insuficiente para ID: {item.ProductoId}. " +
                        $"Disponible: {producto.Stock}");
                }
                //actualizar los datos
                producto.Stock = producto.Stock - item.Cantidad;
                totalCalculado = producto.Precio * item.Cantidad;
            }

            //4. guardar cambios finales
            nuevoPedido.Total = totalCalculado;
            await _context.SaveChangesAsync();

            //5 commit, hacer los cambios en la base de datos
            await transaccion.CommitAsync();
            //public virtual CreatedAtActionResult CreatedAtAction(string? actionName, object? routeValues, [ActionResultObjectValue] object? value)
            return CreatedAtAction(nameof(GetPedidos), new { id = nuevoPedido.Id }, nuevoPedido);            
        }
        catch(Exception ex) {
            //6. Si salta excepcion volvemos hacia atras, deshacemos todo
            // 7. RESPUESTA DE ERROR
            if (ex is KeyNotFoundException ||
                ex is InvalidOperationException) {
                return BadRequest(
                    $"Error al procesar el pedido: {ex.Message}");
            }
            return StatusCode(500,
                $"Error interno. Operación revertida: {ex.Message}");
        }
    }

    [HttpPost("Cancelar/{id}")]
    public async Task<ActionResult> CancelarPedido(int id) {
        // 1. Inicia una transacción. Es una promesa a la BD: "haz todos los cambios o ninguno".
        using var transaction =
            await _context.Database.BeginTransactionAsync();

        try {
            // 2. Carga el Pedido y OBLIGATORIAMENTE sus Detalles (la tabla pivote), si existe.
            var pedidoACancelar = await _context.Pedidos
                .Include(p => p.Detalles) // Sin este Include, la propiedad Detalles sería null.
                .FirstOrDefaultAsync(p => p.Id == id);

            // 3. Validación: Si el pedido no se encuentra, devuelve 404.
            if (pedidoACancelar == null) {
                return NotFound($"El pedido con ID {id} no fue encontrado.");
            }

            // 4. Bucle para revertir el stock
            // Recorremos cada ítem (detalle) del pedido.
            foreach (var detalle in pedidoACancelar.Detalles) {
                // Buscamos la entidad Producto para poder modificar su Stock.
                var producto = await _context.Productos
                    .FindAsync(detalle.ProductoId);

                if (producto != null) {
                    // Aritmética de Reversión: Sumamos la cantidad vendida al stock.
                    producto.Stock += detalle.Cantidad;
                }
            }

            // 5. [Opcional] Actualizar el estado del Pedido (ej: a "Cancelado").
            // pedidoACancelar.Estado = "Cancelado"; 

            // 6. Guarda todos los cambios (Stock y Estado) dentro de la transacción.
            await _context.SaveChangesAsync();

            // 7. Si todo funcionó, confirma la transacción para hacer los cambios permanentes.
            await transaction.CommitAsync();

            return NoContent(); // 204: Éxito
        }
        catch (Exception ex) {
            // 8. Si algo falla (el "try"), deshace todos los cambios de la transacción.
            await transaction.RollbackAsync();

            return StatusCode(500, $"Error al cancelar el pedido: {ex.Message}");
        }
    }    
}