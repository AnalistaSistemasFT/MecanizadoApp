using System;
using System.Collections.Generic;
using System.Text;

namespace AppMecanizado.Models
{
    public class Usuario
    {
        public string Login { get; set; }
        public string Clave { get; set; }
        public int Activo { get; set; }
        public int EmpleadoId { get; set; }
    }
}