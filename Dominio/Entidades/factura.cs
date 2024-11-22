using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dominio.Entidades
{
    public class factura
    {
        public int id { get; set; }

        [StringLength(20)]
        public string numero_factura { get; set; }

        [Required]
        public Guid id_cliente { get; set; }

        [ForeignKey("id_cliente")]
        public Clientes cliente { get; set; }


        [Required]
        public DateTime fecha_creacion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal  monto_total { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal subtotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal monto_itbis { get; set; }

        [Required]
        public DateTime fecha_renta_inicio { get; set; }
        [Required]
        public DateTime fecha_renta_final { get; set; }
        public ICollection<factura_detalle> detalles_factura { get; set; }

    }
}
