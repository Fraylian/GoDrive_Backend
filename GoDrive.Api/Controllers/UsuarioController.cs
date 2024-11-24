using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Usuario;
using Aplicacion.Seguridad;
using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Aplicacion.Seguridad.Response;

namespace GoDrive.Api.Controllers
{
    public class UsuarioController : GeneralController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Modelo datos)
        {
            var response = await Mediator.Send(datos);
            if (response.Success == true)
            {
                return StatusCode(StatusCodes.Status200OK,ResponseService.Respuesta(response.StatusCode,response.Data, response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));

            
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Insertar.Modelo datos)
        {
            var response = await Mediator.Send(datos);
            if(response.Success == true)
            {
                return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(response.StatusCode,response.Data,response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));

        }


        [HttpGet("listado")]
        public async Task<ActionResult<List<Usuarios>>> Lista()
        {
            var response = await Mediator.Send(new Listado.ListaUsuarios());
            if (response.Data == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseService.Respuesta(StatusCodes.Status404NotFound, response.Data, response.Mensaje));
            }
            return response.Data;
            
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        { 
            var response = await Mediator.Send(new UsuarioActual.Modelo());
            if(response.Success == true)
            {
                return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(response.StatusCode,response.Data));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            
            
        }
    }
}
