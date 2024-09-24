using Microsoft.AspNetCore.Mvc;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Persistencia.Context;

namespace GoDrive.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public VehiculosController(ProyectoContext context)
        {
            _context = context;
        }

        // Método para listar todos los vehículos
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarVehiculos()
        {
            var vehiculos = await _context.vehiculos.ToListAsync(); 
            return Ok(vehiculos); 
        }

        // Método para obtener un vehículo por ID
        [HttpGet]
        [Route("obtener/{id}")]
        public async Task<IActionResult> ObtenerVehiculo(int id)
        {
            var vehiculo = await _context.vehiculos.FindAsync(id); 
            if (vehiculo == null)
            {
                return NotFound(new { message = "Vehículo no encontrado" });
            }

            return Ok(vehiculo); 
        }

        // Método para guardar un nuevo vehículo
        [HttpPost]
        [Route("guardar")]
        public async Task<IActionResult> GuardarVehiculo([FromBody] Vehiculos vehiculo)
        {
            if (ModelState.IsValid)
            {
                _context.vehiculos.Add(vehiculo); 
                await _context.SaveChangesAsync(); 
                return Ok(new { message = "Vehículo guardado correctamente" });
            }

            return BadRequest(ModelState); 
        }

        // Método para editar un vehículo existente
        [HttpPut]
        [Route("editar/{id}")]
        public async Task<IActionResult> EditarVehiculo(int id, [FromBody] Vehiculos vehiculoActualizado)
        {
            var vehiculo = await _context.vehiculos.FindAsync(id); 

            if (vehiculo == null)
            {
                return NotFound(new { message = "Vehículo no encontrado" });
            }

            if (ModelState.IsValid)
            {
                // Actualizar los datos del vehículo
                vehiculo.Matricula = vehiculoActualizado.Matricula;
                vehiculo.Marca = vehiculoActualizado.Marca;
                vehiculo.Modelo = vehiculoActualizado.Modelo;
                vehiculo.transmision = vehiculoActualizado.transmision;
                vehiculo.year = vehiculoActualizado.year;
                vehiculo.numero_Puertas = vehiculoActualizado.numero_Puertas;
                vehiculo.numero_asientos = vehiculoActualizado.numero_asientos;
                vehiculo.costo_por_dia = vehiculoActualizado.costo_por_dia;
                vehiculo.rentado = vehiculoActualizado.rentado;
                vehiculo.descripcion = vehiculoActualizado.descripcion;
                vehiculo.imagen = vehiculoActualizado.imagen;

                _context.vehiculos.Update(vehiculo); 
                await _context.SaveChangesAsync(); 

                return Ok(new { message = "Vehículo actualizado correctamente" });
            }

            return BadRequest(ModelState); 
        }

        // Método para borrar un vehículo existente
        [HttpDelete]
        [Route("borrar/{id}")]
        public async Task<IActionResult> BorrarVehiculo(int id)
        {
            var vehiculo = await _context.vehiculos.FindAsync(id); 

            if (vehiculo == null)
            {
                return NotFound(new { message = "Vehículo no encontrado" });
            }

            _context.vehiculos.Remove(vehiculo); 
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Vehículo con ID {id} borrado correctamente" });
        }
    }
}
