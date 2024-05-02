using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class OpcionesDeRespuesta
{
    public int OpcionId { get; set; }

    public string TextoOpcion { get; set; } = null!;

    public bool EsCorrecta { get; set; }

    public int PreguntaId { get; set; }

    public virtual Pregunta Pregunta { get; set; } = null!;

    public virtual ICollection<RespuestasUsuario> RespuestasUsuarios { get; set; } = new List<RespuestasUsuario>();
}
