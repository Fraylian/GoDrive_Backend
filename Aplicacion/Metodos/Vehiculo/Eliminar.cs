using Persistencia.Context;
using MediatR;


namespace Aplicacion.Metodos.Vehiculo
{
    public class Eliminar
    {
        public class Modelo: IRequest
        {
            public int Id { get; set; }
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
                var vehiculo = await _context.vehiculos.FindAsync(request.Id);
                if(vehiculo == null)
                {
                    throw new KeyNotFoundException("No se encotro el vehiculo");
                }

                _context.vehiculos.Remove(vehiculo);
                var resultado = await _context.SaveChangesAsync();
                if(resultado > 0)
                {
                    return Unit.Value;
                }
                throw new InvalidOperationException("No se pudo eliminar el vehículo.");


            }
        }
    }
}
