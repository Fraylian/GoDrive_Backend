using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class factura_detalle
    {
        public int id { get; set; }
        [Required]
        public int factura_id { get; set; }

        [ForeignKey("factura_id")]
        public factura factura { get; set; }

        [Required]
        public int vehiculo_id { get; set; }

        [ForeignKey("vehiculo_id")]
        public Vehiculos vehiculo { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal costo_por_dia { get; set; }

        [Required]
        public int dias_rentados { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal costo_total_vehiculo { get; set; }
    }
}
