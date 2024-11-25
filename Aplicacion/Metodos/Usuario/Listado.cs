using MediatR;
using Persistencia.Context;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Seguridad.Response;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Metodos.Usuario
{
    public class Listado
    {
        public class ListaUsuarios: IRequest<ResponseModel> { }

        public class Manejador : IRequestHandler<ListaUsuarios, ResponseModel>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<ResponseModel> Handle(ListaUsuarios request, CancellationToken cancellationToken)
            {
                var usuarios = await _context.Users.ToListAsync();

                if (usuarios == null || !usuarios.Any())
                {
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound, null, "No hay usuarios disponibles.");
                }
    

                return ResponseService.Respuesta(StatusCodes.Status200OK, usuarios);

            }
        }
    }
}
