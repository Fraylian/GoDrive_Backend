using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Cliente
{
    public class Consulta
    {
        public class Modelo: IRequest<object>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Modelo, object>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<object> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var cliente = await _context.clientes.Where(c => c.id == request.Id).Select(c => new
                {
                    c.nombre,
                    c.apellido,
                    c.correo,
                    c.numero_identificacion,
                    c.tipo_identificacion
                }).FirstOrDefaultAsync();

                if(cliente == null)
                {
                    throw new KeyNotFoundException("No se encontro el cliente");
                }

                return cliente;
            }
        }
    }
}
