using MediatR;
using FluentValidation;
using Persistencia.Context;
using Dominio.Entidades;
using Dominio.Enums;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Metodos.Vehiculo
{
    public class Insertar
    {
        public class modeloVehiculos: IRequest
        {
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
            public IFormFile? imagen { get; set; }
        }

        public class Validator: AbstractValidator<modeloVehiculos>
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
                    .Must(value => Enum.TryParse<tipo_transmision>(value,true, out _))
                    .WithMessage("La transmisión debe ser Manual o Automatica")
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

        public class Manejador : IRequestHandler<modeloVehiculos>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(modeloVehiculos request, CancellationToken cancellationToken)
            {
                byte[]? imagenBytes = null;
                if (request.imagen != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.imagen.CopyToAsync(memoryStream);
                        imagenBytes = memoryStream.ToArray();
                    }
                }

                var vehiculo = new Vehiculos
                {
                    Matricula = request.Matricula,
                    Marca = request.Marca,
                    Modelo = request.Modelo,
                    transmision = request.transmision,
                    year = request.year,
                    numero_Puertas = request.numero_Puertas,
                    numero_asientos = request.numero_asientos,
                    costo_por_dia = request.costo_por_dia,
                    rentado = request.rentado,
                    descripcion = request.descripcion,
                    imagen = imagenBytes
                };

                _context.vehiculos.Add(vehiculo);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el vehiculo");
            }

        }
    }
}
