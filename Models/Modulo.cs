using System;
using System.Collections.Generic;

namespace NurseCourse.Models
{
    public partial class Modulo
    {
        public int ModuloID { get; set; }

        public string Nombre { get; set; } = null!;

        public string Descripcion { get; set; } 

        public int CursoId { get; set; }

        public virtual ICollection<Contenido> Contenidos { get; set; } = new List<Contenido>();

        public virtual Curso Curso { get; set; } = null!;
    }
}