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
public class ModulosController : ControllerBase
{
    private readonly DbnurseCourseContext _context;

    public ModulosController(DbnurseCourseContext context)
    {
        _context = context;
    }

    // GET: api/Modulos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ModuloDto>>> GetAllModulos()
    {
        var modulos = await _context.Modulos
            .Select(m => new ModuloDto
            {
                ModuloId = m.ModuloId,
                Nombre = m.Nombre,
                Orden = m.Orden,
                CursoId = m.CursoId
            })
            .ToListAsync();

        return Ok(modulos);
    }

    // GET: api/Modulos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ModuloDto>> GetModulo(int id)
    {
        var modulo = await _context.Modulos
            .Where(m => m.ModuloId == id)
            .Select(m => new ModuloDto
            {
                ModuloId = m.ModuloId,
                Nombre = m.Nombre,
                Orden = m.Orden,
                CursoId = m.CursoId
            })
            .FirstOrDefaultAsync();

        if (modulo == null)
        {
            return NotFound();
        }

        return modulo;
    }

    // POST: api/Modulos
    [HttpPost]
    public async Task<ActionResult<ModuloDto>> CreateModulo([FromBody] ModuloDto moduloDto)
    {
        var modulo = new Modulo
        {
            Nombre = moduloDto.Nombre,
            Orden = moduloDto.Orden,
            CursoId = moduloDto.CursoId
        };
        _context.Modulos.Add(modulo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetModulo), new { id = modulo.ModuloId }, moduloDto);
    }

    // PUT: api/Modulos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModulo(int id, [FromBody] ModuloDto moduloDto)
    {
        if (id != moduloDto.ModuloId)
        {
            return BadRequest();
        }

        var modulo = await _context.Modulos.FindAsync(id);
        if (modulo == null)
        {
            return NotFound();
        }

        modulo.Nombre = moduloDto.Nombre;
        modulo.Orden = moduloDto.Orden;
        modulo.CursoId = moduloDto.CursoId;
        _context.Modulos.Update(modulo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Modulos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModulo(int id)
    {
        var modulo = await _context.Modulos.FindAsync(id);
        if (modulo == null)
        {
            return NotFound();
        }

        _context.Modulos.Remove(modulo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
