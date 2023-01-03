using System;
using System.Collections.Generic;
using System.Text;

namespace AppMecanizado.Models
{
    public class RespuestaMecanizado
    {
        public int p_Id_respuesta_mecanizado { get; set; }
        public int p_Id_servicio { get; set; }
        public string p_Estado { get; set; }
        //Estado=> Pendiente/En proceso/Parcial/Finalizado
        public string p_Observacion { get; set; }
        //Puede ser Null
    }
}
