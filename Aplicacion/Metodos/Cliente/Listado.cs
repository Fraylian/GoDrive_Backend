using MediatR;
using Persistencia.Context;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cliente
{
    public class Listado
    {
        public class ListaClientes: IRequest<object> { }

        public class Manejador : IRequestHandler<ListaClientes, object>    
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<object> Handle(ListaClientes request, CancellationToken cancellationToken)
            {
                var clientes = await _context.clientes.Select(c => new
                {

                    c.id,
                    c.nombre,
                    c.apellido,
                    c.correo,
                    c.numero_identificacion,
                    c.tipo_identificacion


                }).ToListAsync();

                if (clientes == null ||!clientes.Any())
                {
                    return new KeyNotFoundException("No hay clientes registrados");
                }
                return clientes;
            }
        }
    }
}
