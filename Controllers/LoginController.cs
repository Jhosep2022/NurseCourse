using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace NurseCourse.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Registro()
    {
        return View();
    }
}