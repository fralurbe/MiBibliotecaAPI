using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiBibliotecaAPI.Data;

namespace MiBibliotecaAPI.Models.DTOs {
    //📦 ¿Qué es un DTO (Data Transfer Object)?
    //   Un DTO (Objeto de Transferencia de Datos) es simplemente una clase de C#
    //   cuyo único propósito es enviar y recibir datos específicos entre tu
    //   aplicación y el mundo exterior (el cliente web, una aplicación móvil, etc.).     

    //🛡️ ¿Por Qué Necesitas DTOs?
    //Hay dos razones principales por las que nunca debes usar tu objeto de
    //dominio(Producto.cs) directamente en los métodos POST o PUT de tu API:
    //1. Seguridad y Exposición(DTOs de Salida: GET)
    //Si un objeto Usuario tiene campos como PasswordHash o Salario,
    //al devolver el objeto completo en un GET, los expones accidentalmente
    //en la respuesta JSON.
    //    Solución: Creas un UsuarioPublicoDto que omite esos campos sensibles.

    //2. Prevención de Sobreescritura (DTOs de Entrada: POST/PUT)
    //Cuando el cliente envía datos para crear o actualizar un producto(POST/PUT),
    //solo quieres que te envíe los campos modificables.
    //Problema: Si el cliente malicioso incluye el campo Id o FechaCreacion en el JSON
    //y tú usas el objeto de dominio completo (Producto producto), podría sobrescribir valores importantes.

    //Solución: Creas un CrearProductoDto que NO contiene el Id. Así,
    //el cliente solo puede enviar el Nombre y el Precio.

    public class ProductoDetalleDto {
        public int Id { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Precio { get; set; }

        // Propiedad proyectada: Nombre de la Categoria
        public string NombreCategoria { get; set; } = string.Empty;
    }
}