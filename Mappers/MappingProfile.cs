using AutoMapper;
using MiBibliotecaAPI.Models;
using MiBibliotecaAPI.Models.DTOs;
using System.Linq; // Necesario para usar .Select() en las colecciones

namespace MiBibliotecaAPI.Mappers {
    public class MappingProfile : Profile {
        public MappingProfile() {
            // 1. DTO DE ENTRADA (CrearProductoDto -> Producto)
            CreateMap<CrearProductoDto, Producto>();

            // 2. DTO DE SALIDA (Producto -> ProductoResumenDto)
            // Esta regla ya estaba perfecta.
            CreateMap<Producto, ProductoResumenDto>()
                .ForMember(dest => dest.NombreDeCategoria,
                    opt => opt.MapFrom(src => src.Categoria.Nombre));

            // 3. DTO DE SALIDA ANIDADO (Pedido -> PedidoDetalleDto)
            // ¡ESTA ES LA PARTE CORREGIDA!
            CreateMap<Pedido, PedidoDetalleDto>()
                // El destino es 'Productos' (List<ProductoResumenDto>)
                .ForMember(dest => dest.Productos,
                    // La fuente ahora navega a través de 'Detalles' para obtener la lista de Productos.
                    // Esto devuelve List<Producto>, que AutoMapper convierte usando la Regla 2.
                    opt => opt.MapFrom(src => src.Detalles.Select(d => d.Producto)));
        }
    }
}