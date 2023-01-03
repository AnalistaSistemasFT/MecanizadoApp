using System;
using System.Collections.Generic;
using System.Text;

namespace AppMecanizado.Models
{
    public class UsuarioMecanizado
    {
        public int p_Id_usuario_mecanizado { get; set; }
        public int p_Id_empleado { get; set; }
        public string p_Login { get; set; }
        public string p_Clave { get; set; }
        public string p_Nombre { get; set; }
        public int p_Id_grupo { get; set; }
    }
}