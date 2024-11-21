using Persistencia.Context;
using Dominio.Entidades;
using Aplicacion.Metodos.Email;
using MediatR;
using FluentValidation;

namespace Aplicacion.Metodos.Factura
{
    public class CrearFactura
    {
        public class ModeloFactura: IRequest
        {
            public Guid id_cliente { get; set; }
            public DateTime fecha_creacion { get; set; }
            public decimal monto_total { get; set; }
            public decimal subtotal { get; set; }
            public decimal monto_itbis { get; set; }
            public DateTime fecha_renta_inicio { get; set; }
            public DateTime fecha_renta_final { get; set; }
            public List<ModeloDetalles> detalles { get; set; }
        }

        public class ModeloDetalles
        {
            public int vehiculo_id { get; set; }
            public decimal costo_por_dia { get; set; }
            public int dias_rentados { get; set; }
            public decimal costo_total_vehiculo { get; set; }
        }

        public class Validador: AbstractValidator<ModeloFactura>
        {
            public Validador()
            {
                RuleFor(x => x.id_cliente).NotEmpty().WithMessage("El cliente debe ser ingresado");

                RuleFor(x => x.fecha_renta_inicio).GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de renta inicial no puede ser anterior al día de hoy");

                RuleFor(x => x.fecha_renta_final).GreaterThanOrEqualTo(x => x.fecha_renta_inicio)
                .WithMessage("La fecha final debe ser mayor o igual a la renta inicial");

                RuleFor(x => x.subtotal).GreaterThan(0).WithMessage("El subtotal debe ser mayor a 0")
                .ScalePrecision(2, 10).WithMessage("El subtotal no puede tener mas de 2 decimales");

                RuleFor(x => x.monto_itbis).GreaterThan(0).WithMessage("El monto itbis debe ser mayor a 0")
                .ScalePrecision(2, 10).WithMessage("el itbis no puede tener mas de 2 decimales");

                RuleFor(x => x.monto_total)
                .ScalePrecision(2, 10).WithMessage("El monto total no puede tener mas de 2 decimales")
                .GreaterThanOrEqualTo(x => x.subtotal).WithMessage("El monto total debe ser mayor o igual que el subtotal");
            }
        }

        public class Manejador : IRequestHandler<ModeloFactura>
        {
            private readonly ProyectoContext _context;
            private readonly IEmailSender _emailSender;

            public Manejador(ProyectoContext context, IEmailSender emailSender)
            {
                _context = context;
                _emailSender = emailSender;
            }

            public async Task<Unit> Handle(ModeloFactura request, CancellationToken cancellationToken)
            {
                var factura = new factura
                {
                    id_cliente = request.id_cliente,
                    fecha_creacion = DateTime.Now,
                    fecha_renta_inicio = request.fecha_renta_inicio,
                    fecha_renta_final = request.fecha_renta_final,
                    monto_itbis = request.monto_itbis,
                    subtotal = request.subtotal,
                    monto_total = request.monto_total
                };

                _context.factura.Add(factura);
                var resultadoFactura = await _context.SaveChangesAsync();

                if (resultadoFactura <= 0)
                {
                    throw new InvalidOperationException("No se pudo crear la factura.");
                }

                foreach (var detalle in request.detalles)
                {
                    var facturaDetalle = new factura_detalle
                    {
                        factura_id = factura.id,
                        vehiculo_id = detalle.vehiculo_id,
                        costo_por_dia = detalle.costo_por_dia,
                        dias_rentados = detalle.dias_rentados,
                        costo_total_vehiculo = detalle.costo_total_vehiculo
                    };

                    _context.factura_Detalles.Add(facturaDetalle);
                }

                var resultadoDetalles = await _context.SaveChangesAsync();

                if (resultadoDetalles <= 0)
                {
                    throw new InvalidOperationException("No se pudieron crear los detalles de la factura.");
                }
                return Unit.Value;
            }
        }
    }
}
