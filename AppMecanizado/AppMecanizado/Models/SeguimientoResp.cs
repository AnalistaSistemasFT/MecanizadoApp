using System;
using System.Collections.Generic;
using System.Text;

namespace AppMecanizado.Models
{
    public class SeguimientoResp
    {
        public int p_Id_servicio_mecanizado { get; set; }
        public string CentroTrabajo { get; set; }
        public string Producto { get; set; }
        public DateTime Fecha { get; set; }
        public string Encargado { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha_Final { get; set; }
        public int p_Id_solicitante { get; set; }
    }
}
