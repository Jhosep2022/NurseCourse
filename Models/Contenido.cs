using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class Contenido
{
    public int ContenidoId { get; set; }

    public string Tipo { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Texto { get; set; } = null!;

    public int ModuloId { get; set; }

    public virtual Modulo Modulo { get; set; } = null!;

    public virtual ICollection<Progreso> Progresos { get; set; } = new List<Progreso>();
}
