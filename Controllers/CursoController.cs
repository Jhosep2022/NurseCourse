using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace NurseCourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CursosController : ControllerBase
{
    private readonly DbnurseCourseContext _context;

    public CursosController(DbnurseCourseContext context)
    {
        _context = context;
    }

    // GET: api/Cursos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CursoDto>>> GetAllCursos()
    {
        var cursos = await _context.Cursos
            .Select(c => new CursoDto
            {
                CursoId = c.CursoId,
                Titulo = c.Titulo,
                Descripcion = c.Descripcion
            })
            .ToListAsync();

        return Ok(cursos);
    }

    // GET: api/Cursos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CursoDto>> GetCurso(int id)
    {
        var curso = await _context.Cursos
            .Where(c => c.CursoId == id)
            .Select(c => new CursoDto
            {
                CursoId = c.CursoId,
                Titulo = c.Titulo,
                Descripcion = c.Descripcion
            })
            .FirstOrDefaultAsync();

        if (curso == null)
        {
            return NotFound();
        }

        return curso;
    }

    // POST: api/Cursos
    [HttpPost]
    public async Task<ActionResult<CursoDto>> CreateCurso([FromBody] CursoDto cursoDto)
    {
        var curso = new Curso
        {
            Titulo = cursoDto.Titulo,
            Descripcion = cursoDto.Descripcion
        };
        _context.Cursos.Add(curso);
        await _context.SaveChangesAsync();

        var createdCursoDto = new CursoDto
        {
            CursoId = curso.CursoId,
            Titulo = curso.Titulo,
            Descripcion = curso.Descripcion
        };

        return createdCursoDto;
    }


    // PUT: api/Cursos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCurso(int id, [FromBody] CursoDto cursoDto)
    {
        if (id != cursoDto.CursoId)
        {
            return BadRequest();
        }

        var curso = await _context.Cursos.FindAsync(id);
        if (curso == null)
        {
            return NotFound();
        }

        curso.Titulo = cursoDto.Titulo;
        curso.Descripcion = cursoDto.Descripcion;
        _context.Cursos.Update(curso);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Cursos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCurso(int id)
    {
        var curso = await _context.Cursos.FindAsync(id);
        if (curso == null)
        {
            return NotFound();
        }

        _context.Cursos.Remove(curso);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
