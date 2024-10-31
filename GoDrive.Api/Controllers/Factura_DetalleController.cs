using Microsoft.AspNetCore.Mvc;
using MediatR;
using Aplicacion.Metodos.Factura_Detalle;


namespace GoDrive.Api.Controllers
{
    public class Factura_DetalleController : GeneralController
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
    }
}
