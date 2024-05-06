using System;
using System.Collections.Generic;

namespace NurseCourse.Models
{
    public partial class gModulo
    {
        public string id { get; set; }
        public int moduloID { get; set; }

        public string nombre { get; set; } = null!;

        public string descripcion { get; set; } 

        public int cursoId { get; set; }
    }
}