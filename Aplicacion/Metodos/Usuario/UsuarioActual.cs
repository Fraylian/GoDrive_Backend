using Aplicacion.Seguridad.Usuario;
using Microsoft.AspNetCore.Identity;
using Aplicacion.Seguridad;
using MediatR;
using Dominio.Entidades;

namespace Aplicacion.Metodos.Usuario
{
    public class UsuarioActual
    {
        public class Modelo: IRequest<UsuarioData> { }

        public class Manejador : IRequestHandler<Modelo, UsuarioData>
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

            public async Task<UsuarioData> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
                if(usuario == null)
                {
                    throw new KeyNotFoundException("No se encontro el usuario");
                }
                return new UsuarioData
                {
                    nombre = usuario.nombre,
                    apellido = usuario.apellido,
                    email = usuario.Email,
                    Token = _tokenUsuario.CrearToken(usuario)
                };
                
                
            }
        }
    }
}
