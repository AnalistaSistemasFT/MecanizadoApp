using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Drawing;
using Android.Graphics;
using System.Data;
using System.Data.SqlClient;

namespace AppMecanizado.Models
{
    public class ServicioMecanizado
    {
        public int p_Id_servicio_mecanizado { get; set; }
        public int p_Id_solicitante { get; set; }
        public DateTime p_Fecha { get; set; }
        public string p_Centro_trabajo { get; set; }
        public int p_Prioridad { get; set; }
        public int p_Producto { get; set; }
        public string p_Descripcion { get; set; }
        public int p_Cantidad { get; set; }
        public byte[] p_Muestra_img { get; set; }
        public byte[] p_Plano_img { get; set; }
        public string p_Material { get; set; }
        public string p_Dureza { get; set; }
        public string p_Otros { get; set; }
        public DateTime p_Fecha_Finalizacion { get; set; }
        //Default: 7 dias de la fecha actual
        public int p_Estado { get; set; }
        public int p_Id_Empleado { get; set; }
        public int p_Cantidad_pendiente { get; set; }
        public string Codigo { get; set; }
        public int Correlativo { get; set; }
        public DateTime p_Fecha_estimada { get; set; }
        public int p_Liberado { get; set; }
        public string Observaciones { get; set; }
        public string CodRelacionado { get; set; }
        public string p_display_lista { get { return $"Orden: {Codigo} - {p_Fecha.ToString("dd/mm/yyyy")} - {p_Estado}"; } }
    }
}