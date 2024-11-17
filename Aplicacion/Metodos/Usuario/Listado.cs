﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Persistencia.Context;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Metodos.Usuario
{
    public class Listado
    {
        public class ListaUsuarios: IRequest<List<Usuarios>> { }

        public class Manejador : IRequestHandler<ListaUsuarios, List<Usuarios>>
        {
            private readonly ProyectoContext _context;
            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<List<Usuarios>> Handle(ListaUsuarios request, CancellationToken cancellationToken)
            {
                var usuarios = await _context.Users.ToListAsync();

                return usuarios;

            }
        }
    }
}