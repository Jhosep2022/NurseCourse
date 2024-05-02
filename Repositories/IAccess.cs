using NurseCourse.Models;
using NurseCourse.Models.DTOs;
using System.Threading.Tasks;


namespace NurseCourse.Repositories
{
    public interface IAccess{
        Task<Usuario> Register(RegistroUsuarioDto registroDto);
        Task<Usuario> Login(LoginDTO loginDto);
        Task<Usuario> GetUsuario(int id);   

    }
}
