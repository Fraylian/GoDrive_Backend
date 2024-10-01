﻿using MediatR;
using FluentValidation;
using Dominio.Entidades;
using Microsoft.AspNetCore.Identity;
using Aplicacion.Seguridad;
using System.Net;

namespace Aplicacion.Metodos.Usuario
{
    public class Login
    {
        public class Modelo: IRequest<UsuarioData>
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

        public class Manejador : IRequestHandler<Modelo, UsuarioData>
        {
            private readonly UserManager<Usuarios> _userManager;
            private readonly SignInManager<Usuarios> _signInManager;

            public Manejador(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<UsuarioData> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.email);
                if (usuario == null)
                {
                    throw new Exception("No se encontro el usuario");
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.password, false);

                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        nombre = usuario.nombre,
                        apellido = usuario.apellido,
                        email = usuario.Email
                    };
                }

                throw new Exception("La contraseña es incorrecta");

            }
        }
    }
}