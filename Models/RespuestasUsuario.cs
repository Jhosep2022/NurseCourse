using System;
using System.Collections.Generic;

namespace NurseCourse.Models;

public partial class RespuestasUsuario
{
    public int RespuestaUsuarioId { get; set; }

    public int OpcionId { get; set; }

    public int PreguntaId { get; set; }

    public int UsuarioId { get; set; }

    public virtual OpcionesDeRespuesta Opcion { get; set; } = null!;

    public virtual Pregunta Pregunta { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
