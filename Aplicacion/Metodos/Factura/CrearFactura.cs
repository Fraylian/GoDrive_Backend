using Persistencia.Context;
using Dominio.Entidades;
using Aplicacion.Metodos.Email;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;

namespace Aplicacion.Metodos.Factura
{
    public class CrearFactura
    {
        public class Modelo : IRequest<ResponseModel>
        {
            public Guid id_cliente { get; set; }
            public DateTime fecha_creacion { get; set; }
            public DateTime fecha_renta_inicio { get; set; }
            public DateTime fecha_renta_final { get; set; }
            public int vehiculo_id { get; set; }
        }

        public class Validador: AbstractValidator<Modelo>
        {
            public Validador()
            {
                RuleFor(x => x.id_cliente).NotEmpty().WithMessage("El cliente debe ser ingresado");

                RuleFor(x => x.fecha_renta_inicio).GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de renta inicial no puede ser anterior al día de hoy");

                RuleFor(x => x.fecha_renta_final).GreaterThan(x => x.fecha_renta_inicio)
                .WithMessage("La fecha final debe ser mayor a la renta inicial");

                RuleFor(x => x.vehiculo_id).NotEmpty().WithMessage("El id del vehículo debe ser ingresado")
               .GreaterThan(0).WithMessage("El id del vehículo debe ser un entero positivo");
            }
        }

        public class Manejador : IRequestHandler<Modelo, ResponseModel>
        {
            private readonly ProyectoContext _context;
            private readonly FacturaEmailService _facturaEmailService;

           public Manejador(ProyectoContext context, FacturaEmailService facturaEmailService)
            {
                _context = context;
                _facturaEmailService = facturaEmailService;
            }

            public async Task<ResponseModel> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var vehiculo = await _context.vehiculos.Where(v => v.id == request.vehiculo_id).FirstOrDefaultAsync();
                if (vehiculo == null)
                {
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound, null, "No se encontro el vehiculo");
                    
                }

                if (vehiculo.rentado)
                {
                    return ResponseService.Respuesta(StatusCodes.Status409Conflict, null, "El vehículo ya está rentado.");
                    
                }

                int diasRentados = (request.fecha_renta_final - request.fecha_renta_inicio).Days;
                if (diasRentados <= 0)
                {
                    return ResponseService.Respuesta(StatusCodes.Status400BadRequest, null, "La fecha de renta final debe ser posterior a la inicial.");
                    
                }

                decimal subtotal = diasRentados * vehiculo.costo_por_dia;

                
                decimal itbis = subtotal * 0.18m;

                
                decimal montoTotal = subtotal + itbis;

                var ultimaFactura = await _context.factura.OrderByDescending(f => f.id).FirstOrDefaultAsync();
                string numeroFactura = ultimaFactura == null
                    ? "FAC-0001"
                    : $"FAC-{(int.Parse(ultimaFactura.numero_factura.Split('-')[1]) + 1):D4}";

                var factura = new factura
                {
                    id_cliente = request.id_cliente,
                    fecha_creacion = DateTime.Now,
                    fecha_renta_inicio = request.fecha_renta_inicio,
                    fecha_renta_final = request.fecha_renta_final,
                    monto_itbis = itbis,
                    subtotal = subtotal,
                    monto_total = montoTotal,
                    numero_factura = numeroFactura,
                };

                _context.factura.Add(factura);
                var resultadoFactura = await _context.SaveChangesAsync();

                if (resultadoFactura <= 0)
                {
                    return ResponseService.Respuesta(StatusCodes.Status500InternalServerError,null, "No se pudo crear la factura.");
                   
                }

                var facturaDetalle = new factura_detalle
                {
                    factura_id = factura.id,
                    vehiculo_id = vehiculo.id,
                    costo_por_dia = vehiculo.costo_por_dia,
                    dias_rentados = diasRentados,
                    costo_total_vehiculo = subtotal

                };

               

                _context.factura_Detalles.Add(facturaDetalle);


                vehiculo.rentado = true;
                _context.vehiculos.Update(vehiculo);



                var resultadoDetalles = await _context.SaveChangesAsync();

                if (resultadoDetalles <= 0)
                {
                    return ResponseService.Respuesta(StatusCodes.Status500InternalServerError, null, "No se pudieron crear los detalles de la factura.");
                    
                }
                string mensajeCorreo = "La factura se creó correctamente.";
                try
                {
                    await _facturaEmailService.EnviarCorreoFactura(factura, facturaDetalle, vehiculo);
                    mensajeCorreo += " El correo de la factura fue enviado exitosamente.";
                }
                catch (Exception)
                {

                    mensajeCorreo += " Sin embargo, no se pudo enviar el correo de la factura.";
                }

             
                return ResponseService.Respuesta(StatusCodes.Status201Created, null,mensajeCorreo);
            }
        }
    }
}
