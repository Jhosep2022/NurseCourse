using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class Examene
{
    public int ExamenId { get; set; }
    public string Titulo { get; set; } = null!;
    public int CursoId { get; set; }
    public string LinkExame { get; set; } = null!;
    public virtual Curso Curso { get; set; } = null!;
    public virtual ICollection<NotaExamen> NotasExamenes { get; set; } = new List<NotaExamen>(); // New collection
}

