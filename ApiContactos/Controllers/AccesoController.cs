using ApiContactos.Custom;
using ApiContactos.Models;
using ApiContactos.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiContactos.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly ContactosContext _contactosContext;
        private readonly Utilidades _utilidades;
        public AccesoController(ContactosContext contactosContext, Utilidades utilidades)
        {
            _contactosContext = contactosContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro(UsuarioDTO user)
        {
            var userModel = new Usuario
            {
                NombreUsuario = user.NombreUsuario,
                CorreoUsuario = user.CorreoUsuario,
                Clave = _utilidades.encriptarSHA256(user.Clave)
            };
            await _contactosContext.AddAsync(userModel);
            await _contactosContext.SaveChangesAsync();

            if (userModel.IdUsuario != 0) return StatusCode(StatusCodes.Status200OK, new { isSuccess = true }); else return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var usuarioEncontrado = await _contactosContext.Usuarios
                         .Where(u =>
                         u.CorreoUsuario == login.Correo &&
                         u.Clave == _utilidades.encriptarSHA256(login.Clave)
                         ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null) return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
        }
    }
}
