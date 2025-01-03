﻿using MediatR;
using FluentValidation;
using Persistencia.Context;
using Dominio.Entidades;
using Dominio.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Seguridad;
using Aplicacion.Seguridad.Cliente;

namespace Aplicacion.Cliente
{
    public class Registrar
    {
        public class Modelo: IRequest<ResponseModel>
        {
            public string nombre { get; set; }
            public string apellido { get; set; }
            public string correo { get; set; }
            public string password { get; set; }
            public string tipo_identificacion { get; set; }
            public string numero_identificacion { get; set; }
        }


        public class Validador: AbstractValidator<Modelo>
        {
            public Validador()
            {
                RuleFor(x => x.nombre).NotEmpty().WithMessage("El campo nombre no puede estar vacio")
                .MaximumLength(50).WithMessage("El maximo de caracteres es de 50");

                RuleFor(x => x.apellido).NotEmpty().WithMessage("El campo apellido no puede estar vacio")
                .MaximumLength(50).WithMessage("El maximo de caracteres es de 50");

                RuleFor(x => x.correo).NotEmpty().WithMessage("El campo correo no puede estar vacio")
                .EmailAddress().WithMessage("El correo ingresado no es valido")
                .MaximumLength(100).WithMessage("El maximo de caracteres es de 100");

                RuleFor(x => x.password).NotEmpty().WithMessage("El campo contraseña no puede estar vacio")
                .MinimumLength(8).WithMessage("La contraseña debe tener un minimo de 8 caracteres")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial");

                RuleFor(x => x.tipo_identificacion)
               .NotEmpty().WithMessage("Debe de elegir un tipo de identificación")
                .Must(value => Enum.TryParse<tipo_identificacion>(value,true, out _))
                .WithMessage("El tipo de identificación debe ser Cedula o Pasaporte");

                RuleFor(x => x.numero_identificacion).NotEmpty().WithMessage("El campo numero de documento no puede estar vacio")
                    .MaximumLength(20).WithMessage("El maximo de caracteres es 20");
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

                var cliente_existe = await _context.clientes.Where(x => x.correo == request.correo).AnyAsync();
                if (cliente_existe)
                {
                    return ResponseService.Respuesta(StatusCodes.Status409Conflict,null, "Este correo ya esta registrado");
                   
                    
                }

                var numero_identificacion_registrado = await _context.clientes.Where(x => x.numero_identificacion == request.numero_identificacion).AnyAsync();
                if (numero_identificacion_registrado)
                {
                    return ResponseService.Respuesta(StatusCodes.Status409Conflict, null, "Este numero de identificacion ya esta registrado");
                    
                }
                var cliente = new Clientes
                {
                    id = Guid.NewGuid(),
                    nombre = request.nombre,
                    apellido = request.apellido,
                    correo = request.correo,
                    fecha_creacion = DateTime.Now,
                    tipo_identificacion = request.tipo_identificacion,
                    numero_identificacion = request.numero_identificacion,

                };

                cliente.password = _passwordHasher.HashPassword(cliente,request.password);

                _context.clientes.Add(cliente);
                var resultado = await _context.SaveChangesAsync();
                if (resultado > 0)
                {
                    var data = new ClienteData{
                        nombre = cliente.nombre,
                        apellido = cliente.apellido,
                        correo = cliente.correo,
                        Token = _tokenCliente.CrearToken(cliente)
                        
                        
                    };

                    return ResponseService.Respuesta(StatusCodes.Status201Created,data);
                }
                return ResponseService.Respuesta(StatusCodes.Status500InternalServerError,null, "No se pudo registrar el cliente");
                
            }
        }
    }
}
