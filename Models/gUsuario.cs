namespace NurseCourse.Models;

public partial class gUsuario
{
    public int usuarioId { get; set; }
    public string nombre { get; set; } = null!;
    public string correoElectronico { get; set; } = null!;
    public string contraseña { get; set; } = null!;
    public int edad { get; set; }
    public string cargo { get; set; } = null!;
    public int rolId { get; set; }
    public virtual ICollection<Progreso> progresos { get; set; } = new List<Progreso>();
    public virtual ICollection<NotaExamen> notasExamenes { get; set; } = new List<NotaExamen>(); // New collection
}

