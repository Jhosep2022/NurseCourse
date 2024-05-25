namespace NurseCourse.Models.DTOs
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contraseña { get; set; }
        public int Edad { get; set; }
        public string Cargo { get; set; }
        public int RolId { get; set; }
        public string Telefono { get; set; }  // Nuevo campo Telefono
        public ICollection<ProgresoDto> Progresos { get; set; } = new List<ProgresoDto>();
        public ICollection<NotaExamenDto> NotasExamenes { get; set; } = new List<NotaExamenDto>();
    }

    public class UpdateUserRoleDto
    {
        public int UsuarioId { get; set; }
        public int NuevoRolId { get; set; }
    }
}
