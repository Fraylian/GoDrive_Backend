using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Usuario;
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
            try
            {
                return await Mediator.Send(datos);
            }
            catch (KeyNotFoundException ex)
            {

                return BadRequest(new {mensaje = ex.Message});
            }
            
        }
    }
}
