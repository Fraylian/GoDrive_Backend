using Dominio.Entidades;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Aplicacion.Seguridad.Usuario
{
    public class TokenUsuario : ITokenUsuario
    {
        public string CrearToken(Usuarios usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.Email),
                new Claim(ClaimTypes.Role, "Usuario")
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
