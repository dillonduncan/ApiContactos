using System;
using System.Collections.Generic;

namespace ApiContactos.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? CorreoUsuario { get; set; }

    public string? Clave { get; set; }

    public virtual ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();
}
