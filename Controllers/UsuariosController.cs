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
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Progresos) 
            .Include(u => u.Rol)       
            .Include(u => u.NotasExamenes)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok(usuario);
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAllUsuarios()
    {
        try
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Progresos)
                .Include(u => u.Rol)
                .Include(u => u.NotasExamenes) // Asegúrate de incluir las notas de exámenes
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
                }).ToList(),
                NotasExamenes = u.NotasExamenes.Select(n => new NotaExamenDto
                {
                    NotaExamenId = n.NotaExamenId, 
                    UsuarioId = n.UsuarioId,
                    ExamenId = n.ExamenId,
                    Calificacion = n.Calificacion
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