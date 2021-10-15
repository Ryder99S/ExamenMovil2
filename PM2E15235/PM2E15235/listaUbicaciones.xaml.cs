using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PM2E15235.Models;
using Plugin.Geolocator;
using SQLite;

namespace PM2E15235
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class listaUbicaciones : ContentPage
    {

        private Localizacion temporal = new Localizacion();
        public listaUbicaciones()
        {
            InitializeComponent();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var listaUbicaciones = await App.BaseDatos.ObtenerListaLocalizacion();

            listUbicaciones.ItemsSource = listaUbicaciones;
        }
        private async void refrescar()
        {
            var listUbicate = await App.BaseDatos.ObtenerListaLocalizacion();

            if (listUbicate != null)
            {
                listUbicaciones.ItemsSource = listUbicate;
            }
        }
        private async void listUbicaciones_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var objetoSeleccionado = (Localizacion)e.SelectedItem;
            btnmapa.IsVisible = true;
            btneliminar.IsVisible = true;

            var idObjetoSeleccionado = objetoSeleccionado.id;


            if (objetoSeleccionado.id != 0)
            {
                var objetoExtraidoBasedeDatos = await App.BaseDatos.ObtenerLocalizacion(idObjetoSeleccionado);
                if (objetoExtraidoBasedeDatos != null)
                {
                    temporal.id = objetoExtraidoBasedeDatos.id;
                    temporal.latitud = objetoExtraidoBasedeDatos.latitud;
                    temporal.longitud = objetoExtraidoBasedeDatos.longitud;
                    temporal.descripcionLarga = objetoExtraidoBasedeDatos.descripcionLarga;
                    temporal.descripcionCorta = objetoExtraidoBasedeDatos.descripcionCorta;
                }
            }
        }

        private async void btneliminar_Clicked(object sender, EventArgs e)
        {
            var obtenerLocalizacion = await App.BaseDatos.ObtenerLocalizacion(temporal.id);
            if (obtenerLocalizacion != null)
            {
                await App.BaseDatos.EliminarLocalizacion(obtenerLocalizacion);

                await DisplayAlert("Eliminacion", "El registro se elimino correctamente", "OK");

                btneliminar.IsVisible = false;
                btnmapa.IsVisible = false;
                refrescar();
            }
        }

        private async void mensaje()
        {
            await DisplayAlert("Sin Seleccion", "Por Favor Seleccione un Dato", "OK");
        }

        private async void Ver()
        {

            var mapa = new
            {
                id = temporal.id,
                latitud = temporal.latitud,
                longitud = temporal.longitud,
                descripcionCorta = temporal.descripcionCorta,
                descripcionLarga = temporal.descripcionLarga
            };

            //await DisplayAlert("Datos a Enviar> " + seleccinarId.Id + " " + seleccinarId.DescripcionCorta, " Ubicacion Larga> " + seleccinarId.DescripcionLarga + " Coordenadas >> " + seleccinarId.Latitud + " " + seleccinarId.Longitud, "OK");

            var Page = new mapPage();
            Page.BindingContext = mapa;
            await Navigation.PushAsync(Page);
            temporal = null;

        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Accion", "¿Desea ir a la Ubicacion indicada?", "Yes", "No");
                if (result) Ver();

            });


            return true;
        }



        private async void btnmapa_Clicked(object sender, EventArgs e)
        {
            var Gps = CrossGeolocator.Current;
            if (Gps.IsGeolocationEnabled)//Servicio de Geolocalizacion existente
            {


                if (Gps.IsGeolocationEnabled)//VALIDA QUE EL GPS ESTA ENCENDIDO
                {

                    var conexion = await App.BaseDatos.ObtenerListaLocalizacion();
                    {
                        if (temporal != null)
                        {
                            OnBackButtonPressed();

                        }

                        else
                            mensaje();
                    }
                }

            }
            else
            {
                await DisplayAlert("GPS Apagado", "Para ver la Ubicacion de manera Correcta. Por favor, Active el GPS/ Ubicacion", "OK");
            }
        }
    }
}