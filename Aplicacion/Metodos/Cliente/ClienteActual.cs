using Aplicacion.Seguridad;
using Aplicacion.Seguridad.Cliente;
using Persistencia.Context;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Aplicacion.Metodos.Cliente
{
    public class ClienteActual
    {
        public class Modelo: IRequest<ClienteData> { }

        public class Manejador : IRequestHandler<Modelo, ClienteData>
        {
            private readonly ITokenCliente _tokenCliente;
            private readonly IClienteSesion _clienteSesion;
            private readonly ProyectoContext _context;


            public async Task<ClienteData> Handle(Modelo request, CancellationToken cancellationToken)
            {

                var clienteId = _clienteSesion.ObtenerClienteSesion();

                var cliente = await _context.clientes.FirstOrDefaultAsync(x => x.correo == clienteId);
                return new ClienteData
                {
                    nombre = cliente.nombre,
                    apellido = cliente.apellido,
                    correo = cliente.correo,
                    Token = _tokenCliente.CrearToken(cliente)
                };
                throw new NotImplementedException();
            }
        }
    }
}
