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
}

