using Aplicacion.Seguridad.Usuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;
using Aplicacion.Seguridad;
using MediatR;
using Dominio.Entidades;

namespace Aplicacion.Metodos.Usuario
{
    public class UsuarioActual
    {
        public class Modelo: IRequest<ResponseModel> { }

        public class Manejador : IRequestHandler<Modelo, ResponseModel>
        {
            private readonly UserManager<Usuarios> _userManager;
            private readonly ITokenUsuario _tokenUsuario;
            private readonly IUsuarioSesion _usuarioSesion;

            public Manejador(UserManager<Usuarios> userManager, ITokenUsuario tokenUsuario, IUsuarioSesion usuarioSesion)
            {
                _userManager = userManager;
                _tokenUsuario = tokenUsuario;
                _usuarioSesion = usuarioSesion;
            }

            public async Task<ResponseModel> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
                if(usuario == null)
                {
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound,null, "No se encontro el usuario");
                }
                var data = new UsuarioData
                {
                    nombre = usuario.nombre,
                    apellido = usuario.apellido,
                    email = usuario.Email,
                    Token = _tokenUsuario.CrearToken(usuario)
                };
                
                return ResponseService.Respuesta(StatusCodes.Status200OK,data);
                
            }
        }
    }
}
