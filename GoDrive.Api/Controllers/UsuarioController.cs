using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Usuario;
using MediatR;
using Dominio.Entidades;
using Aplicacion.Seguridad;

namespace GoDrive.Api.Controllers
{
    public class UsuarioController : GeneralController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Modelo datos)
        {
            return await Mediator.Send(datos);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Insertar.Modelo datos)
        {
            return await Mediator.Send(datos);
        }
    }
}
