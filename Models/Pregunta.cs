using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class Pregunta
{
    public int PreguntaId { get; set; }

    public string TextoPregunta { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public int ExamenId { get; set; }

    public virtual Examene Examen { get; set; } = null!;

    public virtual ICollection<OpcionesDeRespuesta> OpcionesDeRespuesta { get; set; } = new List<OpcionesDeRespuesta>();

    public virtual ICollection<RespuestasUsuario> RespuestasUsuarios { get; set; } = new List<RespuestasUsuario>();
}
