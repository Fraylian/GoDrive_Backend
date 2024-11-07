using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Aplicacion.Seguridad.Cliente
{
    public class ClienteSesion : IClienteSesion
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ClienteSesion(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string ObtenerClienteSesion()
        {
            var cliente = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return cliente;
        }
    }
}
