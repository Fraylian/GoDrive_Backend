using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Aplicacion.Seguridad.Response;

namespace Aplicacion.Metodos.Vehiculo
{
    public class Consulta
    {
        public class Modelo
        {
            public string Marca { get; set; }
            public string modelo { get; set; }
            public string transmision { get; set; }
            public int year { get; set; }
            public int numero_Puertas { get; set; }
            public int numero_asientos { get; set; }
            public decimal costo_por_dia { get; set; }
            public bool rentado { get; set; }
            public string descripcion { get; set; }
            public List<string> Imagenes { get; set; }
        }
        public class VehiculoId: IRequest<ResponseModel>
        {
            public int  Id { get; set; }
        }


        public class Manejador : IRequestHandler<VehiculoId, ResponseModel>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }


            public async Task<ResponseModel> Handle(VehiculoId request, CancellationToken cancellationToken)
            {
                var vehiculo = await _context.vehiculos.Where(v => v.id == request.Id)
                .Include(v => v.imagenes)
                .Select(v => new Modelo
                {
                    
                   Marca = v.Marca,
                   modelo = v.Modelo,
                   year = v.year,
                   transmision = v.transmision,
                   numero_Puertas = v.numero_Puertas,
                   numero_asientos = v.numero_asientos,
                   costo_por_dia = v.costo_por_dia,
                   rentado = v.rentado,
                   descripcion = v.descripcion,
                    Imagenes = v.imagenes != null && v.imagenes.Any()
                    ? v.imagenes.Select(i => Convert.ToBase64String(i.Data)).ToList()
                    : new List<string>()

                }).FirstOrDefaultAsync();

                if(vehiculo == null)
                {
                    return ResponseService.Respuesta(StatusCodes.Status400BadRequest,null, "No se encontro el vehiculo"); 
                }

                return ResponseService.Respuesta(StatusCodes.Status200OK,vehiculo, "");
                
            }
        }
    }
}
