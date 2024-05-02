namespace NurseCourse.Models;

public partial class NotaExamen
{
    public int NotaExamenId { get; set; }
    public int UsuarioId { get; set; }
    public int ExamenId { get; set; }
    public double Calificacion { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Examene Examen { get; set; } = null!;
}
