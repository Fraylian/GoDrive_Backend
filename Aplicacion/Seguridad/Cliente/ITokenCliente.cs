using Dominio.Entidades;

namespace Aplicacion.Seguridad.Cliente
{
    public interface ITokenCliente
    {
        string CrearToken(Clientes cliente);
    }
}
