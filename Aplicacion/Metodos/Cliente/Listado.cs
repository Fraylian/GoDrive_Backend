using MediatR;
using Persistencia.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;

namespace Aplicacion.Cliente
{
    public class Listado
    {
        public class ListaClientes: IRequest<ResponseModel> { }

        public class Manejador : IRequestHandler<ListaClientes, ResponseModel>    
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<ResponseModel> Handle(ListaClientes request, CancellationToken cancellationToken)
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
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound,null, "No hay clientes registrados");
                }
                return ResponseService.Respuesta(StatusCodes.Status200OK,clientes);
            }
        }
    }
}
