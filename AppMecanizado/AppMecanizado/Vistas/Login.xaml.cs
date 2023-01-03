using AppMecanizado.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMecanizado.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        Conexion.Conex CON = new Conexion.Conex();
        public Login()
        {
            InitializeComponent();
        }
        private async void btnIngresar_Clicked(object sender, EventArgs e)
        {
            indicator_login.IsRunning = true;
            await Task.Delay(800);
            if (CrossConnectivity.Current.IsConnected)
            {
                if (!string.IsNullOrWhiteSpace(entryUsuario.Text) || (!string.IsNullOrEmpty(entryUsuario.Text)))
                {
                    if (!string.IsNullOrWhiteSpace(entryPassword.Text) || (!string.IsNullOrEmpty(entryPassword.Text)))
                    {
                        try
                        {
                            string user = entryUsuario.Text;
                            string pass = entryPassword.Text;
                            string sentencia = String.Format("select EmpleadoId from empleados.dbo.tbluser where Login='{0}' and Clave='{1}'", user, pass);
                            int[] IdEmpleado = null;
                            var data = CON.ejecutarConsultaLogin(sentencia);
                            foreach (DataRow item in data.Rows)
                            {
                                IdEmpleado = new int[] { (int)item[0] };
                            }

                            if (IdEmpleado != null)
                            {
                                App._idPersonal = IdEmpleado[0];
                                App._nombrePersonal = entryUsuario.Text;
                                //await Navigation.PushAsync(new ListaServicios());
                                await Navigation.PushAsync(new ListaDeServicios());
                                Navigation.RemovePage(this);
                                indicator_login.IsRunning = false;
                            }
                            else
                            {
                                await DisplayAlert("Error", "Usuario no encontrado", "Ok");
                                indicator_login.IsRunning = false;
                            }
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine("################=== " + err.ToString());
                            //await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                            await DisplayAlert("Error", err.ToString(), "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "El campo de 'Password' esta vacio", "Ok");
                        indicator_login.IsRunning = false;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "El campo de 'Contraseña' esta vacio", "Ok");
                    indicator_login.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
                indicator_login.IsRunning = false;
            }
        }
    }
}