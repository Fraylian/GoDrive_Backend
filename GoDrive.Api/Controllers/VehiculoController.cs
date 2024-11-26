using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Vehiculo;
using MediatR;
using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Aplicacion.Seguridad.Response;

namespace GoDrive.Api.Controllers
{
    
    public class VehiculoController : GeneralController
    {
        
        [HttpPost]

        public async Task<ActionResult<ResponseModel>> Insertar([FromBody] Insertar.modeloVehiculos datos)
        {

            var response = await Mediator.Send(datos);
            if (response.Success == true)
            {
                return StatusCode(StatusCodes.Status201Created, ResponseService.Respuesta(StatusCodes.Status201Created, response, "El vehículo fue insertado correctamente."));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));

        }

        [AllowAnonymous]
        [HttpGet("lista")]
        public async Task<ActionResult<List<listado.Modelo>>> Lista()
        {
           
               var response = await Mediator.Send(new listado.ListaVehiculos());
                if (response.Data == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseService.Respuesta(StatusCodes.Status404NotFound, response.Data, response.Mensaje));
                }
                return response.Data;
           
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel>> ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ResponseService.Respuesta(StatusCodes.Status400BadRequest,null, "El id debe ser mayor a 0"));
            }
            var response = await Mediator.Send(new Consulta.VehiculoId { Id = id });
            if(response.Data == null)
            {
                return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));


        }

        [AllowAnonymous]
        [HttpGet("filtrar")]
        public async Task<ActionResult<ResponseModel>> Filtro([FromQuery] Filtros.Parametros modelo)
        {

            var response = await Mediator.Send(modelo);
            if(response.Data == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, ResponseService.Respuesta(StatusCodes.Status404NotFound, response.Data, response.Mensaje));
            }
            return response;
            
            
        }

        [Authorize(Policy = "User")]
        [HttpPut("editar/{id}")]

        public async Task<ActionResult<ResponseModel>> Editar(int id, [FromBody] Actualizar.modelo modelo)
        {
            if (id <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ResponseService.Respuesta(StatusCodes.Status400BadRequest, null, "El id debe ser mayor a 0"));
            }
            modelo.id = id;
            var response = await Mediator.Send(modelo);
            if (response.Success == true)
            {
                return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(StatusCodes.Status200OK,response.Data, response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data,response.Mensaje));
            


        }

        [Authorize(Policy = "User")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel>> Eliminar(int id)
        {
            if(id <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ResponseService.Respuesta(StatusCodes.Status400BadRequest, null, "El id debe ser mayor a 0"));
            }
            var response = await Mediator.Send(new Eliminar.Modelo { Id = id });
            if(response.Success == true)
            {
                return StatusCode(StatusCodes.Status200OK, ResponseService.Respuesta(StatusCodes.Status200OK, response.Data, response.Mensaje));
            }
            return StatusCode(response.StatusCode, ResponseService.Respuesta(response.StatusCode, response.Data, response.Mensaje));
        }
    }
}
