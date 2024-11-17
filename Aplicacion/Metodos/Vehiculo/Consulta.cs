﻿using Persistencia.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            public string Imagen { get; set; }
        }
        public class VehiculoId: IRequest<Modelo>
        {
            public int  Id { get; set; }
        }


        public class Manejador : IRequestHandler<VehiculoId, Modelo>
        {
            private readonly ProyectoContext _context;

            public Manejador(ProyectoContext context)
            {
                _context = context;
            }


            public async Task<Modelo> Handle(VehiculoId request, CancellationToken cancellationToken)
            {
                var vehiculo = await _context.vehiculos.Where(v => v.id == request.Id).Select(v => new Modelo
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
                   Imagen = v.imagen != null ? Convert.ToBase64String(v.imagen) : null
                }).FirstOrDefaultAsync();

                if(vehiculo == null)
                {
                    throw new KeyNotFoundException("No se encontro el vehiculo");
                }

                return vehiculo;
                
            }
        }
    }
}