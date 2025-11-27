using AutoMapper;
using MiBibliotecaAPI.Models;
using MiBibliotecaAPI.Models.DTOs;

namespace MiBibliotecaAPI.Mappers {
    public class MappingProfile : Profile {

        public MappingProfile() {
            // 1. DTO DE ENTRADA (CrearProductoDto -> Producto)
            // Regla de POST: Mapea los campos del DTO a la Entidad Producto.
            CreateMap<CrearProductoDto, Producto>();

            //2. DTO DE SALIDA (Producto -> ProductoResumenDto)
            // Regla de GET: Mapea la Entidad Producto al DTO de Resumen.
            // Aquí le decimos cómo obtener la Categoría, si no coinciden los nombres.
            CreateMap<Producto, ProductoResumenDto>()
                // Le indicamos que el campo 'NombreDeCategoria del DTO
                // debe obtenerse de 'p.Categoria.Nombre' de la Entidad.
                .ForMember(dest => dest.NombreDeCategoria,
                    opt => opt.MapFrom(src => src.Categoria.Nombre));

            // 3. DTO DE SALIDA ANIDADO (Pedido -> PedidoDetalleDto)
            // Regla para el GET de Pedidos (usando la relación de DTOs)
            CreateMap<Pedido, PedidoDetalleDto>()
                // Mapea la coleccion de Productos (Entidad) a la coleccion de ProductoResumenDto (DTO)
                // AutoMapper es lo suficientemente inteligente para usar la regla 2 anterior.
                .ForMember(dest => dest.Productos,
                           opt => opt.MapFrom(src => src.Productos));
        }
    }
}
