using AppMecanizado.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMecanizado.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Seguimiento : ContentPage
    {
        private string _Codigo;
        int solicitante = App._idPersonal;
        string _nombreProd;
        string _producto = "";
        string _estado = "";
        int[] _IdRespuestaMecanizado = null;
        Conexion.Conex CON = new Conexion.Conex();
        private Conexion.Conex conex;
        DateTime _fecha;
        DateTime _fechaFinal;
        public Seguimiento(string Codigo, DateTime Fecha, string Centro_trabajo, string Producto, string Descripcion, string Estado)
        {
            InitializeComponent();
            this.conex = new Conexion.Conex();
            _Codigo = Codigo;
            txtNroServicio.Text = Codigo;
            txtCentroTrabajo.Text = Centro_trabajo;
            txtProducto.Text = Producto;
            txtDescripcion.Text = Descripcion;
            txtFecha.Text = Fecha.ToString("dddd, dd/MM/yyyy").ToUpper();
            txtEstado.Text = Estado;
            _estado = Estado;
            GetRespuesta();
        }
        async void GetRespuesta()
        {
            try
            {
                string sentencia = String.Format("SELECT p_Fecha_Finalizacion FROM tblServicioMecanizado " +
                    "WHERE p_Id_solicitante = '{0}' AND Codigo = '{1}'", solicitante, _Codigo);
                var data = CON.ejecutarConsulta(sentencia);
                foreach (DataRow item in data.Rows)
                {
                    _fechaFinal = Convert.ToDateTime(item[0]);
                }
                try
                {
                    //txtFecha.Text = _fecha.ToString("dddd, dd/MM/yyyy").ToUpperInvariant();
                    if (_estado == "Pendiente")
                    {
                        txtFechaEstimada.Text = String.Empty;
                    }
                    else if(_estado == "Finalizado")
                    {
                        lblFechaFin.Text = "Fecha de entrega";
                        txtFechaEstimada.Text = _fechaFinal.ToString("dddd, dd/MM/yyyy").ToUpperInvariant();
                    }
                    else
                    {
                        txtFechaEstimada.Text = _fechaFinal.ToString("dddd, dd/MM/yyyy").ToUpperInvariant();
                    }
                }
                catch(Exception ex)
                {
                    txtFechaEstimada.Text = "Fecha a definir";
                    await DisplayAlert("Error", ex.ToString(), "Ok");
                    //btnAprobacionSi.IsEnabled = false;
                    //btnAprobacionNo.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", "Algo salio, intentelo de nuevo", "Ok");
                await DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }
    }
}