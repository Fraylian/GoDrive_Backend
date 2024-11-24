using Microsoft.AspNetCore.Mvc;
using Aplicacion.Seguridad.Response;
using Microsoft.AspNetCore.Http;
using Aplicacion.Metodos.Factura_Detalle;



namespace GoDrive.Api.Controllers
{
    public class Factura_DetalleController : GeneralController
    {
       /* [HttpPost]
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

        [HttpGet]
        public async Task<ActionResult<ResponseModel>> Lista()
        {
            var response = await Mediator.Send(new Listado.listado_detalles_facturas());
            if (response.Data == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseService.Respuesta(StatusCodes.Status404NotFound, response.Data, response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode,response.Data,response.Mensaje));
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel>> ObtenerById(int id)
        {
            if(id <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,ResponseService.Respuesta(StatusCodes.Status400BadRequest,null,"El id debe ser mayor a 0"));
            }

            var response = await Mediator.Send(new Consulta.Parametro { Id = id });
            if (response.Data == null)
            {
                return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
        }
    }
}
