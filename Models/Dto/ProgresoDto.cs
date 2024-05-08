namespace NurseCourse.Models.DTOs
{
    public class ProgresoDto
    {
        public int ProgresoId { get; set; }
        public int ModuloActual { get; set; }
        public bool Completo { get; set; }
        public int ContenidoId { get; set; }
        public int UsuarioId { get; set; }
        public int CursoId { get; set; } 
    }
}