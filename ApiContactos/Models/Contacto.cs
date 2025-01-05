using System;
using System.Collections.Generic;

namespace ApiContactos.Models;

public partial class Contacto
{
    public int IdContacto { get; set; }

    public string? NombreContacto { get; set; }

    public string? CorreoContacto { get; set; }

    public string? CelularContacto { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
