using AppMecanizado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Java.Text.Normalizer;

namespace AppMecanizado.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaDeServicios : TabbedPage
    {
        Conexion.Conex CON = new Conexion.Conex();
        List<ServicioMecanizadoLista> ListaPend = new List<ServicioMecanizadoLista>();
        List<ServicioMecanizadoLista> ListaEnProc = new List<ServicioMecanizadoLista>(); 
        List<ServicioMecanizadoLista> ListaFin = new List<ServicioMecanizadoLista>();
        public ListaDeServicios()
        {
            InitializeComponent();
            txtNombrePend.Text = "Usuario: " + App._nombrePersonal + " - " + App._idPersonal.ToString();
            txtNombreEnProc.Text = "Usuario: " + App._nombrePersonal + " - " + App._idPersonal.ToString(); 
            txtNombreFin.Text = "Usuario: " + App._nombrePersonal + " - " + App._idPersonal.ToString();
            TraerData();
        }
        public async void TraerData()
        {
            int solicitante = App._idPersonal;
            try
            {
                string sentenciaPend = String.Format("select a.Codigo, a.p_Id_solicitante, a.p_Fecha, a.p_Centro_trabajo, " +
                    "CASE WHEN a.p_Prioridad = 1 THEN 'Urgente'" +
                    "WHEN a.p_Prioridad = 2 THEN 'Importante' " +
                    "WHEN a.p_Prioridad = 3 THEN 'Repuesto' " +
                    "ELSE 'Importante' END AS p_Prioridad, " +
                    "CASE WHEN a.p_Estado = 0 THEN 'Pendiente' " +
                    "WHEN a.p_Estado = 1 THEN 'En proceso' " +
                    "WHEN a.p_Estado = 2 THEN 'Parcial' " +
                    "WHEN a.p_Estado = 3 THEN 'Finalizado' " +
                    "Else 'Pendiente' END AS p_Estado, b.p_Nombre as p_Producto, a.p_Descripcion, a.p_Fecha, a.Codigo from tblServicioMecanizado a " +
                    "INNER JOIN tblCatalogoProductos b ON a.p_Producto = b.p_Id_producto where a.p_Estado = 0 AND a.p_Id_solicitante = " + solicitante);
                var dataPend = CON.ejecutarConsulta(sentenciaPend);

                for (int i = 0; i < dataPend.Rows.Count; i++)
                {
                    ServicioMecanizadoLista ListaServicioMecanizado = new ServicioMecanizadoLista();
                    ListaServicioMecanizado.Id_servicio_mecanizado = dataPend.Rows[i][0].ToString();
                    ListaServicioMecanizado.Id_solicitante = (int)dataPend.Rows[i][1];
                    ListaServicioMecanizado.Fecha = Convert.ToDateTime(dataPend.Rows[i][2]);
                    ListaServicioMecanizado.Centro_trabajo = dataPend.Rows[i][3].ToString();
                    ListaServicioMecanizado.Prioridad = dataPend.Rows[i][4].ToString();
                    ListaServicioMecanizado.Estado = dataPend.Rows[i][5].ToString();
                    ListaServicioMecanizado.Producto = dataPend.Rows[i][6].ToString();
                    ListaServicioMecanizado.Descripcion = dataPend.Rows[i][7].ToString();
                    ListaServicioMecanizado.Fecha_Finalizacion = Convert.ToDateTime(dataPend.Rows[i][8]);
                    ListaServicioMecanizado.Codigo = dataPend.Rows[i][9].ToString();
                    ListaPend.Add(ListaServicioMecanizado);
                }
                if (ListaPend.Count > 0)
                {
                    listServiciosPend.ItemsSource = ListaPend;
                }
                else
                {
                    listServiciosPend.IsVisible = false;
                    lblPend.IsVisible = true;
                }
                //
                await Task.Delay(500);
                string sentenciaEnProc = String.Format("select a.Codigo, a.p_Id_solicitante, a.p_Fecha, a.p_Centro_trabajo, " +
                    "CASE WHEN a.p_Prioridad = 1 THEN 'Urgente'" +
                    "WHEN a.p_Prioridad = 2 THEN 'Importante' " +
                    "WHEN a.p_Prioridad = 3 THEN 'Repuesto' " +
                    "ELSE 'Importante' END AS p_Prioridad, " +
                    "CASE WHEN a.p_Estado = 0 THEN 'Pendiente' " +
                    "WHEN a.p_Estado = 1 THEN 'En proceso' " +
                    "WHEN a.p_Estado = 2 THEN 'Parcial' " +
                    "WHEN a.p_Estado = 3 THEN 'Finalizado' " +
                    "Else 'Pendiente' END AS p_Estado, b.p_Nombre as p_Producto, a.p_Descripcion, a.p_Fecha_finalizacion, a.Codigo, a.p_Fecha_estimada from tblServicioMecanizado a " +
                    "INNER JOIN tblCatalogoProductos b ON a.p_Producto = b.p_Id_producto where a.p_Estado = 1 AND a.p_Id_solicitante = " + solicitante);
                var dataEnProc = CON.ejecutarConsulta(sentenciaEnProc);

                for (int i = 0; i < dataEnProc.Rows.Count; i++)
                {
                    ServicioMecanizadoLista ListaServicioMecanizadoProc = new ServicioMecanizadoLista();
                    ListaServicioMecanizadoProc.Id_servicio_mecanizado = dataEnProc.Rows[i][0].ToString();
                    ListaServicioMecanizadoProc.Id_solicitante = (int)dataEnProc.Rows[i][1];
                    ListaServicioMecanizadoProc.Fecha = Convert.ToDateTime(dataEnProc.Rows[i][2]);
                    ListaServicioMecanizadoProc.Centro_trabajo = dataEnProc.Rows[i][3].ToString();
                    ListaServicioMecanizadoProc.Prioridad = dataEnProc.Rows[i][4].ToString();
                    ListaServicioMecanizadoProc.Estado = dataEnProc.Rows[i][5].ToString();
                    ListaServicioMecanizadoProc.Producto = dataEnProc.Rows[i][6].ToString();
                    ListaServicioMecanizadoProc.Descripcion = dataEnProc.Rows[i][7].ToString();
                    ListaServicioMecanizadoProc.Fecha_Finalizacion = Convert.ToDateTime(dataEnProc.Rows[i][8]);
                    ListaServicioMecanizadoProc.Codigo = dataEnProc.Rows[i][9].ToString();
                    ListaServicioMecanizadoProc.p_Fecha_estimada = Convert.ToDateTime(dataEnProc.Rows[i][2]);
                    ListaEnProc.Add(ListaServicioMecanizadoProc);
                }
                if (ListaEnProc.Count > 0)
                {
                    listServiciosEnProc.ItemsSource = ListaEnProc;
                }
                else
                {
                    listServiciosEnProc.IsVisible = false;
                    lblEnProc.IsVisible = true;
                }
                //
                await Task.Delay(500);
                string sentenciaFin = String.Format("select a.Codigo, a.p_Id_solicitante, a.p_Fecha, a.p_Centro_trabajo, " +
                    "CASE WHEN a.p_Prioridad = 1 THEN 'Urgente'" +
                    "WHEN a.p_Prioridad = 2 THEN 'Importante' " +
                    "WHEN a.p_Prioridad = 3 THEN 'Repuesto' " +
                    "ELSE 'Importante' END AS p_Prioridad, " +
                    "CASE WHEN a.p_Estado = 0 THEN 'Pendiente' " +
                    "WHEN a.p_Estado = 1 THEN 'En proceso' " +
                    "WHEN a.p_Estado = 2 THEN 'Parcial' " +
                    "WHEN a.p_Estado = 3 THEN 'Finalizado' " +
                    "Else 'Pendiente' END AS p_Estado, b.p_Nombre as p_Producto, a.p_Descripcion, a.p_Fecha_Finalizacion, a.Codigo from tblServicioMecanizado a " +
                    "INNER JOIN tblCatalogoProductos b ON a.p_Producto = b.p_Id_producto where a.p_Estado = 3 AND a.p_Id_solicitante = " + solicitante);
                var dataFin = CON.ejecutarConsulta(sentenciaFin);
                for (int i = 0; i < dataFin.Rows.Count; i++)
                {
                    ServicioMecanizadoLista ListaServicioMecanizadoFin = new ServicioMecanizadoLista();
                    ListaServicioMecanizadoFin.Id_servicio_mecanizado = dataFin.Rows[i][0].ToString();
                    ListaServicioMecanizadoFin.Id_solicitante = (int)dataFin.Rows[i][1];
                    ListaServicioMecanizadoFin.Fecha = Convert.ToDateTime(dataFin.Rows[i][2]);
                    ListaServicioMecanizadoFin.Centro_trabajo = dataFin.Rows[i][3].ToString();
                    ListaServicioMecanizadoFin.Prioridad = dataFin.Rows[i][4].ToString();
                    ListaServicioMecanizadoFin.Estado = dataFin.Rows[i][5].ToString();
                    ListaServicioMecanizadoFin.Producto = dataFin.Rows[i][6].ToString();
                    ListaServicioMecanizadoFin.Descripcion = dataFin.Rows[i][7].ToString();
                    ListaServicioMecanizadoFin.Fecha_Finalizacion = Convert.ToDateTime(dataFin.Rows[i][8]);
                    ListaServicioMecanizadoFin.Codigo = dataFin.Rows[i][9].ToString();
                    ListaFin.Add(ListaServicioMecanizadoFin);
                }
                if (ListaFin.Count > 0)
                {
                    listServiciosFin.ItemsSource = ListaFin;
                }
                else
                {
                    listServiciosFin.IsVisible = false;
                    lblFin.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(App._actualizar == 1)
            {
                listServiciosPend.ItemsSource = null;
                ListaPend.Clear(); 
                listServiciosEnProc.ItemsSource = null;
                ListaEnProc.Clear();
                listServiciosFin.ItemsSource= null;
                ListaFin.Clear();
                TraerData();
                App._actualizar = 0;
            }
        }
        private async void toolbarPend_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SolicitudServicio());
        }
        private async void toolbarEnProc_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SolicitudServicio());
        }
        private async void toolbarFin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SolicitudServicio());
        }
        private async void listServiciosPend_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as ServicioMecanizadoLista;
            await Navigation.PushAsync(new Seguimiento(detalles.Codigo, detalles.Fecha, detalles.Centro_trabajo, detalles.Producto, detalles.Descripcion, detalles.Estado));
        }
        private async void listServiciosEnProc_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as ServicioMecanizadoLista;
            await Navigation.PushAsync(new Seguimiento(detalles.Codigo, detalles.Fecha, detalles.Centro_trabajo, detalles.Producto, detalles.Descripcion, detalles.Estado));
        }
        private async void listServiciosFin_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as ServicioMecanizadoLista;
            await Navigation.PushAsync(new Seguimiento(detalles.Codigo, detalles.Fecha, detalles.Centro_trabajo, detalles.Producto, detalles.Descripcion, detalles.Estado));
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
        private void tbPendCerrar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alerta", "¿Quiere cerrar sesion?", "Si", "No");
                if (result) await this.Navigation.PushAsync(new Login());
            });
        }
        private void tbProcCerrar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alerta", "¿Quiere cerrar sesion?", "Si", "No");
                if (result) await this.Navigation.PushAsync(new Login());
            });
        }
        private void tbFinCerrar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alerta", "¿Quiere cerrar sesion?", "Si", "No");
                if (result) await this.Navigation.PushAsync(new Login());
            });
        }
    }
}