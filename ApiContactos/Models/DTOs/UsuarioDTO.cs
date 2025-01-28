using System.ComponentModel.DataAnnotations;

namespace ApiContactos.Models.DTOs
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage ="Este campo es requerido")]
        public string NombreUsuario { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string CorreoUsuario { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Clave { get; set; }
    }
}
