using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using AppMecanizado.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using AppMecanizado.Conexion;
using System.IO;
using Android.Graphics;
using Xamarin.Essentials;
using System.Data;

namespace AppMecanizado.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SolicitudServicio : ContentPage
    {
        Conexion.Conex CON = new Conexion.Conex();
        string _centroDeTrabajo;
        int _prioridad;
        int _idProducto;
        string _producto;
        string _muestras;
        byte[] imageMuestraArray = null;
        byte[] imagePlanoArray = null;
        Bitmap _imagePlano;
        Bitmap _imgMuestra;
        private MediaFile _mediaFile;
        private Image _muestraIMG;
        private string rutaMuestra;
        string _planos;
        private MediaFile _mediaFilePlano;
        private Image _planoIMG;
        private string rutaPlano;
        List<CatalogoProducto> ListaProductos = new List<CatalogoProducto>();
        List<String> ListaCentros = new List<String>();
        Conex _conexion = new Conex();
        string _correlativoBD = string.Empty;
        string _correlativoNuevo = "";
        int correlativoInt = 0;
        string _idMec = string.Empty;

        public SolicitudServicio()
        {
            InitializeComponent();
            txtSolicitante.Text = App._nombrePersonal;
            GetCentros();
            pickerCentroTrabajo.ItemsSource = ListaCentros;
            GetProductos();
            pickerProducto.ItemsSource = ListaProductos;
            pickFecha.Date = DateTime.Now;
            GetCorrelativo();
        }
        private void GetCentros()
        {
            try
            {
                string sentencia = String.Format("select p_Nombre from tblCentroTrabajoMec");
                var data = CON.ejecutarConsulta(sentencia);
                foreach (DataRow item in data.Rows)
                {
                    ListaCentros.Add(item[0].ToString());
                }
            }
            catch(Exception err)
            {
                Console.WriteLine("################# = " + err.ToString());
            }
        }
        private void GetProductos()
        {
            try
            {
                string sentencia = String.Format("select * from tblCatalogoProducto");
                var data = CON.ejecutarConsulta(sentencia);
                foreach (DataRow item in data.Rows)
                {
                    ListaProductos.Add(new CatalogoProducto { p_Id_producto = Convert.ToInt32(item[0]), p_Nombre = item[1].ToString() });
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("################# = " + err.ToString());
            }
            
        }
        private async void pickerCentroTrabajo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex != -1)
                {
                    _centroDeTrabajo = picker.Items[selectedIndex];
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                Console.WriteLine("################ = " + err.ToString());
            }
        }
        private void checkUrgente_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(checkUrgente.IsChecked == true)
            {
                checkImportante.IsChecked = false;
                checkRepuesto.IsChecked = false;
                _prioridad = 1;
            }
        }
        private void checkImportante_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(checkImportante.IsChecked == true)
            {
                checkUrgente.IsChecked = false;
                checkRepuesto.IsChecked = false;
                _prioridad = 2;
            }
        }
        private void checkRepuesto_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(checkRepuesto.IsChecked == true)
            {
                checkUrgente.IsChecked = false;
                checkImportante.IsChecked = false;
                _prioridad = 3;
            }
        }
        private async void pickerProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex != -1)
                {
                    _producto = picker.Items[selectedIndex];
                }
                try
                {
                    foreach (var item in ListaProductos)
                    {
                        if (_producto == item.p_Nombre)
                        {
                            _idProducto = item.p_Id_producto;
                        }
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine("################ = " + err.ToString());
                    await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");

                }
            }
            catch (Exception err)
            {
                Console.WriteLine("################ = " + err.ToString());
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
            }
        }
        async void GetCorrelativo()
        {
            try
            {
                string _cadena = string.Empty;
                string sentenciaCor = String.Format("select top 1 Codigo from tblServicioMecanizado where LEN(codigo) < 13 order by Correlativo desc");
                var dataCor = CON.ejecutarConsulta(sentenciaCor);
                
                //DisplayAlert("Contador query", dataCor.Rows.Count.ToString(), "Ok");
                foreach (DataRow item in dataCor.Rows)
                {
                    _correlativoBD = item[0].ToString();
                }
                char _delimitador = '-';
                string[] _valores = _correlativoBD.Split(_delimitador);
                int _valSec = Convert.ToInt32(_valores[2]);
                int cod_sec = _valSec + 1;
                correlativoInt = cod_sec;
                string _year = DateTime.Now.ToString("yyyy").ToUpper();
                string _anho = DateTime.Now.ToString("dd").ToUpper();
                string _mes = DateTime.Now.ToString("MM").ToUpper();
                string _dia = DateTime.Now.ToString("dd").ToUpper();
                _correlativoNuevo = "MEC-" + _year + "-" + cod_sec;
                Random rnd = new Random();
                int value = rnd.Next(001, 999);
                _idMec = _anho + _mes + _dia + value;
            }
            catch(Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo nuevamente", "Ok");
                Console.WriteLine("################ = " + err.ToString());
            }
        }
        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            
            indicator_login.IsRunning = true;
            await Task.Delay(500);
            try
            {
                if (!string.IsNullOrWhiteSpace(pickerCentroTrabajo.SelectedItem.ToString()) || (!string.IsNullOrEmpty(pickerCentroTrabajo.SelectedItem.ToString())))
                {
                    if (_prioridad != 0)
                    {
                        if (!string.IsNullOrWhiteSpace(_producto) || (!string.IsNullOrEmpty(_producto)))
                        {
                            if (!string.IsNullOrWhiteSpace(entryDescripcion.Text) || (!string.IsNullOrEmpty(entryDescripcion.Text)))
                            {
                                if (!string.IsNullOrWhiteSpace(entryCantidad.Text) || (!string.IsNullOrEmpty(entryCantidad.Text)))
                                {
                                    if (string.IsNullOrWhiteSpace(entryMaterial.Text) || (string.IsNullOrEmpty(entryMaterial.Text)))
                                    {
                                        entryMaterial.Text = "NN";
                                    }
                                    if (string.IsNullOrWhiteSpace(entryDureza.Text) || (string.IsNullOrEmpty(entryDureza.Text)))
                                    {
                                        entryDureza.Text = "NN";
                                    }
                                    if (string.IsNullOrWhiteSpace(entryOtros.Text) || (string.IsNullOrEmpty(entryOtros.Text)))
                                    {
                                        entryOtros.Text = "NN";
                                    }
                                    if (CrossConnectivity.Current.IsConnected)
                                    {
                                        try
                                        {
                                            if (string.IsNullOrWhiteSpace(entryObservacion.Text) || (string.IsNullOrEmpty(entryObservacion.Text)))
                                            {
                                                entryObservacion.Text = "NN";
                                            }
                                            if (string.IsNullOrWhiteSpace(_muestras) || (string.IsNullOrEmpty(_muestras)) && (string.IsNullOrWhiteSpace(_planos) || (string.IsNullOrEmpty(_planos))))
                                            {
                                                await DisplayAlert("Error", "Necesita añadir al menos una imagen", "Ok");
                                                indicator_login.IsRunning = false;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrWhiteSpace(_muestras) || (string.IsNullOrEmpty(_muestras)))
                                                {
                                                    string author = "Ferrotodo";
                                                    byte[] img_default = Encoding.ASCII.GetBytes(author);
                                                    imageMuestraArray = img_default;
                                                }
                                                if (string.IsNullOrWhiteSpace(_planos) || (string.IsNullOrEmpty(_planos)))
                                                {
                                                    string author = "Ferrotodo";
                                                    byte[] img_default = Encoding.ASCII.GetBytes(author);
                                                    imagePlanoArray = img_default;
                                                }
                                                if(entryDureza.Text == null)
                                                {
                                                    entryDureza.Text = "NN";
                                                }
                                                
                                                //Data
                                                if (_correlativoNuevo.Length > 1)
                                                {
                                                    ServicioMecanizado _servicioMecanizado = new ServicioMecanizado()
                                                    {
                                                        p_Id_servicio_mecanizado = Convert.ToInt32(_idMec),
                                                        p_Id_solicitante = App._idPersonal,
                                                        p_Fecha = pickFecha.Date,
                                                        p_Centro_trabajo = _centroDeTrabajo,
                                                        p_Prioridad = _prioridad,
                                                        p_Producto = _idProducto,
                                                        p_Descripcion = entryDescripcion.Text,
                                                        p_Cantidad = Convert.ToInt32(entryCantidad.Text),
                                                        p_Muestra_img = imageMuestraArray,
                                                        p_Plano_img = imagePlanoArray,
                                                        p_Material = entryMaterial.Text,
                                                        p_Dureza = entryDureza.Text,
                                                        p_Otros = entryOtros.Text,
                                                        p_Fecha_Finalizacion = DateTime.Today.AddDays(14),
                                                        p_Estado = 0,
                                                        p_Id_Empleado = 1,
                                                        p_Cantidad_pendiente = Convert.ToInt32(entryCantidad.Text), 
                                                        Codigo = _correlativoNuevo,
                                                        Correlativo = correlativoInt,
                                                        p_Liberado = 0,
                                                        p_Fecha_estimada = DateTime.Today.AddDays(14),
                                                        CodRelacionado = "NN",
                                                        Observaciones = "NN"
                                                    };
                                                    try
                                                    {
                                                        _conexion.GuardarSolicitud(_servicioMecanizado);

                                                        bool GuardarData = _conexion.GuardarSolicitud(_servicioMecanizado);
                                                        if (GuardarData == true)
                                                        {
                                                            await DisplayAlert("Agregado", "Se agrego la solicitud correctamente", "OK");
                                                            App._actualizar = 1;
                                                            indicator_login.IsRunning = false;
                                                            await Navigation.PopAsync();
                                                        }
                                                        else
                                                        {
                                                            await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
                                                            indicator_login.IsRunning = false;
                                                            await Navigation.PopAsync();
                                                        }
                                                    }
                                                    catch (Exception err)
                                                    {
                                                        await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                                                        Console.WriteLine("################ = " + err.ToString());
                                                        //await DisplayAlert("Error", err.ToString(), "OK");
                                                        indicator_login.IsRunning = false;
                                                    }
                                                }
                                                else
                                                {
                                                    await DisplayAlert("Error", "Correlativo vacio", "Ok");
                                                }
                                            }
                                        }
                                        catch (Exception err)
                                        {
                                            await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                                            Console.WriteLine("################ = " + err.ToString());
                                            //await DisplayAlert("Error", err.ToString(), "OK");
                                            indicator_login.IsRunning = false;
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
                                        indicator_login.IsRunning = false;
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Error", "El campo de 'Cantidad' esta vacio", "Ok");
                                    indicator_login.IsRunning = false;
                                }
                            }
                            else
                            {
                                await DisplayAlert("Error", "El campo de 'Descripcion' esta vacio", "Ok");
                                indicator_login.IsRunning = false;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "El campo de 'Producto' esta vacio", "Ok");
                            indicator_login.IsRunning = false;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "El campo de 'Prioridad esta sin marcar", "Ok");
                        indicator_login.IsRunning = false;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "El campo de 'Centro de trabajo' esta vacio", "Ok");
                    indicator_login.IsRunning = false;
                }
            }
            catch(Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                Console.WriteLine("################ = " + err.ToString());
                //await DisplayAlert("Error", err.ToString(), "OK");
                indicator_login.IsRunning = false;
            }
        }
        private async void btnAgregarMuestra_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_producto) || (!string.IsNullOrEmpty(_producto)))
                {
                    Random r = new Random();
                    int num_rand = r.Next();
                    var action = await DisplayActionSheet("Agregar imagenes", "Cancel", null, "SACAR FOTO", "ELEGIR DE LA GALERIA");
                    switch (action)
                    {
                        case "SACAR FOTO":
                            activityMuestra.IsRunning = true;
                            try
                            {
                                await CrossMedia.Current.Initialize();
                                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                                {
                                    await DisplayAlert("Error", "Camara no disponible", "OK");
                                    activityMuestra.IsRunning = false;
                                    return;
                                }
                                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                                {
                                    SaveToAlbum = true,
                                    PhotoSize = PhotoSize.Medium,
                                    Name = "M_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg"
                                });

                                using (MemoryStream memory = new MemoryStream())
                                {
                                    try
                                    {
                                        Stream stream = _mediaFile.GetStream();
                                        stream.CopyTo(memory);
                                        imageMuestraArray = memory.ToArray();
                                        _imgMuestra = BitmapFactory.DecodeStream(memory);
                                    }
                                    catch (Exception err)
                                    {
                                        await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                                        //await DisplayAlert("Error", err.ToString(), "OK");
                                    }
                                }
                                activityMuestra.IsRunning = true;
                                if (_mediaFile == null)
                                {
                                    activityMuestra.IsRunning = false;
                                    return;
                                }

                                imgMuestra.Source = ImageSource.FromStream(() =>
                                {
                                    return _mediaFile.GetStream();
                                });
                                //
                                rutaMuestra = "/api_/images/" + "M_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg";
                                txtMuestra.Text = "M_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg";
                                _muestras = "Muestra";
                                _muestraIMG = imgMuestra;
                                //_imgMuestra = Convert.toin imgMuestra;
                                activityMuestra.IsRunning = false;
                            }
                            catch (Exception err)
                            {
                                _muestras = "";
                                activityMuestra.IsRunning = false;
                                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                                //await DisplayAlert("Error", err.ToString(), "OK");
                            }
                            break;
                        case "ELEGIR DE LA GALERIA":
                            activityMuestra.IsRunning = true;
                            try
                            {
                                if (!CrossMedia.Current.IsPickPhotoSupported)
                                {
                                    await DisplayAlert("Error", "No se puede acceder a las imagenes", "OK");
                                    activityMuestra.IsRunning = false;
                                    return;
                                }
                                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                                if (_mediaFile == null)
                                {
                                    activityMuestra.IsRunning = false;
                                    return;
                                }
                                using (MemoryStream memory = new MemoryStream())
                                {
                                    Stream stream = _mediaFile.GetStream();
                                    stream.CopyTo(memory);
                                    imageMuestraArray = memory.ToArray();
                                    _imgMuestra = BitmapFactory.DecodeStream(memory);
                                }
                                imgMuestra.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                                string value = _mediaFile.Path.ToString();
                                char[] delimeters = new char[] { '/' };
                                String[] parts = value.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < parts.Length; i++)
                                {
                                    //txtMuestra.Text = parts[parts.Length - 1].ToString();
                                }

                                txtMuestra.Text = "M_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg";
                                rutaMuestra = "/api_/images/" + txtMuestra.Text;
                                _muestras = "Muestra";
                                activityMuestra.IsRunning = false;
                            }
                            catch (Exception err)
                            {
                                _muestras = "";
                                activityMuestra.IsRunning = false;
                                //await DisplayAlert("Error", err.ToString(), "OK");
                                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                            }
                            break;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Primero seleccione un producto", "Ok");
                }
            }
            catch(Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                //await DisplayAlert("Error", err.ToString(), "Ok");
            }
        }
        private async void btnAgregarPlano_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_producto) || (!string.IsNullOrEmpty(_producto)))
            {
                Random r = new Random();
                int num_rand = r.Next();
                var action = await DisplayActionSheet("Agregar imagenes", "Cancel", null, "SACAR FOTO", "ELEGIR DE LA GALERIA");
                switch (action)
                {
                    case "SACAR FOTO":
                        activityPlano.IsRunning = true;
                        try
                        {
                            await CrossMedia.Current.Initialize();
                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                            {
                                await DisplayAlert("Error", "Camara no disponible", "OK");
                                activityPlano.IsRunning = false;
                                return;
                            }
                            _mediaFilePlano = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                SaveToAlbum = true,
                                PhotoSize = PhotoSize.Medium,
                                Name = "P_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg"
                            });
                            using (MemoryStream memory = new MemoryStream())
                            {
                                try
                                {
                                    Stream stream = _mediaFilePlano.GetStream();
                                    stream.CopyTo(memory);
                                    imagePlanoArray = memory.ToArray();
                                    _imagePlano = BitmapFactory.DecodeStream(memory);
                                }
                                catch (Exception err)
                                {
                                    //
                                }
                            }
                            if (_mediaFilePlano == null)
                            {
                                activityPlano.IsRunning = false;
                                return;
                            }
                            activityPlano.IsRunning = true;
                            imgPlano.Source = ImageSource.FromStream(() =>
                            {
                                return _mediaFilePlano.GetStream();
                            });
                            //
                            rutaPlano = "/api_/images/" + "P_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg";
                            txtPlano.Text = "P_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg";
                            _planos = "Plano";
                            _planoIMG = imgPlano;
                            activityPlano.IsRunning = false;
                        }
                        catch (Exception err)
                        {
                            _planos = "";
                            activityPlano.IsRunning = false;
                            //await DisplayAlert("Error", err.ToString(), "OK");
                            await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                        }
                        break;
                    case "ELEGIR DE LA GALERIA":
                        activityPlano.IsRunning = true;
                        try
                        {
                            if (!CrossMedia.Current.IsPickPhotoSupported)
                            {
                                await DisplayAlert("Error", "No se puede acceder a las imagenes", "OK");
                                activityPlano.IsRunning = false;
                                return;
                            }
                            _mediaFilePlano = await CrossMedia.Current.PickPhotoAsync();
                            if (_mediaFilePlano == null)
                            {
                                activityPlano.IsRunning = false;
                                return;
                            }
                            using (MemoryStream memory = new MemoryStream())
                            {
                                Stream stream = _mediaFilePlano.GetStream();
                                stream.CopyTo(memory);
                                imagePlanoArray = memory.ToArray();
                                _imagePlano = BitmapFactory.DecodeStream(memory);
                            }
                            //
                            imgPlano.Source = ImageSource.FromStream(() => _mediaFilePlano.GetStream());
                            string value = _mediaFilePlano.Path.ToString();
                            char[] delimeters = new char[] { '/' };
                            String[] parts = value.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < parts.Length; i++)
                            {
                                //txtPlano.Text = parts[parts.Length - 1].ToString();
                            }

                            txtPlano.Text = "P_" + _producto + "_" + num_rand + "_" + pickFecha.Date.ToString("dd/MM/yyyy") + ".jpg";
                            rutaPlano = "/api_/images/" + txtPlano.Text;
                            _planos = "Plano";
                            _planoIMG = imgPlano;
                            activityPlano.IsRunning = false;
                        }
                        catch (Exception err)
                        {
                            _planos = "";
                            activityPlano.IsRunning = false;
                            //await DisplayAlert("Error", err.ToString(), "OK");
                            await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
                        }
                        break;
                }
            }
            else
            {
                await DisplayAlert("Error", "Primero seleccione un producto", "Ok");
            }
        }
    }
}