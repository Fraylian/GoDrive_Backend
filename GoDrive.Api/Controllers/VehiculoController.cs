using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Vehiculo;
using MediatR;
using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;

namespace GoDrive.Api.Controllers
{
    [AllowAnonymous]
    public class VehiculoController : GeneralController
    {
        [HttpPost]

        public async Task<ActionResult<Unit>> Insertar([FromBody] Insertar.modeloVehiculos datos)
        {
            try
            {
                 await Mediator.Send(datos);
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "El vehículo fue insertado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, (new { mensaje = ex.Message }));

            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });

            }

        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<listado.Modelo>>> Lista()
        {
            try
            {
                return await Mediator.Send(new listado.ListaVehiculos());
            }
            catch (KeyNotFoundException ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, (new { mensaje = ex.Message }));
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta.Modelo>> ObtenerPorId(int id)
        {
            try
            {
                return await Mediator.Send(new Consulta.VehiculoId { Id = id });
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new { mensaje = ex.Message });
            }
            
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<Filtros.Modelo>>> Filtro([FromQuery] Filtros.Parametros modelo)
        {
            try
            {
                return await Mediator.Send(modelo);
            }
            catch (KeyNotFoundException ex)
            {

                return StatusCode(StatusCodes.Status404NotFound, (new { mensaje = ex.Message }));
            }
            
        }

        [HttpPut("editar/{id}")]

        public async Task<ActionResult<Unit>> Editar(int id, [FromBody] Actualizar.modelo modelo)
        {
            try
            {
                modelo.id = id;
                return await Mediator.Send(modelo);
            }
            catch (KeyNotFoundException ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, new {mensaje = ex.Message});
            }
            catch (InvalidOperationException ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(int id)
        {
            try
            {
                return await Mediator.Send(new Eliminar.Modelo { Id = id });
                //return Ok();
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado.", detalles = ex.Message });
            }
        }
    }
}
