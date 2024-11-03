using Dominio.Entidades;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Aplicacion.Seguridad.Cliente
{
    public class TokenCliente : ITokenCliente
    {
        public string CrearToken(Clientes cliente)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, cliente.correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Proyecto Final TDS-2024 | TDS-601"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(150),
                SigningCredentials = credenciales,
            };

            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);
            return tokenManejador.WriteToken(token);
        }
    }
}
