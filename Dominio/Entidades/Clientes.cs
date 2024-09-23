using System.ComponentModel.DataAnnotations;


namespace Dominio.Entidades
{
    public class Clientes
    {
        public Guid id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El maximo de caracteres es 50")]
        public string nombre { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El maximo de caracteres es 50")]
        public string apellido { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "El maximo de caracteres es 100")]
        public string correo { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public DateTime fecha_creacion { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "El maximo de caracteres es 20")]
        public string numero_identificacion { get; set; }

        [StringLength(10, ErrorMessage = "El maximo de caracteres es 10")]
        public string tipo_identificacion { get; set; }


    }
}
