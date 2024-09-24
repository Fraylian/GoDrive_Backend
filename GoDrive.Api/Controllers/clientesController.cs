using Microsoft.AspNetCore.Mvc;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Persistencia.Context;

namespace GoDrive.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public ClientesController(ProyectoContext context)
        {
            _context = context;
        }

        // Método para listar todos los clientes
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarCliente()
        {
            var clientes = await _context.clientes.ToListAsync(); // Obtiene todos los clientes
            return Ok(clientes); // Devuelve la lista de clientes
        }

        // Método para guardar un nuevo cliente
        [HttpPost]
        [Route("guardar")]
        public async Task<IActionResult> GuardarCliente([FromBody] Clientes cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.id = Guid.NewGuid(); 
                cliente.fecha_creacion = DateTime.Now; 
                _context.clientes.Add(cliente); 
                await _context.SaveChangesAsync(); 
                return Ok(new { message = "Cliente guardado correctamente" });
            }

            return BadRequest(ModelState); 
        }

        // Método para editar un cliente existente
        [HttpPut]
        [Route("editar/{id}")]
        public async Task<IActionResult> EditarCliente(Guid id, [FromBody] Clientes clienteActualizado)
        {
            var cliente = await _context.clientes.FindAsync(id); 

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }

            if (ModelState.IsValid)
            {
                // Actualizar los datos del cliente
                cliente.nombre = clienteActualizado.nombre;
                cliente.apellido = clienteActualizado.apellido;
                cliente.correo = clienteActualizado.correo;
                cliente.password = clienteActualizado.password; 
                cliente.numero_identificacion = clienteActualizado.numero_identificacion;
                cliente.tipo_identificacion = clienteActualizado.tipo_identificacion;

                _context.clientes.Update(cliente); 
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cliente actualizado correctamente" });
            }

            return BadRequest(ModelState); 
        }

        // Método para borrar un cliente existente
        [HttpDelete]
        [Route("borrar/{id}")]
        public async Task<IActionResult> BorrarCliente(Guid id)
        {
            var cliente = await _context.clientes.FindAsync(id); // Buscar el cliente por ID

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }

            _context.clientes.Remove(cliente); 
            await _context.SaveChangesAsync(); 

            return Ok(new { message = $"Cliente con ID {id} borrado correctamente" });
        }
    }
}
