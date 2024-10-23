using MediatR;
using FluentValidation;
using Persistencia.Context;
using Dominio.Entidades;
namespace Aplicacion.Metodos.Vehiculo
{
    public class Actualizar
    {
        public class modelo: IRequest
        {
            public int id { get; set; }
            public string Matricula { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string transmision { get; set; }
            public int year { get; set; }
            public int numero_Puertas { get; set; }
            public int numero_asientos { get; set; }
            public decimal costo_por_dia { get; set; }
            public bool rentado { get; set; }
            public string descripcion { get; set; }
            public string? imagen { get; set; }

        }

        public class Validator : AbstractValidator<modelo>
        {
            public Validator()
            {
                RuleFor(x => x.Matricula)
               .NotEmpty().WithMessage("La matrícula es requerida.")
               .Length(1, 10).WithMessage("La matrícula debe tener un máximo de 10 caracteres.");


                RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("La marca es requerida.")
                .Length(1, 50).WithMessage("La marca debe tener un máximo de 50 caracteres.");


                RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("El modelo es requerido.")
                .Length(1, 50).WithMessage("El modelo debe tener un máximo de 50 caracteres.");


                RuleFor(x => x.transmision)
                .NotEmpty().WithMessage("La transmisión es requerida.")
                .Length(1, 15).WithMessage("La transmisión debe tener un máximo de 15 caracteres.");


                RuleFor(x => x.year)
                .InclusiveBetween(1900, DateTime.Now.Year).WithMessage($"El año debe estar entre 1900 y {DateTime.Now.Year}.");


                RuleFor(x => x.numero_Puertas)
                .InclusiveBetween(1, 10).WithMessage("El número de puertas debe estar entre 1 y 10.");


                RuleFor(x => x.numero_asientos)
                .InclusiveBetween(1, 10).WithMessage("El número de asientos debe estar entre 1 y 10.");


                RuleFor(x => x.costo_por_dia)
                .GreaterThan(0).WithMessage("El costo por día debe ser mayor a 0.")
                .ScalePrecision(2, 10).WithMessage("El costo no puede tener mas de 2 decimales.");
            }
        }


        public class Manejador : IRequestHandler<modelo>
        {
            
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(modelo request, CancellationToken cancellationToken)
            {
                var vehiculo = await _context.vehiculos.FindAsync(request.id);
                if(vehiculo == null)
                {
                    throw new Exception("No se encontro el vehiculo");
                }
                vehiculo.Matricula = request.Matricula;
                vehiculo.Marca = request.Marca;
                vehiculo.Modelo = request.Modelo;
                vehiculo.transmision = request.transmision;
                vehiculo.year = request.year;
                vehiculo.numero_Puertas = request.numero_Puertas;
                vehiculo.numero_asientos = request.numero_asientos;
                vehiculo.costo_por_dia = request.costo_por_dia;
                vehiculo.rentado = request.rentado;
                vehiculo.descripcion = request.descripcion;
                vehiculo.imagen = request.imagen;

                _context.vehiculos.Update(vehiculo);

                var resultado = await _context.SaveChangesAsync();
                if(resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo editar los datos del vehiculo");
            }
        }
    }
}
