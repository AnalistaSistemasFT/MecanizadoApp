using AppMecanizado.Vistas;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMecanizado
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new Login());
            //MainPage = new NavigationPage(new SolicitudServicio());
        }
        public static int _idPersonal = 1;
        public static string _nombrePersonal = "NN";
        public static int _actualizar = 0;
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
