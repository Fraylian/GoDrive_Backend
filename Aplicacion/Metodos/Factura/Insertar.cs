using MediatR;
using FluentValidation;
using Persistencia.Context;
using Dominio.Entidades;

namespace Aplicacion.Metodos.Factura
{
    public class Insertar
    {
        public class Modelo: IRequest
        {
            public Guid id_cliente { get; set; }
            public String id_usuario { get; set; }
            public DateTime fecha_creacion { get; set; }
            public decimal monto_total { get; set; }
            public decimal subtotal { get; set; }
            public decimal monto_itbis { get; set; }
            public DateTime fecha_renta_inicio { get; set; }
            public DateTime fecha_renta_final { get; set; }
        }


        public class Validador: AbstractValidator<Modelo>
        {
            public Validador()
            {
                RuleFor(x => x.id_cliente).NotEmpty().WithMessage("El cliente debe ser ingresado");

                RuleFor(x => x.id_usuario).NotEmpty().WithMessage("El usuario debe ser ingresado");

                RuleFor(x => x.fecha_renta_inicio).GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de renta inicial no puede ser anterior al día de hoy");

                RuleFor(x => x.fecha_renta_final).GreaterThanOrEqualTo(x => x.fecha_renta_inicio)
                .WithMessage("La fecha final debe ser mayor o igual a la renta inicial");

                RuleFor(x => x.subtotal).GreaterThan(0).WithMessage("El subtotal debe ser mayor a 0")
                .ScalePrecision(2,10).WithMessage("El subtotal no puede tener mas de 2 decimales");

                RuleFor(x => x.monto_itbis).GreaterThan(0).WithMessage("El monto itbis debe ser mayor a 0")
                .ScalePrecision(2,10).WithMessage("el itbis no puede tener mas de 2 decimales");

                RuleFor(x => x.monto_total)
                .ScalePrecision(2,10).WithMessage("El monto total no puede tener mas de 2 decimales")
                .GreaterThanOrEqualTo(x => x.subtotal).WithMessage("El monto total debe ser mayor o igual que el subtotal");
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
                var factura = new factura
                {
                    id_cliente = request.id_cliente,
                    id_usuario = request.id_usuario,
                    fecha_creacion = DateTime.Today,
                    fecha_renta_inicio = request.fecha_renta_inicio,
                    fecha_renta_final = request.fecha_renta_final,
                    monto_itbis = request.monto_itbis,
                    monto_total = request.monto_total,
                    subtotal = request.subtotal
                };

                 _context.factura.Add(factura);

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
