using Microsoft.AspNetCore.Mvc;
using Aplicacion.Cliente;
using Aplicacion.Metodos.Cliente;
using Aplicacion.Seguridad.Response;
using Microsoft.AspNetCore.Authorization;

namespace GoDrive.Api.Controllers
{
    public class ClienteController : GeneralController
    {
        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<ActionResult<ResponseModel>> Registrar(Registrar.Modelo datos)
        {
            var response = await Mediator.Send(datos);
            if (response.Success == true)
            {
                return StatusCode(StatusCodes.Status201Created, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            
        }

        [Authorize(Policy = "User")]
        [HttpGet("listado")]
        public async Task<ActionResult<object>> Lista()
        {
            var response = await Mediator.Send(new Listado.ListaClientes());
            if(response.Data == null)
            {
                return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(response.StatusCode,response.Data));
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel>> ObetenerCliente(Guid id)
        {
            var response = await Mediator.Send(new Consulta.Modelo { Id = id });
            if(response.Data == null)
            {
                return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode,response.Data,response.Mensaje));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));

        }

       /* [HttpGet]
        public async Task<ActionResult<ClienteData>> DevolverCliente()
        {
            return await Mediator.Send(new ClienteActual.Modelo());
        }*/


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel>> Login(Login.Modelo parametros)
        {
            var response = await Mediator.Send(parametros);
            if (response.Success == true)
            {
                return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            
            
        }

    }
}
