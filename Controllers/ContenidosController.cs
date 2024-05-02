using Microsoft.AspNetCore.Mvc;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace NurseCourse.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ContenidosController : ControllerBase
{
    private readonly DbnurseCourseContext _context;
    private readonly FileStorageService _fileStorageService;

    public ContenidosController(DbnurseCourseContext context, FileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    [HttpPost]
    public async Task<ActionResult<ContenidoDto>> CreateContenido([FromForm] CreateContenidoDto createContenidoDto, IFormFile file)
    {
        var stream = file.OpenReadStream();
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var url = await _fileStorageService.UploadFileAsync(stream, fileName);

        var contenido = new Contenido
        {
            Tipo = createContenidoDto.Tipo,
            Url = url, // Esto será la URL ordinaria
            Texto = createContenidoDto.Texto,
            ModuloId = createContenidoDto.ModuloId
        };

        _context.Contenidos.Add(contenido);
        await _context.SaveChangesAsync();

        var presignedUrl = await _fileStorageService.GetPresignedUrlAsync(fileName);

        return CreatedAtAction(nameof(GetContenido), new { id = contenido.ContenidoId }, new ContenidoDto
        {
            ContenidoId = contenido.ContenidoId,
            Tipo = contenido.Tipo,
            Texto = contenido.Texto,
            ModuloId = contenido.ModuloId,
            Url = presignedUrl 
        });
    }


    [HttpGet("ByModulo/{moduloId}")]
    public async Task<ActionResult<IEnumerable<ContenidoDto>>> GetAllContenidosByModuloId(int moduloId)
    {
        var contenidos = await _context.Contenidos
            .Where(c => c.ModuloId == moduloId)
            .Select(c => new ContenidoDto
            {
                ContenidoId = c.ContenidoId,
                Tipo = c.Tipo,
                Url = c.Url,
                Texto = c.Texto,
                ModuloId = c.ModuloId
            })
            .ToListAsync();

        return Ok(contenidos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContenidoDto>> GetContenido(int id)
    {
        var contenido = await _context.Contenidos
            .Where(c => c.ContenidoId == id)
            .Select(c => new ContenidoDto
            {
                ContenidoId = c.ContenidoId,
                Tipo = c.Tipo,
                Url = c.Url,
                Texto = c.Texto,
                ModuloId = c.ModuloId
            })
            .FirstOrDefaultAsync();

        if (contenido == null)
        {
            return NotFound($"No se encontró contenido con ID {id}");
        }

        return Ok(contenido);
    }


    // Add other CRUD operations as needed
}
