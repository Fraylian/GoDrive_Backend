using Dominio.Entidades;
using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Vehiculo
{
    public class listado
    {
        public class ListaVehiculos: IRequest<List<Vehiculos>> { }
        public class Manejador : IRequestHandler<ListaVehiculos, List<Vehiculos>>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<List<Vehiculos>> Handle(ListaVehiculos request, CancellationToken cancellationToken)
            {
                var vehiculos = await _context.vehiculos.ToListAsync();
                return vehiculos;
            }
        }
    }
}
