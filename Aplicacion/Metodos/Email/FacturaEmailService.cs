using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Persistencia.Context;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;

namespace Aplicacion.Metodos.Email
{
    public class FacturaEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ProyectoContext _context;

        public FacturaEmailService(IEmailSender emailSender, ProyectoContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<ResponseModel> EnviarCorreoFactura(factura factura, factura_detalle detalle, Vehiculos vehiculo)
        {
            var cliente = await _context.clientes
            .Where(c => c.id == factura.id_cliente)
            .Select(c => new { c.nombre, c.correo })
            .FirstOrDefaultAsync();

            if (cliente == null || string.IsNullOrEmpty(cliente.correo))
            {
                return ResponseService.Respuesta(StatusCodes.Status404NotFound,null, "El cliente no tiene un correo electrónico registrado.");

            }

            string asunto = "Factura de Renta de Vehículo";
            string cuerpo = $@"
            <h1>Detalles de la Factura</h1>
            <p><strong>Numero de Factura:</strong> {factura.numero_factura}</p>
            <p><strong>Fecha de Creación:</strong> {factura.fecha_creacion}</p>
            <p><strong>Fechas de Renta:</strong> {factura.fecha_renta_inicio:dd/MM/yyyy} - {factura.fecha_renta_final:dd/MM/yyyy}</p>
            <p><strong>Vehículo:</strong> {vehiculo.Modelo}</p>
            <p><strong>Costo por Día:</strong> {vehiculo.costo_por_dia:C}</p>
            <p><strong>Días Rentados:</strong> {detalle.dias_rentados}</p>
            <p><strong>Subtotal:</strong> {factura.subtotal:C}</p>
            <p><strong>ITBIS (18%):</strong> {factura.monto_itbis:C}</p>
            <p><strong>Total:</strong> {factura.monto_total:C}</p>
        ";

          

            var email = new Emails
            {
                From = new ContentEmail
                {
                    Email = "luislachapelcentropeguero@gmail.com",
                    Name = "Go Drive"
                },
                Personalizations = new List<Personalization>
            {
                new Personalization
                {
                    subject = "Factura de Renta de Vehículo",
                    To = new List<ContentEmail>
                    {
                        new ContentEmail
                        {
                            Email = cliente.correo,
                            Name = cliente.nombre
                        }
                    }
                }
            },
                Content = new List<ContentBody>
            {
                new ContentBody
                {
                    Type = "text/html",
                    Value = cuerpo
                }
            }
            };

            bool correoEnviado = await _emailSender.Execute(email);

            if (!correoEnviado)
            {
                return ResponseService.Respuesta(StatusCodes.Status500InternalServerError, null, "No se pudo enviar el correo de la factura."); 
                
            }
            return ResponseService.Respuesta(StatusCodes.Status200OK, null, "El correo de la factura fue enviado exitosamente.");


        }
    }
}
