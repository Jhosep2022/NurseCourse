namespace NurseCourse.Models.DTOs;

public class RegistroUsuarioDto
{
    public string Nombre { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contraseña { get; set; }
    public int Edad { get; set; }
    public string Cargo { get; set; }
    public int RolId { get; set; }
    public string Telefono { get; set; }  // Nuevo campo Telefono
}

