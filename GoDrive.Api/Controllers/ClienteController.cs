using Microsoft.AspNetCore.Mvc;
using Aplicacion.Cliente;
using MediatR;
using Dominio.Entidades;

namespace GoDrive.Api.Controllers
{
    public class ClienteController : GeneralController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Registrar(Registrar.Modelo datos)
        {
            return await Mediator.Send(datos);
        }

        [HttpGet]
        public async Task<ActionResult<List<Clientes>>> Lista()
        {
            return await Mediator.Send(new Listado.ListaClientes());
        }

    }
}
