using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NurseCourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamenesController : ControllerBase
{
    private readonly DbnurseCourseContext _context;

    public ExamenesController(DbnurseCourseContext context)
    {
        _context = context;
    }

    // GET: api/Examenes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExameneDto>>> GetAllExamenes()
    {
        var examenes = await _context.Examenes
            .Select(e => new ExameneDto
            {
                ExamenId = e.ExamenId,
                Titulo = e.Titulo,
                CursoId = e.CursoId,
                LinkExame = e.LinkExame
            })
            .ToListAsync();

        return Ok(examenes);
    }

    // GET: api/Examenes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ExameneDto>> GetExamene(int id)
    {
        var examene = await _context.Examenes
            .Where(e => e.ExamenId == id)
            .Select(e => new ExameneDto
            {
                ExamenId = e.ExamenId,
                Titulo = e.Titulo,
                CursoId = e.CursoId,
                LinkExame = e.LinkExame
            })
            .FirstOrDefaultAsync();

        if (examene == null)
        {
            return NotFound();
        }

        return examene;
    }

    // POST: api/Examenes
    [HttpPost]
    public async Task<ActionResult<ExameneDto>> CreateExamene([FromBody] ExameneDto exameneDto)
    {
        var examene = new Examene
        {
            Titulo = exameneDto.Titulo,
            CursoId = exameneDto.CursoId,
            LinkExame = exameneDto.LinkExame
        };

        _context.Examenes.Add(examene);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExamene), new { id = examene.ExamenId }, exameneDto);
    }

    // GET: api/Examenes/Curso/5
    [HttpGet("Curso/{cursoId}")]
    public async Task<ActionResult<IEnumerable<ExameneDto>>> GetExamenesByCurso(int cursoId)
    {
        var examenes = await _context.Examenes
            .Where(e => e.CursoId == cursoId)
            .Select(e => new ExameneDto
            {
                ExamenId = e.ExamenId,
                Titulo = e.Titulo,
                CursoId = e.CursoId,
                LinkExame = e.LinkExame
            })
            .ToListAsync();

        if (!examenes.Any())
        {
            return NotFound(new { message = $"No se encontraron exámenes para el curso con ID {cursoId}." });
        }

        return Ok(examenes);
    }

}
