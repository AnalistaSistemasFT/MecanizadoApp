using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppMecanizado.Models
{
    public class ServicioMecanizadoLista
    {
        public string Id_servicio_mecanizado { get; set; }
        public int Id_solicitante { get; set; }
        public DateTime Fecha { get; set; }
        public string Centro_trabajo { get; set; }
        public string Prioridad { get; set; }
        public string Estado { get; set; }
        public string Producto { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_Finalizacion { get; set; }
        public string Codigo { get; set; }
        public int Id_servicioMecanizado { get; set; }
        //MEC-AÑO-CORRELATIVO
        public DateTime p_Fecha_estimada { get; set; }
        public int p_Liberado { get; set; }
        public string Observaciones { get; set; }
        public string CodRelacionado { get; set; }
        public string display_lista_pend { get { return $"Orden: {Codigo} - {Fecha:dd/MM/yyyy} - {Estado}"; } }
        public string display_lista_proc { get { return $"Orden: {Codigo} - {p_Fecha_estimada:dd/MM/yyyy} - {Estado}"; } }
        public string display_lista_pfin { get { return $"Orden: {Codigo} - {Fecha_Finalizacion:dd/MM/yyyy} - {Estado}"; } }
    }
}
