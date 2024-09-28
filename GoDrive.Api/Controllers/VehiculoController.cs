using Microsoft.AspNetCore.Mvc;
using Aplicacion.Vehiculo;
using MediatR;
using Dominio.Entidades;
using Persistencia.Context;
using System.Threading.Tasks;

namespace GoDrive.Api.Controllers
{
    public class VehiculoController : GeneralController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Insertar(Insertar.modeloVehiculos datos)
        {
            return await Mediator.Send(datos);
        }

        [HttpGet]
        public async Task<ActionResult<List<Vehiculos>>> Lista()
        {
            return await Mediator.Send(new listado.ListaVehiculos());
        }
    }
}
