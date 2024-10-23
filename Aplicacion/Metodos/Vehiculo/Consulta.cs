using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Vehiculo
{
    public class Consulta
    {
        public class VehiculoId: IRequest<Object>
        {
            public int  Id { get; set; }
        }

        public class Manejador : IRequestHandler<VehiculoId, Object>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }


            public async Task<Object> Handle(VehiculoId request, CancellationToken cancellationToken)
            {
                var vehiculo = await _context.vehiculos.Where(v => v.id == request.Id).Select(v => new
                {
                    v.Matricula,
                    v.Marca,
                    v.Modelo,
                    v.year,
                    v.transmision,
                    v.numero_Puertas,
                    v.numero_asientos,
                    v.costo_por_dia,
                    v.rentado,
                    v.descripcion,
                    v.imagen
                }).FirstOrDefaultAsync();

                if(vehiculo == null)
                {
                    throw new KeyNotFoundException("No se encontro el vehiculo");
                }

                return vehiculo;
                
            }
        }
    }
}
