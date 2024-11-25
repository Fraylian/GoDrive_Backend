using Microsoft.AspNetCore.Mvc;
using Aplicacion.Metodos.Email;
using Dominio.Entidades;


namespace GoDrive.Api.Controllers
{
    public class EmailController : GeneralController
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarCorreo([FromBody] Emails email)
        {
            var resultado = await _emailSender.Execute(email);

            if (resultado)
            {
                return Ok(new { mensaje = "Correo enviado exitosamente" });
            }

            return StatusCode(500, new { mensaje = "Error al enviar el correo" });
        }
    }
}
