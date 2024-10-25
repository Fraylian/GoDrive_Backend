using Microsoft.AspNetCore.Mvc;
using MediatR;
using Aplicacion.Metodos.Factura;

namespace GoDrive.Api.Controllers
{
    public class FacturaController : GeneralController
    {
        [HttpPost]
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

        }

        [HttpGet]
        public async Task<ActionResult<object>> Listado()
        {
            try
            {
                return await Mediator.Send(new Listado.Listado_Facturas());
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new {mensaje = ex.Message});
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> ObtenerPorId(int id)
        {
            try
            {
                return await Mediator.Send(new Consultar.parametro { id = id });
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new {mensaje = ex.Message});
            }
        }
    }
}
