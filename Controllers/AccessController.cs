using Microsoft.AspNetCore.Mvc;
using NurseCourse.Repositories;

namespace NurseCourse.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccess _access;

        public AccessController(IAccess iaccess) => _access = iaccess;

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
