using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dominio.Entidades
{
    public class Vehiculos
    {
        
        public int id { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "el maximo de caracteres ed de 10")]
        public string Matricula { get; set; }

        [Required]
        [StringLength(50)]
        public string Marca { get; set; }

        
        [Required]
        [StringLength(50)]
        public string Modelo { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "El maximo de caracteres es de 15 ")]
        public string transmision { get; set; }

        public int year { get; set; }

        [Required]
        [Range(1,10, ErrorMessage = "El maximo de puertas es de 10")]
        public int numero_Puertas { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "El maximo de asientos es de 10")]
        public int numero_asientos { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal costo_por_dia { get; set; }
        [Required]
        public bool rentado { get; set; }
        public string descripcion { get; set; }
        public ICollection<Imagen> imagenes { get; set; }


    }
}
