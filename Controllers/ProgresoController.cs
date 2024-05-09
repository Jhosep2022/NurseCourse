using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NurseCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgresoController : ControllerBase
    {
        private readonly DbnurseCourseContext _context;

        public ProgresoController(DbnurseCourseContext context)
        {
            _context = context;
        }

        [HttpPut("update")] 
        public async Task<IActionResult> UpdateProgreso([FromBody] ProgresoDto progresoDto)
        {
            try
            {
                var contenidoExists = await _context.Contenidos.AnyAsync(c => c.ContenidoId == progresoDto.ContenidoId);
                if (!contenidoExists)
                {
                    return NotFound($"Contenido con ID {progresoDto.ContenidoId} no encontrado.");
                }

                var progreso = await _context.Progreso
                    .Include(p => p.Contenido)
                    .ThenInclude(c => c.Modulo)
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

                return NoContent(); // Retorna 204 No Content cuando la actualización es exitosa
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error de base de datos: {dbEx.InnerException?.Message ?? dbEx.Message}");
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
                    .Include(p => p.Contenido)
                    .ThenInclude(c => c.Modulo)
                    .Where(p => p.UsuarioId == usuarioId)
                    .Select(p => new ProgresoDto
                    {
                        ProgresoId = p.ProgresoId,
                        ModuloActual = p.ModuloActual,
                        Completo = p.Completo,
                        ContenidoId = p.ContenidoId,
                        UsuarioId = p.UsuarioId,
                        CursoId = p.Contenido.Modulo.CursoId
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
}