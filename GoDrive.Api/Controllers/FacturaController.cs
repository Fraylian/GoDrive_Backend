using Microsoft.AspNetCore.Mvc;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Persistencia.Context;

namespace GoDrive.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public FacturaController(ProyectoContext context)
        {
            _context = context;
        }

        // Método para listar todas las facturas
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarFacturas()
        {
            var facturas = await _context.factura
                .Include(f => f.cliente)
                .Include(f => f.usuario)
                .Include(f => f.detalles_factura)
                .ToListAsync();
            return Ok(facturas);
        }

        // Método para obtener una factura por ID
        [HttpGet]
        [Route("obtener/{id}")]
        public async Task<IActionResult> ObtenerFactura(int id)
        {
            var factura = await _context.factura
                .Include(f => f.cliente)
                .Include(f => f.usuario)
                .Include(f => f.detalles_factura)
                .FirstOrDefaultAsync(f => f.id == id);

            if (factura == null)
            {
                return NotFound(new { message = "Factura no encontrada" });
            }

            return Ok(factura);
        }

        // Método para crear una nueva factura
        [HttpPost]
        [Route("crear")]
        public async Task<IActionResult> CrearFactura([FromBody] factura nuevaFactura)
        {
            if (ModelState.IsValid)
            {
                // Verificar que el cliente exista
                var clienteExistente = await _context.clientes.FindAsync(nuevaFactura.id_cliente);
                if (clienteExistente == null)
                {
                    return BadRequest(new { message = "Cliente no encontrado" });
                }

                // Verificar que el usuario exista
                var usuarioExistente = await _context.usuarios.FindAsync(nuevaFactura.id_usuario);
                if (usuarioExistente == null)
                {
                    return BadRequest(new { message = "Usuario no encontrado" });
                }

                nuevaFactura.fecha_creacion = DateTime.Now;

                _context.factura.Add(nuevaFactura);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Factura creada correctamente", facturaId = nuevaFactura.id });
            }

            return BadRequest(ModelState);
        }

        // Método para actualizar una factura existente
        [HttpPut]
        [Route("actualizar/{id}")]
        public async Task<IActionResult> ActualizarFactura(int id, [FromBody] factura facturaActualizada)
        {
            if (id != facturaActualizada.id)
            {
                return BadRequest(new { message = "El ID de la factura no coincide" });
            }

            if (ModelState.IsValid)
            {
                // Verificar que la factura exista
                var facturaExistente = await _context.factura.FindAsync(id);
                if (facturaExistente == null)
                {
                    return NotFound(new { message = "Factura no encontrada" });
                }

                // Verificar que el cliente exista
                var clienteExistente = await _context.clientes.FindAsync(facturaActualizada.id_cliente);
                if (clienteExistente == null)
                {
                    return BadRequest(new { message = "Cliente no encontrado" });
                }

                // Verificar que el usuario exista
                var usuarioExistente = await _context.usuarios.FindAsync(facturaActualizada.id_usuario);
                if (usuarioExistente == null)
                {
                    return BadRequest(new { message = "Usuario no encontrado" });
                }

                // Actualizar los campos de la factura
                facturaExistente.id_cliente = facturaActualizada.id_cliente;
                facturaExistente.id_usuario = facturaActualizada.id_usuario;
                facturaExistente.fecha_renta_inicio = facturaActualizada.fecha_renta_inicio;
                facturaExistente.fecha_renta_final = facturaActualizada.fecha_renta_final;
                facturaExistente.monto_total = facturaActualizada.monto_total;
                facturaExistente.subtotal = facturaActualizada.subtotal;
                facturaExistente.monto_itbis = facturaActualizada.monto_itbis;
                facturaExistente.detalles_factura = facturaActualizada.detalles_factura;

                _context.Entry(facturaExistente).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest(new { message = "Error al actualizar la factura" });
                }

                return Ok(new { message = "Factura actualizada correctamente" });
            }

            return BadRequest(ModelState);
        }

        // Método para eliminar una factura
        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<IActionResult> EliminarFactura(int id)
        {
            var factura = await _context.factura
                .Include(f => f.detalles_factura) // Incluimos los detalles para eliminar cascada si es necesario
                .FirstOrDefaultAsync(f => f.id == id);

            if (factura == null)
            {
                return NotFound(new { message = "Factura no encontrada" });
            }

            _context.factura.Remove(factura);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Factura eliminada correctamente" });
        }
    }
}
