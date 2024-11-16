using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Vehiculo;
using MediatR;
using Dominio.Entidades;


namespace GoDrive.Api.Controllers
{
    public class VehiculoController : GeneralController
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Unit>> Insertar([FromForm] Insertar.modeloVehiculos datos)
        {
            return await Mediator.Send(datos);
        }

        [HttpGet]
        public async Task<ActionResult<List<Vehiculos>>> Lista()
        {
            return await Mediator.Send(new listado.ListaVehiculos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> ObtenerPorId(int id)
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

        [HttpPut("editar/{id}")]
        public async Task<ActionResult<Unit>> Editar(int id, [FromForm] Actualizar.modelo modelo)
        {
            modelo.id = id;
            return await Mediator.Send(modelo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(int id)
        {
            try
            {
                return await Mediator.Send(new Eliminar.Modelo { Id = id });
                return Ok();
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
