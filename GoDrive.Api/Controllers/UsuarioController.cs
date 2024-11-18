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
            try
            {
                return await Mediator.Send(datos);
            }
            catch (KeyNotFoundException ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest,new {mensaje = ex.Message});
            }
            
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
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }


        [HttpGet("listado")]
        public async Task<ActionResult<List<Usuarios>>> Lista()
        {
            try
            {
                return await Mediator.Send(new Listado.ListaUsuarios());
            }
            catch (KeyNotFoundException ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, new {mensaje = ex.Message});
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            try
            {
                return await Mediator.Send(new UsuarioActual.Modelo());
            }
            catch (KeyNotFoundException ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, new {mensaje = ex.Message});
            }
            
        }
    }
}
