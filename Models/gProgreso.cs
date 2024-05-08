namespace NurseCourse.Models.DTOs;

public class gProgreso
{
    public int progresoId { get; set; }
    public int moduloActual { get; set; }
    public bool completo { get; set; }
    public int contenidoId { get; set; }
    public int usuarioId { get; set; }
}
