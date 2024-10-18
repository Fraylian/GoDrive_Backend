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
            return await Mediator.Send(datos);
        }

        [HttpGet]
        public async Task<ActionResult<List<Clientes>>> Lista()
        {
            return await Mediator.Send(new Listado.ListaClientes());
        }

        [HttpPost("login")]
        public async Task<ActionResult<ClienteData>> Login(Login.Modelo parametros)
        {
            return await Mediator.Send(parametros);
        }

    }
}
