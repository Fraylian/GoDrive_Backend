using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Usuario;
using Aplicacion.Seguridad;
using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;

namespace GoDrive.Api.Controllers
{
    public class UsuarioController : GeneralController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Modelo datos)
        {
            return await Mediator.Send(datos);
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Insertar.Modelo datos)
        {
            try
            {
                return await Mediator.Send(datos);
            }
            catch (KeyNotFoundException ex)
            {

                return BadRequest(new { mensaje = ex.Message });
            }

        }


        [HttpGet("listado")]
        public async Task<List<Usuarios>> Lista()
        {
            return await Mediator.Send(new Listado.ListaUsuarios());
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await Mediator.Send(new UsuarioActual.Modelo());
        }
    }
}
