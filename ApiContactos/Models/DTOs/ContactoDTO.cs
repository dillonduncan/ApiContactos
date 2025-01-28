using System.ComponentModel.DataAnnotations;

namespace ApiContactos.Models.DTOs
{
    public class ContactoDTO
    {
        [Required(ErrorMessage = "Este campo es requerido")]
        public string NombreContacto { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [EmailAddress]
        public string CorreoContacto { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Phone]
        public string CelularContacto { get; set; }

    }
}
