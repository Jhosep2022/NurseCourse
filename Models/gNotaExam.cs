using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NurseCourse.Models;

public partial class gNotaExamen
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int notaExamenId { get; set; }

    [ForeignKey("Usuario")]
    public int usuarioId { get; set; }

    [ForeignKey("Examen")]
    public int examenId { get; set; }

    public int CursoId { get; set; }

    public double calificacion { get; set; }

    public virtual Usuario Usuario { get; set; }
    public virtual Examene Examen { get; set; }
}
