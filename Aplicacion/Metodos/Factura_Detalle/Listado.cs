using MediatR;
using Persistencia.Context;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Factura_Detalle
{
    public class Listado
    {
        public class Modelo
        {
            public int Id { get; set; }
            public string modelo { get; set; }
            public string marca { get; set; }
            public decimal costo_por_dia { get; set; }
            public int dias_rentados { get; set; }
            public decimal costo_total_vehiculo { get; set; }
            public string cliente { get; set; }
            public string fecha_creacion { get; set; }
            public string fecha_renta_inicio { get; set; }
            public string fecha_renta_final { get; set; }

        }
        public class listado_detalles_facturas: IRequest<List<Modelo>> { }

        public class Manejador : IRequestHandler<listado_detalles_facturas, List<Modelo>>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<List<Modelo>> Handle(listado_detalles_facturas request, CancellationToken cancellationToken)
            {
                var factura_detalle = await (from fd in _context.factura_Detalles
                                             join f in _context.factura on fd.factura_id equals f.id
                                             join v in _context.vehiculos on fd.vehiculo_id equals v.id
                                             join c in _context.clientes on f.id_cliente equals c.id
                                             select new Modelo
                                             {
                                               cliente = $"{c.nombre} {c.apellido}",
                                               modelo = v.Modelo,
                                               marca = v.Marca,
                                               costo_por_dia = fd.costo_por_dia,
                                               dias_rentados = fd.dias_rentados,
                                               costo_total_vehiculo = fd.costo_total_vehiculo,
                                               fecha_creacion = f.fecha_creacion.ToString("dd/MM/y"),
                                               fecha_renta_inicio = f.fecha_renta_inicio.ToString("dd/MM/y"),
                                               fecha_renta_final = f.fecha_renta_final.ToString("dd/mm/y")
                                               
                                             }).ToListAsync();

                if(factura_detalle == null)
                {
                    throw new KeyNotFoundException("No hay un lista de detalles de factura");
                }

                return factura_detalle;
                                            
                                            
            }
        }
    }
}
