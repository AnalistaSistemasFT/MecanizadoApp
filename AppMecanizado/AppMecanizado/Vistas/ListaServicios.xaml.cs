using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppMecanizado.Models;
using System.Data;

namespace AppMecanizado.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaServicios : ContentPage
    {
        Conexion.Conex CON = new Conexion.Conex();
        List<ServicioMecanizadoLista> ListaServicioMecanizados = new List<ServicioMecanizadoLista>();
        List<ServicioMecanizadoLista> ListaVacia = new List<ServicioMecanizadoLista>();
        DataTable dataS = new DataTable();
        public ListaServicios()
        {
            InitializeComponent();
            txtNombre.Text = "Usuario: " + App._nombrePersonal + " - " + App._idPersonal.ToString();
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            listServicios.ItemsSource = ListaVacia; 
            CargarLista();
        }
        public async void CargarLista()
        {
            try
            {
                ListaServicioMecanizados.Clear();
                int solicitante = App._idPersonal;
                string sentencia = String.Format("select p_Id_servicio_mecanizado, p_Id_empleado, p_Fecha, p_Fecha_estimada " +
                    "CASE WHEN p_Prioridad = 1 THEN 'Urgente' WHEN p_Prioridad = 2 THEN 'Importante' " +
                    "WHEN p_Prioridad = 3 THEN 'Repuesto' Else 'Importante' END AS Prioridad, " +
                    "CASE WHEN p_Estado = 0 THEN 'Pendiente' WHEN p_Estado = 1 THEN 'En proceso' " +
                    "WHEN p_Estado = 2 THEN 'Parcial' WHEN p_Estado = 3 THEN 'Finalizado' Else 'Pendiente' END AS Estado " +
                    "from tblServicioMecanizado where p_Id_solicitante = '{0}'", solicitante);
                var data = CON.ejecutarConsulta(sentencia);
                int count = data.Rows.Count;
            
                for (int i = 0; i < count; i++)
                {
                    ServicioMecanizadoLista ListaServicioMecanizado = new ServicioMecanizadoLista();
                    ListaServicioMecanizado.Id_servicio_mecanizado = data.Rows[i][0].ToString();
                    ListaServicioMecanizado.Id_solicitante = (int)data.Rows[i][1];
                    ListaServicioMecanizado.Fecha = (DateTime)data.Rows[i][2];
                    ListaServicioMecanizado.Prioridad = data.Rows[i][3].ToString();
                    ListaServicioMecanizado.Estado = data.Rows[i][4].ToString(); ;

                    ListaServicioMecanizados.Add(ListaServicioMecanizado);
                }
                
                listServicios.ItemsSource = ListaServicioMecanizados;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
            }
        }
        private async void listServicios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                //var detalles = e.Item as ServicioMecanizadoLista;
                //await Navigation.PushAsync(new Seguimiento(detalles.Id_servicio_mecanizado, detalles.Id_empleado));
            }
            catch (Exception err)
            {
                //await DisplayAlert("Error", err.ToString(), "Ok");
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
            }
        }
        private async void toolbar_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new SolicitudServicio());
            }
            catch(Exception err)
            {
                //await DisplayAlert("Error", err.ToString(), "Ok");
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alerta", "¿Quiere cerrar sesion?", "Si", "No");
                if (result) await this.Navigation.PushAsync(new Login());
            });
            return true;
        }
        private void tbCerrar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alerta", "¿Quiere cerrar sesion?", "Si", "No");
                if (result) await this.Navigation.PushAsync(new Login());
            });
        }
        private void tbActualizar_Clicked(object sender, EventArgs e)
        {
            listServicios.ItemsSource = ListaVacia;
            CargarLista();
        }
    }
}