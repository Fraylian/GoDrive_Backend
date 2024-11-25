using System.Text;
using Dominio.Entidades;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Aplicacion.Metodos.Email
{
    public class EmailSender: IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Execute(Emails modelo)
        {
            string apiUrl = "https://api.sendgrid.com/v3/mail/send";
            string apiKey = _configuration["NewApiKey"];

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            client.DefaultRequestHeaders.Add("Accept", $"application/json");

            string emailContent = JsonConvert.SerializeObject(modelo);
            var response = await client.PostAsync(apiUrl, new StringContent(emailContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
