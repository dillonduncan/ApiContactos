using ApiContactos.Models;
using ApiContactos.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ApiContactos.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private readonly ContactosContext _context;
        public ContactosController(ContactosContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Registros")]
        public async Task<ActionResult<IEnumerable<ContactoDTO>>> Registros(IEnumerable<ContactoDTO> contactos)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<Contacto> contactoList = new List<Contacto>();
                if (ModelState.IsValid)
                {
                    foreach (var contacto in contactos)
                    {
                        Contacto contactoDTO = new Contacto
                        {
                            NombreContacto = contacto.NombreContacto,
                            CorreoContacto = contacto.CorreoContacto,
                            CelularContacto = contacto.CelularContacto,
                            IdUsuario = int.Parse(userId!)
                        };
                        contactoList.Add(contactoDTO);
                    }
                    await _context.AddRangeAsync(contactoList);
                    await _context.SaveChangesAsync();
                }

                if (contactoList.Count > 0) return StatusCode(StatusCodes.Status200OK, new { isSuccess = true }); else return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });

            }
            catch (Exception ex) { return StatusCode(StatusCodes.Status200OK, new { message = ex.Message }); }
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro(ContactoDTO contacto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var contactoModel = new Contacto
                {
                    NombreContacto = contacto.NombreContacto,
                    CorreoContacto = contacto.CorreoContacto,
                    CelularContacto = contacto.CelularContacto,
                    IdUsuario = int.Parse(userId!)
                };
                await _context.AddAsync(contactoModel);
                await _context.SaveChangesAsync();
                if (contactoModel.IdContacto != 0) return StatusCode(StatusCodes.Status200OK, new { isSuccess = true }); else return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });

            }
            catch (Exception ex) { return StatusCode(StatusCodes.Status200OK, new { message = ex.Message }); }
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            //obtener el id del usuario logueado

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var listContactos = await _context.Contactos.Where(c => c.IdUsuario == Int32.Parse(userId)).ToListAsync();
            if (listContactos.Count > 0) return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, value = listContactos }); else return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, value = "" });

        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetContacto(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var contacto = await _context.Contactos.Where(c => c.IdUsuario == int.Parse(userId) && c.IdContacto == id).ToListAsync();
                if (contacto != null) return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, value = contacto }); else return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, value = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { Message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> EliminarContacto(int id)
        {
            try
            {
                var idUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Usuarios.FindAsync(int.Parse(idUsuario));
                var contactos = _context.Contactos.Where(c => c.IdUsuario == int.Parse(idUsuario!)).ToList();
                foreach (var contacto in contactos)
                {
                    if (contacto.IdContacto == id)
                    {
                        _context.Contactos.Remove(contacto);
                    }
                }
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { Message = ex.Message });
            }
        }

        [HttpPut("{idContacto}")]
        public async Task<IActionResult> Editar(int idContacto, ContactoDTO contacto)
        {
            try
            {
                var contactoExist = await _context.Contactos.FindAsync(idContacto);
                if (contactoExist == null)
                {
                    return BadRequest();
                }
                else
                {
                    contactoExist.NombreContacto = contacto.NombreContacto;
                    contactoExist.CorreoContacto = contacto.CorreoContacto;
                    contactoExist.CelularContacto = contacto.CelularContacto;

                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ContactoExiste(idContacto))
                {
                    return BadRequest();
                }
                return StatusCode(StatusCodes.Status200OK, new { Message = ex.Message });
            }
        }
        private bool ContactoExiste(int idContacto)
        {
            return _context.Contactos.Any(c => c.IdContacto == idContacto);
        }
    }
}
