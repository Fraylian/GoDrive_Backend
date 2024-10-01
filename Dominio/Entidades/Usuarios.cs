using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Dominio.Entidades
{
    public class Usuarios: IdentityUser
    {
        [StringLength(50,ErrorMessage = "El maximo de caracteres es 50")]
        public string nombre { get; set; }

        [StringLength(50, ErrorMessage = "El maximo de caracteres es 50")]
        public string apellido { get; set; }
        public DateTime fecha_registro { get; set; }
    }
}
