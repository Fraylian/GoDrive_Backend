using MediatR;
using FluentValidation;
using Dominio.Entidades;
using Microsoft.AspNetCore.Identity;
using Aplicacion.Seguridad;
using Aplicacion.Seguridad.Usuario;
using Aplicacion.Seguridad.Response;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Metodos.Usuario
{
    public class Login
    {
        public class Modelo: IRequest<ResponseModel>
        {
            public string email { get; set; }

            public string password { get; set; }
        }

        public class Validator: AbstractValidator<Modelo>
        {
            public Validator()
            {
                RuleFor(x => x.email).NotEmpty().WithMessage("Ingrese el correo");
                RuleFor(x => x.password).NotEmpty().WithMessage("Ingrese la contraseña");
            }
        }

        public class Manejador : IRequestHandler<Modelo, ResponseModel>
        {
            private readonly UserManager<Usuarios> _userManager;
            private readonly SignInManager<Usuarios> _signInManager;
            private readonly ITokenUsuario _tokenUsuario;

            public Manejador(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, ITokenUsuario tokenUsuario)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _tokenUsuario = tokenUsuario;
            }

            public async Task<ResponseModel> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.email);
                if (usuario == null)
                {
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound,null, "No se encontro el usuario");
                    
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.password, false);

                if (resultado.Succeeded)
                {
                    var datos = new UsuarioData
                    {
                        nombre = usuario.nombre,
                        apellido = usuario.apellido,
                        email = usuario.Email,
                        Token = _tokenUsuario.CrearToken(usuario)
                    };

                    return ResponseService.Respuesta(StatusCodes.Status200OK, datos);   
                }
                return ResponseService.Respuesta(StatusCodes.Status401Unauthorized,null, "La contraseña es incorrecta");
                

            }
        }
    }
}
