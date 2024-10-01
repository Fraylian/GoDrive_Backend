using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Persistencia.Context;
using Dominio.Entidades;

namespace Aplicacion.Seguridad
{
    public class DataPrueba
    {
        public async Task InsertarUsuario(ProyectoContext context, UserManager<Usuarios> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuarios
                {
                    nombre = "Jane",
                    apellido = "Doe",
                    UserName = "Jane@gmail.com",
                    fecha_registro = DateTime.Now,
                    Email = "Jane@gmail.com"


                };

                await userManager.CreateAsync(usuario, "Contraseña123$");
            }
        }
    }
}
