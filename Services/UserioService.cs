using NurseCourse.Data;
using NurseCourse.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NurseCourse.Services
{
    public class UsuarioService
    {
        private readonly DbnurseCourseContext _context;

        public UsuarioService(DbnurseCourseContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Progresos)
                .Include(u => u.RespuestasUsuarios)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            return usuario;
        }

        
    }
}
