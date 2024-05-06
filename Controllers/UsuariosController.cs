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
            RolId = registroDto.RolId
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Crear automáticamente el progreso inicial para el nuevo usuario
        var contenidoInicial = await _context.Contenidos.FirstOrDefaultAsync(); // Obtener el contenido inicial
        if (contenidoInicial != null)
        {
            var progreso = new Progreso
            {
                ModuloActual = 1, // O el módulo inicial correspondiente
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAllUsuarios()
    {
        try
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Progresos)
                .Include(u => u.Rol)
                .ToListAsync();

            if (usuarios == null || !usuarios.Any())
            {
                return NotFound("No se encontraron usuarios.");
            }

            var usuariosDto = usuarios.Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                CorreoElectronico = u.CorreoElectronico,
                Contraseña = u.Contraseña,
                Edad = u.Edad,
                Cargo = u.Cargo,
                RolId = u.RolId,
                Progresos = u.Progresos.Select(p => new ProgresoDto
                {
                    // Asignar propiedades de ProgresoDto
                }).ToList(),
                NotasExamenes = u.NotasExamenes.Select(n => new NotaExamenDto
                {
                    // Asignar propiedades de NotaExamenDto
                }).ToList()
            }).ToList();

            return Ok(usuariosDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


}
