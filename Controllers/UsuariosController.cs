using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Si usas métodos específicos de EF Core

namespace NurseCourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly DbnurseCourseContext _context;

    public UsuariosController(DbnurseCourseContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Usuario>> Register(RegistroUsuarioDto registroDto)
    {
        if (_context.Usuarios.Any(u => u.CorreoElectronico == registroDto.CorreoElectronico))
        {
            return BadRequest("Correo electrónico ya registrado.");
        }

        var usuario = new Usuario
        {
            Nombre = registroDto.Nombre,
            CorreoElectronico = registroDto.CorreoElectronico,
            Contraseña = registroDto.Contraseña, 
            Edad = registroDto.Edad,
            Cargo = registroDto.Cargo,
            RolId = registroDto.RolId
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioId }, usuario);
    }

    [HttpPost("login")]
    public async Task<ActionResult<Usuario>> Login(LoginDTO loginDto)
    {
        var usuario = await _context.Usuarios
                                    .FirstOrDefaultAsync(u => u.CorreoElectronico == loginDto.UserName && u.Contraseña == loginDto.Password); // Considere mejorar la seguridad aquí
        
        if (usuario == null)
        {
            return Unauthorized("Credenciales inválidas");
        }

        // Aquí podrías generar un token si estás usando JWT o simplemente retornar un resultado positivo
        return Ok(usuario);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Progresos)
            .Include(u => u.Nombre)
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return usuario;
    }


}
