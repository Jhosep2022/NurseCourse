using Microsoft.AspNetCore.Mvc;
using NurseCourse.Models;

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using NewtonsoftJson = Newtonsoft.Json;

namespace NurseCourse.Controllers;

public class NurseController : Controller
{
    private readonly HttpClient _httpClient;

    public NurseController()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5053/api/")
        };
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RegistroCurso()
    {
        return View();
    }

    public IActionResult Calificaciones()
    {
        return View();
    }

    public async Task<IActionResult> DetallesModulo()
    {
        List<gUsuario> users = new List<gUsuario>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/usuarios"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<gUsuario>>(apiResponse);
            }
        }

        return View(users);
    }

    public async Task<IActionResult> ListaCursos()
    {
        List<gCurso> cursos = new List<gCurso>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Cursos"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                cursos = JsonConvert.DeserializeObject<List<gCurso>>(apiResponse);
            }
        }

        return View(cursos);
    }

    public async Task<IActionResult> ListaUsers()
    {
        List<gUsuario> users = new List<gUsuario>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/usuarios"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<gUsuario>>(apiResponse);
            }
        }

        return View(users);
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> DetallesCurso(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        List<gModulo> modulos = new List<gModulo>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Modulos/Curso/"+id))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    modulos = JsonConvert.DeserializeObject<List<gModulo>>(apiResponse);
                }else{
                    modulos = null;
                }
            }
        }
        List<gExamenes> examenes = new List<gExamenes>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Examenes/Curso/"+id))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    examenes = JsonConvert.DeserializeObject<List<gExamenes>>(apiResponse);
                }else{
                    examenes = null;
                }
            }
        }
        List<gUsuario> personas = new List<gUsuario>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/usuarios"))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    personas = JsonConvert.DeserializeObject<List<gUsuario>>(apiResponse);
                }else{
                    personas = null;
                }
            }
        }        
        ViewData["Link1"] = examenes[0].linkExame;
        ViewData["Link2"] = examenes[1].linkExame;
        ViewData["NumTimes"] = examenes.Count;

        // Pasar las listas al modelo de la vista
        var model = Tuple.Create(modulos, personas);
        return View(model);
    }
    
    public async Task<IActionResult> ListaModulos()
    {
        List<gCurso> cursos = new List<gCurso>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Cursos"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                cursos = JsonConvert.DeserializeObject<List<gCurso>>(apiResponse);
            }
        }

        return View(cursos);
    }

    public async Task<IActionResult> RegistroModulo()
    {
        List<gCurso> cursos = new List<gCurso>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/Cursos"))
            {
                //Console.WriteLine(response+"-- response --"+response.Content.ReadAsStringAsync()+"-- response.Contentaaaa --"+response.Equals("value"));
                string apiResponse = await response.Content.ReadAsStringAsync();
                cursos = JsonConvert.DeserializeObject<List<gCurso>>(apiResponse);
            }
        }

        return View(cursos);
    }
}