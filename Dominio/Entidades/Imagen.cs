using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class Imagen
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int vehiculo_id { get; set; }
        public byte[] Data { get; set; }

        [ForeignKey("vehiculo_id")]
        public Vehiculos Vehiculo { get; set; }



    }
}
