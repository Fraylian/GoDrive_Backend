using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;

namespace Aplicacion.Metodos.Cliente
{
    public class Consulta
    {
        public class Modelo: IRequest<ResponseModel>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Modelo, ResponseModel>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<ResponseModel> Handle(Modelo request, CancellationToken cancellationToken)
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
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound,null, "No se encontro el cliente");
                    
                }

                return ResponseService.Respuesta(StatusCodes.Status200OK, cliente);
            }
        }
    }
}
