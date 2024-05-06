using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class gExamenes
{
    public int examenId { get; set; }
    public string titulo { get; set; } = null!;
    public int cursoId { get; set; }
    public string linkExame { get; set; } = null!;
    public virtual Curso Curso { get; set; } = null!;
    public virtual ICollection<NotaExamen> NotasExamenes { get; set; } = new List<NotaExamen>(); // New collection
}

