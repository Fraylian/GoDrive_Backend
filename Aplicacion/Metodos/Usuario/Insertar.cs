using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistencia.Context;
using Aplicacion.Seguridad;
using Aplicacion.Seguridad.Usuario;
using Aplicacion.Seguridad.Response;
using Dominio.Entidades;

namespace Aplicacion.Metodos.Usuario
{
    public class Insertar
    {
        public class Modelo: IRequest<ResponseModel>
        {
            public string nombre { get; set; }
            public string apellido { get; set; }
            public string correo { get; set; }
            public string password { get; set; }

        }
        public class Validador: AbstractValidator<Modelo>
        {
            public Validador()
            {
                RuleFor(x => x.nombre).NotEmpty().WithMessage("El campo nombre no debe estar vacio");
                RuleFor(x => x.apellido).NotEmpty().WithMessage("El campo apellido no debe estar vacio");
                RuleFor(x => x.correo).NotEmpty().WithMessage("El campo correo no debe estar vacio").EmailAddress().WithMessage("El correo ingresado no es valido");

                RuleFor(x => x.password).NotEmpty().WithMessage("El campo contraseña no debe estar vacio")
                 .MinimumLength(6).WithMessage("la contraseña debe contener un minimo de 6 caracteres")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un numero")
                .Matches("[a-z]").WithMessage("La contraseña debe contener almenos un caracter en minuscula")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener almenos una letra mayuscula")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña de debe contener al menos un caracter especial");
            }
        }

        public class Manejador : IRequestHandler<Modelo, ResponseModel>
        {
            private readonly ProyectoContext _context;
            private readonly UserManager<Usuarios> _userManager;
            private readonly ITokenUsuario _tokenUsuario;

           public Manejador(ProyectoContext context, UserManager<Usuarios> userManager, ITokenUsuario tokenUsuario)
            {
                _context = context;
                _userManager = userManager;
                _tokenUsuario = tokenUsuario;
            }

            public async Task<ResponseModel> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var usuarioRegistrado = await _context.Users.Where(x => x.Email == request.correo).AnyAsync();

                if (usuarioRegistrado == true)
                {
                   return ResponseService.Respuesta(StatusCodes.Status409Conflict,null, "Este correo ya esta registrado");
                    
                }

                var usuario = new Usuarios
                {
                    nombre = request.nombre,
                    apellido = request.apellido,
                    Email = request.correo,
                    UserName = request.correo

                };

                var resultado = await _userManager.CreateAsync(usuario, request.password);
                if (resultado.Succeeded)
                {
                    var nuevo_usuario = new UsuarioData
                    {
                        nombre = usuario.nombre,
                        apellido = usuario.apellido,
                        email = request.correo,
                        Token = _tokenUsuario.CrearToken(usuario)
                    };
                    return ResponseService.Respuesta(StatusCodes.Status201Created,nuevo_usuario);
                }

                else
                {
                    return ResponseService.Respuesta(StatusCodes.Status500InternalServerError, null, "No se puede agregar el usuario");

                    
                }
            }
        }
    }
}
