using Microsoft.AspNetCore.Mvc;
using MediatR;
using Aplicacion.Metodos.Factura;
using Aplicacion.Seguridad.Response;

namespace GoDrive.Api.Controllers
{
    public class FacturaController : GeneralController
    {
        /*[HttpPost]
        public async Task<ActionResult<Unit>> Insertar(Insertar.Modelo modelo)
        {
            try
            {
                return await Mediator.Send(modelo);
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(new { mensaje = ex.Message });
            }

        }*/

        [HttpPost("facturar")]
        public async Task<ActionResult<ResponseModel>> CrearFactura(CrearFactura.Modelo modelo)
        {
            var response = await Mediator.Send(modelo);
            if (response.Success == true)
            {
                return StatusCode(StatusCodes.Status201Created, ResponseService.Respuesta(StatusCodes.Status201Created, response, response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));

        }

        [HttpGet]
        public async Task<ActionResult<ResponseModel>> Listado()
        {
                var response = await Mediator.Send(new Listado.Listado_Facturas());
                if(response.Data == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseService.Respuesta(StatusCodes.Status404NotFound, response.Data, response.Mensaje));
                }
                return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(StatusCodes.Status200OK, response.Data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel>> ObtenerPorId(int id)
        {
            if(id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ResponseService.Respuesta(StatusCodes.Status400BadRequest));
            }
            var response = await Mediator.Send(new Consultar.parametro { id = id });
            if(response.Data == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseService.Respuesta(StatusCodes.Status404NotFound, response.Data, response.Mensaje));
            }
            return response.Data;
           
        }
    }
}
