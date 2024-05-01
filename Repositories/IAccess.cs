using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NurseCourse.Models;
using NurseCourse.Models.CustomModels;


namespace NurseCourse.Repositories
{
    public interface IAccess
    {
        Task<bool> CreatePerson(Person person, User user, string confirmPassword);
        Task<User> Login(LoginDTO login);
        Task<int> RecoveryPassword(string email);
        Task<bool> ChangePassword(string newPassword, string confirmPassword, string code, int id);
    }
}
