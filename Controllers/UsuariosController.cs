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
    public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Progresos)
            .Include(u => u.NotasExamenes)
                .ThenInclude(ne => ne.Examen)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        // Asumimos que hay una propiedad CursoId en el modelo Examen
        var usuarioDto = new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            Nombre = usuario.Nombre,
            CorreoElectronico = usuario.CorreoElectronico,
            Contraseña = usuario.Contraseña,
            Edad = usuario.Edad,
            Cargo = usuario.Cargo,
            RolId = usuario.RolId,
            Progresos = usuario.Progresos.Select(p => new ProgresoDto
            {
                // Mapear propiedades de ProgresoDto
            }).ToList(),
            NotasExamenes = usuario.NotasExamenes.Select(ne => new NotaExamenDto
            {
                NotaExamenId = ne.NotaExamenId,
                UsuarioId = ne.UsuarioId,
                ExamenId = ne.ExamenId,
                CursoId = ne.Examen.CursoId, // Accedemos a CursoId desde Examen
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
            Progresos = u.Progresos.Select(p => new ProgresoDto
            {
                // Mapear propiedades de ProgresoDto
            }).ToList(),
            NotasExamenes = u.NotasExamenes.Select(ne => new NotaExamenDto
            {
                NotaExamenId = ne.NotaExamenId,
                UsuarioId = ne.UsuarioId,
                ExamenId = ne.ExamenId,
                CursoId = ne.Examen.CursoId, // Accedemos a CursoId desde Examen
                Calificacion = ne.Calificacion
            }).ToList()
        }).ToList();

        return Ok(usuariosDto);
    }
}