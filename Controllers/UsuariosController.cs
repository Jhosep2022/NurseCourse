using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            RolId = registroDto.RolId,
            Telefono = registroDto.Telefono  // Asignando el teléfono
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var contenidoInicial = await _context.Contenidos.FirstOrDefaultAsync();
        if (contenidoInicial != null)
        {
            var progreso = new Progreso
            {
                ModuloActual = 1,
                Completo = false,
                ContenidoId = contenidoInicial.ContenidoId,
                UsuarioId = usuario.UsuarioId
            };

            _context.Progreso.Add(progreso);
            await _context.SaveChangesAsync();
        }

        return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioId }, usuario);
    }

    [HttpPost("login")]
    public async Task<ActionResult<Usuario>> Login(LoginDTO loginDto)
    {
        var usuario = await _context.Usuarios
                                    .FirstOrDefaultAsync(u => u.CorreoElectronico == loginDto.UserName && u.Contraseña == loginDto.Password);
        
        if (usuario == null)
        {
            return Unauthorized("Credenciales inválidas");
        }

        return Ok(usuario);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Progresos)
                .ThenInclude(p => p.Contenido)
                    .ThenInclude(c => c.Modulo)
            .Include(u => u.NotasExamenes)
                .ThenInclude(ne => ne.Examen)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        var usuarioDto = new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            Nombre = usuario.Nombre,
            CorreoElectronico = usuario.CorreoElectronico,
            Contraseña = usuario.Contraseña,
            Edad = usuario.Edad,
            Cargo = usuario.Cargo,
            RolId = usuario.RolId,
            Telefono = usuario.Telefono,  // Asegúrate de incluir el teléfono aquí
            Progresos = usuario.Progresos.Select(p => new ProgresoDto
            {
                ProgresoId = p.ProgresoId,
                ModuloActual = p.ModuloActual,
                Completo = p.Completo,
                ContenidoId = p.ContenidoId,
                UsuarioId = p.UsuarioId,
                CursoId = p.Contenido.Modulo.CursoId
            }).ToList(),
            NotasExamenes = usuario.NotasExamenes.Select(ne => new NotaExamenDto
            {
                NotaExamenId = ne.NotaExamenId,
                UsuarioId = ne.UsuarioId,
                ExamenId = ne.ExamenId,
                CursoId = ne.Examen.CursoId,
                Calificacion = ne.Calificacion
            }).ToList()
        };

        return Ok(usuarioDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAllUsuarios()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.Progresos)
                .ThenInclude(p => p.Contenido)
                    .ThenInclude(c => c.Modulo)
            .Include(u => u.NotasExamenes)
                .ThenInclude(ne => ne.Examen)
            .ToListAsync();

        var usuariosDto = usuarios.Select(u => new UsuarioDto
        {
            UsuarioId = u.UsuarioId,
            Nombre = u.Nombre,
            CorreoElectronico = u.CorreoElectronico,
            Contraseña = u.Contraseña,
            Edad = u.Edad,
            Cargo = u.Cargo,
            RolId = u.RolId,
            Telefono = u.Telefono,  // Incluye el teléfono aquí también
            Progresos = u.Progresos.Select(p => new ProgresoDto
            {
                ProgresoId = p.ProgresoId,
                ModuloActual = p.ModuloActual,
                Completo = p.Completo,
                ContenidoId = p.ContenidoId,
                UsuarioId = p.UsuarioId,
                CursoId = p.Contenido.Modulo.CursoId
            }).ToList(),
            NotasExamenes = u.NotasExamenes.Select(ne => new NotaExamenDto
            {
                NotaExamenId = ne.NotaExamenId,
                UsuarioId = ne.UsuarioId,
                ExamenId = ne.ExamenId,
                CursoId = ne.Examen.CursoId,
                Calificacion = ne.Calificacion
            }).ToList()
        }).ToList();

        return Ok(usuariosDto);
    }


    [HttpPut("update-role")]
    public async Task<IActionResult> PutUpdateUserRole(UpdateUserRoleDto updateRoleDto)
    {
        var usuario = await _context.Usuarios.FindAsync(updateRoleDto.UsuarioId);

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        var rolExistente = await _context.Roles.FindAsync(updateRoleDto.NuevoRolId);

        if (rolExistente == null)
        {
            return BadRequest("Rol especificado no existe.");
        }

        usuario.RolId = updateRoleDto.NuevoRolId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Usuarios.Any(e => e.UsuarioId == updateRoleDto.UsuarioId))
            {
                return NotFound("Usuario no encontrado para actualizar.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.NotasExamenes)
            .Include(u => u.Progresos)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        _context.NotasExamenes.RemoveRange(usuario.NotasExamenes);
        _context.Progreso.RemoveRange(usuario.Progresos);
        _context.Usuarios.Remove(usuario);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            return StatusCode(500, "No se puede eliminar el usuario porque tiene datos relacionados en otras tablas.");
        }

        return NoContent();
    }
}