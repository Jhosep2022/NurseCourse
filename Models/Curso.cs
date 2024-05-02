using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class Curso
{
    public int CursoId { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Examene> Examenes { get; set; } = new List<Examene>();

    public virtual ICollection<Modulo> Modulos { get; set; } = new List<Modulo>();
}
