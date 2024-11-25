using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Seguridad.Response;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Metodos.Factura
{
    public class Consultar
    {
       public class parametro : IRequest<ResponseModel>
        {
            public int id { get; set; }
        }
        
        public class Modelo
        {
            public string numero_factura { get; set; }
            public string cliente { get; set; }
            public string fecha_creacion { get; set; }
            public string fecha_renta_inicio { get; set; }
            public string fecha_renta_final { get; set; }
            public decimal monto_total { get; set; }
            public decimal subtotal { get; set; }
            public decimal monto_itbis { get; set; }
        }

        public class Manejador : IRequestHandler<parametro, ResponseModel>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<ResponseModel> Handle(parametro request, CancellationToken cancellationToken)
            {
                var factura = await (from f in _context.factura where f.id == request.id
                               join c in _context.clientes on f.id_cliente equals c.id
                               
                               select new Modelo
                               { 
                                   numero_factura = f.numero_factura,
                                   cliente = $"{c.nombre} {c.apellido}",
                                   fecha_creacion = f.fecha_creacion.ToString("dd/MM/yyyy HH:mm:ss"),
                                   fecha_renta_inicio = f.fecha_renta_inicio.ToString("dd/MM/yy"),
                                   fecha_renta_final = f.fecha_renta_final.ToString("dd/MM/yy"),
                                   monto_itbis = f.monto_itbis,
                                   monto_total = f.monto_total,
                                   subtotal = f.subtotal


                               }).FirstOrDefaultAsync();
                if(factura == null)
                {
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound, null, "No se encontro la factura");
                }

                return ResponseService.Respuesta(StatusCodes.Status200OK, factura);

            }
        }
    }
}
