using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Aplicacion.Metodos.Factura
{
    public class Listado
    {
        public class FacturaDto
        {
            public int Id { get; set; }
            public string cliente { get; set; }
            public string usuario { get; set; }
            public string fecha_creacion { get; set; }
            public string fecha_renta_inicio { get; set; }
            public string fecha_renta_final { get; set; }
            public decimal monto_total { get; set; }
            public decimal subtotal { get; set; }
            public decimal monto_itbis { get; set; }
        }
        public class Listado_Facturas: IRequest<List<FacturaDto>> { }

        public class Manejador : IRequestHandler<Listado_Facturas, List<FacturaDto>>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }
            public async Task<List<FacturaDto>> Handle(Listado_Facturas request, CancellationToken cancellationToken)
            {
                var facturas = await (from f in _context.factura
                                      join c in _context.clientes on f.id_cliente equals c.id
                                      select new FacturaDto
                                      {
                                          Id = f.id,
                                          cliente = $"{c.nombre} {c.apellido}",
                                          fecha_creacion = f.fecha_creacion.ToString("dd/MM/yy"),
                                          fecha_renta_inicio = f.fecha_renta_inicio.ToString("dd/MM/yy"),
                                          fecha_renta_final = f.fecha_renta_final.ToString("dd/MM/yy"),
                                          monto_itbis = f.monto_itbis,
                                          monto_total = f.monto_total,
                                          subtotal = f.subtotal

                                      }).ToListAsync();

                if (facturas == null)
                {
                    throw new KeyNotFoundException("No hay facturas disponibles");
                }
                return facturas;
            }
        }
    }
}
