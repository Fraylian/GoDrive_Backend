using MediatR;
using Persistencia.Context;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cliente
{
    public class Listado
    {
        public class ListaClientes: IRequest<List<Clientes>> { }

        public class Manejador : IRequestHandler<ListaClientes, List<Clientes>>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<List<Clientes>> Handle(ListaClientes request, CancellationToken cancellationToken)
            {
                var clientes = await _context.clientes.ToListAsync();
                return clientes;
            }
        }
    }
}
