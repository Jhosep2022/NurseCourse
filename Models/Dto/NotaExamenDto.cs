namespace NurseCourse.Models.DTOs;

public class NotaExamenDto
{
    public int NotaExamenId { get; set; }
    public int UsuarioId { get; set; }
    public int ExamenId { get; set; }
    public double Calificacion { get; set; }
}
