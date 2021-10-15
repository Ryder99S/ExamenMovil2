using SQLite;
using PM2E15235 .Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using System;

namespace PM2E15235
{
    public partial class MainPage : ContentPage
    {
        double numeroLatitud;
        double numeroLongitud;
        public MainPage()
        {
            InitializeComponent();

            var Localizacion = CrossGeolocator.Current;
            if (Localizacion.IsGeolocationEnabled)//Servicio de Geolocalizacion existente
            {

            }
            else
            {
                DisplayAlert("Permisos", "De Acceso a su ubicacion", "OK");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Localizacion();
        }

        private async void nuevaUbicacion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void ubicacionesSalvadas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new listaUbicaciones());
        }

        private async void Localizacion()
        {
            var localizacion = CrossGeolocator.Current;
            localizacion.DesiredAccuracy = 50;

            if (localizacion.IsGeolocationAvailable)
            {
                if (localizacion.IsGeolocationEnabled)
                {
                    if (!localizacion.IsListening)
                    {
                        await localizacion.StartListeningAsync(TimeSpan.FromSeconds(1), 5);
                    }
                    localizacion.PositionChanged += (cambio, args) =>
                    {
                        var localiza = args.Position;
                        txtLatitud.Text = localiza.Latitude.ToString();
                        numeroLatitud = double.Parse(txtLatitud.Text);
                        txtLongitud.Text = localiza.Longitude.ToString();
                        numeroLongitud = double.Parse(txtLongitud.Text);
                    };
                }
            }
        }

        private async    void btnGuardar_Clicked(object sender, EventArgs e)
        {
            //validamos que los campos no esten vacios
            if (String.IsNullOrEmpty(txtLongitud.Text) && String.IsNullOrEmpty(txtLatitud.Text))
            {
                await DisplayAlert("Sin Datos", "Para Obtener la Lactitud y Longitud presionar <<Nueva Ubicacion>> ", "Ok");
            }
            else if (String.IsNullOrEmpty(txtUbicacion.Text))
            {
                await DisplayAlert("Campo Vacio", "Por favor, Ingrese una Descripcion de la Ubicacion ", "Ok");
            }
            else if (String.IsNullOrEmpty(txtCorta.Text))
            {
                await DisplayAlert("Campo Vacio", "Por favor, Ingrese una Descripcion Corta, Por favor ", "Ok");
            }
            else
            {
                //NOS PREPARAMOS PARA GUARDAR

                var ubicaciones = new Models.Localizacion
                {
                    latitud = Convert.ToDouble(txtLatitud.Text),
                    longitud = Convert.ToDouble(txtLongitud.Text),
                    descripcionLarga = txtUbicacion.Text,
                    descripcionCorta = txtCorta.Text
                };


                var resultado = await App.BaseDatos.GrabarLocalizacion(ubicaciones);

                if (resultado == 1)
                {
                    await DisplayAlert("Agregado", "Ingresado Exitosamente", "OK");
                    txtUbicacion.Text = "";
                    txtCorta.Text = "";
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo guardar", "OK");
                }
            }
        }

        private async void btnUbicaciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new listaUbicaciones());
        }
    }
   
}
