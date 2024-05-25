namespace NurseCourse.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = null!;
    public string CorreoElectronico { get; set; } = null!;
    public string Contraseña { get; set; } = null!;
    public int Edad { get; set; }
    public string Cargo { get; set; } = null!;
    public int RolId { get; set; }
    public string Telefono { get; set; } = null!;  // Nuevo campo Telefono
    public virtual ICollection<Progreso> Progresos { get; set; } = new List<Progreso>();
    public virtual ICollection<NotaExamen> NotasExamenes { get; set; } = new List<NotaExamen>();
    public virtual Role Rol { get; set; } = null!;
}