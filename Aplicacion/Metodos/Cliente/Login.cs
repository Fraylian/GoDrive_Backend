using Dominio.Entidades;
using Microsoft.AspNetCore.Identity;
using Aplicacion.Seguridad;
using Aplicacion.Seguridad.Cliente;
using MediatR;
using FluentValidation;
using Persistencia.Context;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Seguridad.Response;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Metodos.Cliente
{
    public class Login
    {
        public class Modelo : IRequest<ResponseModel> {

            public string correo { get; set; }
            public string password { get; set; }

        }

        public class Validador: AbstractValidator<Modelo>
        {
            public Validador() {
                RuleFor(x => x.correo).NotEmpty().WithMessage("El campo correo no debe estar vacio")
                    .EmailAddress().WithMessage("El correo ingresado no es valido");
                RuleFor(x => x.password).NotEmpty().WithMessage("El campo contraseña es requerido");
            
            }
        }

        public class Manejador : IRequestHandler<Modelo, ResponseModel>
        {

            private readonly ProyectoContext _context;
            private readonly IPasswordHasher<Clientes> _passwordHasher;
            private readonly ITokenCliente _tokenCliente;

           public Manejador(ProyectoContext context, IPasswordHasher<Clientes> passwordHasher, ITokenCliente tokenCliente)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _tokenCliente = tokenCliente;
            }

            public async Task<ResponseModel> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var cliente = await _context.clientes
                   .FirstOrDefaultAsync(c => c.correo == request.correo);

                if (cliente == null)
                {
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound,null, "Este correo no esta registrado");
                    
                }

               
                var resultado = _passwordHasher.VerifyHashedPassword(cliente, cliente.password, request.password);

                if (resultado == PasswordVerificationResult.Failed)
                {
                   return ResponseService.Respuesta(StatusCodes.Status401Unauthorized,null, "La contraseña es incorrecta");
                }


                var session = new ClienteData
                {
                    Id = cliente.id,
                    nombre = cliente.nombre,
                    apellido = cliente.apellido,
                    correo = cliente.correo,
                    Token = _tokenCliente.CrearToken(cliente)
                    
                };

                return ResponseService.Respuesta(StatusCodes.Status200OK,session);
            }
        }
        
    }
}
