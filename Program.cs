using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NurseCourse.Data;
using NurseCourse.Repositories;
using NurseCourse.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json.Serialization; // Importa este namespace para JsonSerializerOptions

var builder = WebApplication.CreateBuilder(args);

// servicio minio
builder.Services.AddScoped<FileStorageService>();

// Configuración de DbContext para usar MySQL
builder.Services.AddDbContext<DbnurseCourseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Configuración de cookies para la autenticación
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cookie =>
    {
        cookie.LoginPath = "/Access/Login"; 
        cookie.ExpireTimeSpan = TimeSpan.FromDays(7); 
        cookie.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Agrega los controladores con configuración JSON para evitar ciclos
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true; // Si quieres que el JSON se indente para facilitar la lectura
});

builder.Services.AddScoped<IAccess, AccessService>(); 
builder.Services.AddScoped<UsuarioService>(); // Agregando UsuarioService

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
