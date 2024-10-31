using FluentValidation;
using Persistencia.Context;
using Dominio.Entidades;
using MediatR;

namespace Aplicacion.Metodos.Factura_Detalle
{
    public class Insertar
    {
        public class Modelo: IRequest
        {
            public int factura_id { get; set; }
            public int vehiculo_id { get; set; }
            public decimal costo_por_dia { get; set; }
            public int dias_rentados { get; set; }
            public decimal costo_total_vehiculo { get; set; }
        }

        public class Validador: AbstractValidator<Modelo>
        {
            public Validador()
            {
                RuleFor(x => x.factura_id).NotEmpty().WithMessage("El id factura debe ser ingresada");

                RuleFor(x => x.vehiculo_id).NotEmpty().WithMessage("El id vehiculo debe ser ingresado");

                RuleFor(x => x.costo_por_dia).GreaterThan(0)
               .WithMessage("El costo total por dia debe ser mayor a 0")
                .ScalePrecision(2,10).WithMessage("El costo por dia no puede tener mas de 2 decimales");

                RuleFor(x => x.dias_rentados).GreaterThan(1)
               .WithMessage("Los dias de rentados debe ser mator a 0");

                RuleFor(x => x.costo_total_vehiculo).GreaterThan(0).WithMessage("El costo total del vehiculo debe ser mayor a 0")
                .ScalePrecision(2,10).WithMessage("El costo total del vehiculo no puede tener mas de dos decimales.");

            }
        }

        public class Manejador : IRequestHandler<Modelo>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var factura_detalle = new factura_detalle
                {
                    factura_id = request.factura_id,
                    vehiculo_id = request.vehiculo_id,
                    costo_por_dia = request.costo_por_dia,
                    dias_rentados = request.dias_rentados,
                    costo_total_vehiculo = request.costo_total_vehiculo,
                };

                _context.factura_Detalles.Add(factura_detalle);

                var resultado = await _context.SaveChangesAsync();
                if(resultado > 0)
                {
                    return Unit.Value;
                }
                throw new InvalidOperationException("No se pudo realizar la factura");
            }
        }
    }
}
