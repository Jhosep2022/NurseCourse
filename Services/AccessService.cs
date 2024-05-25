using Microsoft.EntityFrameworkCore;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using NurseCourse.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NurseCourse.Services
{
    public class AccessService : IAccess
    {
        private readonly DbnurseCourseContext _context;

        public AccessService(DbnurseCourseContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Register(RegistroUsuarioDto registroDto)
        {
            if (_context.Usuarios.Any(u => u.CorreoElectronico == registroDto.CorreoElectronico))
            {
                return null;
            }

            var usuario = new Usuario
            {
                Nombre = registroDto.Nombre,
                CorreoElectronico = registroDto.CorreoElectronico,
                Contraseña = HashPassword(registroDto.Contraseña),
                Edad = registroDto.Edad,
                Cargo = registroDto.Cargo,
                RolId = registroDto.RolId
                
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> Login(LoginDTO loginDto)
        {
            var usuario = await _context.Usuarios
                                        .FirstOrDefaultAsync(u => u.CorreoElectronico == loginDto.UserName && u.Contraseña == HashPassword(loginDto.Password));

            return usuario;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = new StringBuilder();

            foreach (var b in hashedBytes)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }

        public Task<Usuario> GetUsuario(int id)
        {
            throw new NotImplementedException();
        }
    }
}
