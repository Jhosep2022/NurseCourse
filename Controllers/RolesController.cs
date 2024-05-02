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
public class RolesController : ControllerBase
{
    private readonly DbnurseCourseContext _context;

    public RolesController(DbnurseCourseContext context)
    {
        _context = context;
    }

    // GET: api/Roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
    {
        var roles = await _context.Roles
            .Select(r => new RoleDto
            {
                RolId = r.RolId,
                NombreRol = r.NombreRol
            })
            .ToListAsync();

        return Ok(roles);
    }
}
