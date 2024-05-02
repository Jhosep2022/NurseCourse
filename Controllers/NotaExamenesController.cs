using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace NurseCourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotaExamenesController : ControllerBase
{
    private readonly DbnurseCourseContext _context;

    public NotaExamenesController(DbnurseCourseContext context)
    {
        _context = context;
    }

    // GET: api/NotaExamenes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotaExamenDto>>> GetAllNotaExamenes()
    {
        var notas = await _context.NotasExamenes
            .Select(n => new NotaExamenDto
            {
                NotaExamenId = n.NotaExamenId,
                UsuarioId = n.UsuarioId,
                ExamenId = n.ExamenId,
                Calificacion = n.Calificacion
            })
            .ToListAsync();

        return Ok(notas);
    }

    // GET: api/NotaExamenes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<NotaExamenDto>> GetNotaExamen(int id)
    {
        var nota = await _context.NotasExamenes
            .Where(n => n.NotaExamenId == id)
            .Select(n => new NotaExamenDto
            {
                NotaExamenId = n.NotaExamenId,
                UsuarioId = n.UsuarioId,
                ExamenId = n.ExamenId,
                Calificacion = n.Calificacion
            })
            .FirstOrDefaultAsync();

        if (nota == null)
        {
            return NotFound();
        }

        return nota;
    }

    // POST: api/NotaExamenes
    [HttpPost]
    public async Task<ActionResult<NotaExamenDto>> CreateNotaExamen([FromBody] NotaExamenDto notaDto)
    {
        var nota = new NotaExamen
        {
            UsuarioId = notaDto.UsuarioId,
            ExamenId = notaDto.ExamenId,
            Calificacion = notaDto.Calificacion
        };

        _context.NotasExamenes.Add(nota);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNotaExamen), new { id = nota.NotaExamenId }, notaDto);
    }

    // PUT: api/NotaExamenes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNotaExamen(int id, [FromBody] NotaExamenDto notaDto)
    {
        if (id != notaDto.NotaExamenId)
        {
            return BadRequest();
        }

        var nota = await _context.NotasExamenes.FindAsync(id);
        if (nota == null)
        {
            return NotFound();
        }

        nota.UsuarioId = notaDto.UsuarioId;
        nota.ExamenId = notaDto.ExamenId;
        nota.Calificacion = notaDto.Calificacion;
        _context.NotasExamenes.Update(nota);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/NotaExamenes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotaExamen(int id)
    {
        var nota = await _context.NotasExamenes.FindAsync(id);
        if (nota == null)
        {
            return NotFound();
        }

        _context.NotasExamenes.Remove(nota);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
