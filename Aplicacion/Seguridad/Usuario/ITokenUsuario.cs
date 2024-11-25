using Dominio.Entidades;

namespace Aplicacion.Seguridad.Usuario
{
    public interface ITokenUsuario
    {
        string CrearToken(Usuarios usuario);
    }
}
