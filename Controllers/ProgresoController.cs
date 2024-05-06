using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace NurseCourse.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProgresoController : ControllerBase
{
    private readonly DbnurseCourseContext _context;

    public ProgresoController(DbnurseCourseContext context)
    {
        _context = context;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateProgreso([FromBody] ProgresoDto progresoDto)
    {
        try
        {
            var progreso = await _context.Progreso
                .FirstOrDefaultAsync(p => p.ProgresoId == progresoDto.ProgresoId);

            if (progreso == null)
            {
                return NotFound("Progreso no encontrado.");
            }

            progreso.ModuloActual = progresoDto.ModuloActual;
            progreso.Completo = progresoDto.Completo;
            progreso.ContenidoId = progresoDto.ContenidoId;
            progreso.UsuarioId = progresoDto.UsuarioId;

            _context.Progreso.Update(progreso);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<ProgresoDto>>> GetProgresoByUsuario(int usuarioId)
    {
        try
        {
            var progresos = await _context.Progreso
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => new ProgresoDto
                {
                    ProgresoId = p.ProgresoId,
                    ModuloActual = p.ModuloActual,
                    Completo = p.Completo,
                    ContenidoId = p.ContenidoId,
                    UsuarioId = p.UsuarioId
                })
                .ToListAsync();

            if (progresos == null || !progresos.Any())
            {
                return NotFound($"No se encontraron progresos para el usuario con ID {usuarioId}");
            }

            return Ok(progresos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


}
