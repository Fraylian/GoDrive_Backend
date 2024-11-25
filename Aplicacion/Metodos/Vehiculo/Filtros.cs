using MediatR;
using FluentValidation;
using Persistencia.Context;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Vehiculo
{
    public class Filtros
    {
        public class Modelo
        {
            public string Marca { get; set; }
            public string modelo { get; set; }
            public string transmision { get; set; }
            public int? year { get; set; }
            public int numero_Puertas { get; set; }
            public int numero_asientos { get; set; }
            public decimal costo_por_dia { get; set; }
            public bool rentado { get; set; }
            public string descripcion { get; set; }
            public List<string> Imagenes { get; set; }
        }
        public class Parametros: IRequest<ResponseModel>
        {
            public string? Marca { get; set; }
            public string? modelo { get; set; }
            public string? transmision { get; set; }
            public int? year { get; set; }
        }

        public class Validador: AbstractValidator<Parametros>
        {
            public Validador()
            {
               RuleFor(x => x.modelo)
                .MaximumLength(50).WithMessage("El modelo solo permite un máximo de 50 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.modelo)); 

                RuleFor(x => x.Marca)
                    .Must(value => Marcas.ListaMarcas.Contains(value))
                    .WithMessage("La marca insertada no pertenece a la lista de marcas disponibles")
                    .When(x => !string.IsNullOrEmpty(x.Marca)); 

                RuleFor(x => x.transmision)
                    .Must(value => Enum.TryParse<tipo_transmision>(value, true, out _))
                    .WithMessage("La transmisión debe ser Manual o Automatica")
                    .Length(1, 15).WithMessage("La transmisión solo permite un máximo de 15 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.transmision)); 

                RuleFor(x => x.year)
                    .InclusiveBetween(1900, DateTime.Now.Year)
                    .WithMessage($"El año debe estar entre 1900 y {DateTime.Now.Year}.")
                    .When(x => x.year.HasValue);
            }
        }

        public class Manejador : IRequestHandler<Parametros, ResponseModel>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<ResponseModel> Handle(Parametros request, CancellationToken cancellationToken)
            {
                var Filtros = _context.vehiculos.Include(v => v.imagenes).AsQueryable();

                if (!String.IsNullOrEmpty(request.Marca))
                {
                    Filtros = Filtros.Where(v => v.Marca.Contains(request.Marca));
                }
                if (!String.IsNullOrEmpty(request.modelo))
                {
                    Filtros = Filtros.Where(v => v.Modelo.Contains(request.modelo));
                }
                if (request.year.HasValue)
                {
                    Filtros = Filtros.Where(v => v.year == request.year);
                }
                if (!String.IsNullOrEmpty(request.transmision))
                {
                    Filtros = Filtros.Where(v => v.transmision.Equals(request.transmision));
                }

                var listaVehiculos = await Filtros.ToListAsync();

                if (listaVehiculos == null || !listaVehiculos.Any())
                {
                   return ResponseService.Respuesta(StatusCodes.Status404NotFound, null, "No se encontro ningun vehiculo con los criterios especificados");
                }
                var respuesta =  listaVehiculos.Select(v => new Modelo
                {
                    Marca = v.Marca,
                    modelo = v.Modelo,
                    year = v.year,
                    numero_Puertas = v.numero_Puertas,
                    numero_asientos = v.numero_asientos,
                    costo_por_dia = v.costo_por_dia,
                    rentado = v.rentado,
                    descripcion = v.descripcion,
                    Imagenes = v.imagenes != null && v.imagenes.Any()
                    ? v.imagenes.Select(i => Convert.ToBase64String(i.Data)).ToList()
                    : new List<string>()
                }).ToList();

                return ResponseService.Respuesta(StatusCodes.Status200OK,respuesta);
            }
        }
    }
}
