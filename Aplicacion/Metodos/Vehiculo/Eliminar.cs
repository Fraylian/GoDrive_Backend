using Persistencia.Context;
using MediatR;
using Aplicacion.Seguridad.Response;
using Microsoft.AspNetCore.Http;


namespace Aplicacion.Metodos.Vehiculo
{
    public class Eliminar
    {
        public class Modelo: IRequest<ResponseModel>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<Modelo, ResponseModel>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }

            public async Task<ResponseModel> Handle(Modelo request, CancellationToken cancellationToken)
            {
                var vehiculo = await _context.vehiculos.FindAsync(request.Id);
                if(vehiculo == null)
                {
                    return ResponseService.Respuesta(StatusCodes.Status404NotFound,null, "No se encotro el vehiculo");
                }

                _context.vehiculos.Remove(vehiculo);
                var resultado = await _context.SaveChangesAsync();
                if(resultado > 0)
                {
                    return ResponseService.Respuesta(StatusCodes.Status200OK,null, "Se ha eliminado el vehiculo");
                }
                return ResponseService.Respuesta(StatusCodes.Status500InternalServerError,null, "No se pudo eliminar el vehículo.");
                


            }
        }
    }
}
