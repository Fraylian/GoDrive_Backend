using Microsoft.AspNetCore.Mvc;
using MediatR;
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
        public async Task<ActionResult<object>> Lista()
        {
            try
            {
                return await Mediator.Send(new Listado.listado_detalles_facturas());
            }
            catch (KeyNotFoundException ex)
            {

                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> ObtenerById(int id)
        {
            try
            {
                return await Mediator.Send(new Consulta.Parametro { Id = id });
            }
            catch (KeyNotFoundException ex)
            {

                return BadRequest(new {mensaje = ex.Message});
            }
        }
    }
}
