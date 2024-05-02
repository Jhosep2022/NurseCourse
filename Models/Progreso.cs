using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class Progreso
{
    public int ProgresoId { get; set; }

    public int ModuloActual { get; set; }

    public bool Completo { get; set; }

    public int ContenidoId { get; set; }

    public int UsuarioId { get; set; }

    public virtual Contenido Contenido { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
