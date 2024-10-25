using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Factura
{
    public class Consultar
    {
       public class parametro : IRequest<object>
        {
            public int id { get; set; }
        }
        
        public class Modelo
        {
            
            public string cliente { get; set; }
            public string usuario { get; set; }
            public string fecha_creacion { get; set; }
            public string fecha_renta_inicio { get; set; }
            public string fecha_renta_final { get; set; }
            public decimal monto_total { get; set; }
            public decimal subtotal { get; set; }
            public decimal monto_itbis { get; set; }
        }

        public class Manejador : IRequestHandler<parametro, object>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<object> Handle(parametro request, CancellationToken cancellationToken)
            {
                var factura = await (from f in _context.factura where f.id == request.id
                               join c in _context.clientes on f.id_cliente equals c.id
                               join u in _context.usuarios on f.id_usuario equals u.Id
                               select new Modelo
                               {
                                   cliente = $"{c.nombre} {c.apellido}",
                                   usuario = $"{u.nombre} {u.apellido}",
                                   fecha_creacion = f.fecha_creacion.ToString("dd/MM/yy"),
                                   fecha_renta_inicio = f.fecha_renta_inicio.ToString("dd/MM/yy"),
                                   fecha_renta_final = f.fecha_renta_final.ToString("dd/MM/yy"),
                                   monto_itbis = f.monto_itbis,
                                   monto_total = f.monto_total,
                                   subtotal = f.subtotal


                               }).FirstOrDefaultAsync();
                if(factura == null)
                {
                    throw new KeyNotFoundException("No se encontro la factura");
                }

                return factura;

            }
        }
    }
}
