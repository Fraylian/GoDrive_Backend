using Dominio.Entidades;
using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Vehiculo
{
    public class listado
    {
        public class Modelo
        {
            public int Id { get; set; }
            public string Marca { get; set; }
            public string modelo { get; set; }
            public string transmision { get; set; }
            public int year { get; set; }
            public int numero_Puertas { get; set; }
            public int numero_asientos { get; set; }
            public decimal costo_por_dia { get; set; }
            public bool rentado { get; set; }
            public string descripcion { get; set; }
            public List<string> Imagenes { get; set; }

        }
        public class ListaVehiculos : IRequest<List<Modelo>> { }
        public class Manejador : IRequestHandler<ListaVehiculos, List<Modelo>>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<List<Modelo>> Handle(ListaVehiculos request, CancellationToken cancellationToken)
            {
                var vehiculos = await _context.vehiculos
                .Include(v => v.imagenes)
                .Select(v => new Modelo
                {
                    Id = v.id,
                    Marca = v.Marca,
                    modelo = v.Modelo,
                    year = v.year,
                    transmision = v.transmision,
                    numero_Puertas = v.numero_Puertas,
                    numero_asientos = v.numero_asientos,
                    costo_por_dia = v.costo_por_dia,
                    rentado = v.rentado,
                    descripcion = v.descripcion,
                    Imagenes = v.imagenes != null && v.imagenes.Any()
                    ? v.imagenes.Select(i => Convert.ToBase64String(i.Data)).ToList()
                    : new List<string>()

                }).ToListAsync();


                if (!vehiculos.Any())
                {
                    throw new KeyNotFoundException("No hay vehículos disponibles.");
                }

                return vehiculos;
            }
        }
    }
}
