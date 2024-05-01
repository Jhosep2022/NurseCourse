using Microsoft.EntityFrameworkCore;
using NurseCourse.Data;
using NurseCourse.Models;
using NurseCourse.Models.CustomModels;
using NurseCourse.Repositories;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace NurseCourse.Services
{
    public class AccessService : IAccess
    {
        private readonly DbnurseCourseContext _context;

        public AccessService(DbnurseCourseContext context)
        {
            _context = context;
        }
        public async Task<bool> ChangePassword(string newPassword, string confirmPassword, string code, int id)
        {
            if (newPassword == confirmPassword)
            {
                var codeEncrypted = GetSHA256(code);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id && m.codeRecovery == codeEncrypted);
                if (user != null)
                {
                    try
                    {
                        var passwordEncrypted = GetSHA256(newPassword);
                        user.Password = passwordEncrypted;
                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                return false;
            }
            return false;
        }

        public async Task<bool> CreatePerson(Person person, User user, string confirmPassword)
        {
            if (user.Password == confirmPassword)
            {
                using var transation = _context.Database.BeginTransaction();
                try
                {
                    await _context.People.AddAsync(person);
                    await _context.SaveChangesAsync();
                    user.Id = person.Id;
                    var pass = GetSHA256(user.Password);
                    user.Password = pass;
                    user.UserName = CreateUserName(person.FirstName, person.LastName, person.MinistryCode);
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    await transation.CommitAsync();
                    sendEmailUserName(person.Email, user.UserName);
                    return true;
                }
                catch
                {
                    await transation.RollbackAsync();
                    return false;
                }
            }
            return false;
        }

        private string CreateUserName(string firstName, string lastName, string ministryCode)
        {
            string newName = firstName.Substring(0, 2).ToLower();
            string newLastName = lastName.Substring(0, 2).ToLower();
            string newMinistryCode = ministryCode.Substring(0, 2).ToLower();

            string UserName = string.Concat(newName, newLastName, newMinistryCode);
            return UserName;
        }

        private string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null!;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        #region EmailCodeRecovery
        private void sendEmail(string to, string code)
        {
            StringBuilder body = new StringBuilder();

            const string Myuser = "victor00337@gmail.com";
            const string Mypassword = "skns bdbj jblu ydcv";

            try
            {
                body.Append(string.Format(" -- Curso Enfermeras -- "));
                body.Append(Environment.NewLine);
                body.Append(string.Format("Codigo de recuperación: " + code));
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress("victor00337@gmail.com");
                mail.To.Add(to);
                mail.Subject = "Credencial de acceso a la app Curso De Enfermeras";
                mail.Body = body.ToString();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Myuser, Mypassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region EmailUserName
        private void sendEmailUserName(string to, string userName)
        {
            StringBuilder body = new StringBuilder();

            const string Myuser = "victor00337@gmail.com";
            const string Mypassword = "skns bdbj jblu ydcv";

            try
            {
                body.Append(string.Format(" -- Curso Enfermeras -- "));
                body.Append(Environment.NewLine);
                body.Append(string.Format("Nombre de Usuario: " + userName));
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress("victor00337@gmail.com");
                mail.To.Add(to);
                mail.Subject = "Credencial de acceso a la app Curso De Enfermeras";
                mail.Body = body.ToString();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Myuser, Mypassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public async Task<User> Login(LoginDTO login)
        {
            string newPassword = GetSHA256(login.password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == login.userName && u.Password == newPassword);
            if (user == null)
            {
                return null!;
            }
            return user;
        }

        public async Task<int> RecoveryPassword(string email)
        {
            var code = "";
            var person = await _context.People.FirstOrDefaultAsync(m => m.Email == email);
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == person.Id);
            if (user != null)
            {
                code = Guid.NewGuid().ToString().Split("-")[0];

                user.codeRecovery = GetSHA256(code);
                _context.Entry(user).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
                sendEmail(email, code);
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }
            return person!.Id;
        }
    }
}
