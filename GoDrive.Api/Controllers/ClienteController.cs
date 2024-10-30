using Microsoft.AspNetCore.Mvc;
using Aplicacion.Cliente;
using MediatR;
using Dominio.Entidades;
using Aplicacion.Seguridad;
using Aplicacion.Metodos.Cliente;

namespace GoDrive.Api.Controllers
{
    public class ClienteController : GeneralController
    {
        [HttpPost("registrar")]
        public async Task<ActionResult<Unit>> Registrar(Registrar.Modelo datos)
        {
            try
            {
                return await Mediator.Send(datos);
            }
            catch (KeyNotFoundException ex)
            {

                return BadRequest(new {mensaje = ex.Message});
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<List<Clientes>>> Lista()
        {
            return await Mediator.Send(new Listado.ListaClientes());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> ObetenerCliente(Guid id)
        {
            try
            {
                return await Mediator.Send(new Consulta.Modelo{ Id = id});
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new {mensaje = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ClienteData>> Login(Login.Modelo parametros)
        {
            return await Mediator.Send(parametros);
        }

    }
}
