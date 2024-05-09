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
using NurseCourse.Models.DTOs;

namespace NurseCourse.Controllers;

public class StudentController : Controller
{
    private readonly HttpClient _httpClient;

    public StudentController()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5053/api/")
        };
    }
    public async Task<IActionResult> Index()
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

    public async Task<IActionResult> Curso(int? id, int? iterador)
    {
        if (id == null || iterador == null)
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
        List<gProgreso> permitidos = new List<gProgreso>();
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync("http://localhost:5053/api/progreso/usuario/"+id))
            {
                if(response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    permitidos = JsonConvert.DeserializeObject<List<gProgreso>>(apiResponse);
                }else{
                    permitidos = null;
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
        ViewData["id"] = id;
        ViewData["iterador"] = iterador;
        ViewData["Link1"] = examenes[0].linkExame;
        ViewData["Link2"] = examenes[1].linkExame;
        var listaIds = new List<int>();
        for(int i = 0; i < modulos.Count; i++)
        {
            listaIds.Add(modulos[i].moduloID);
        }
        var model = Tuple.Create(modulos, listaIds);
        return View(model);
    }
}