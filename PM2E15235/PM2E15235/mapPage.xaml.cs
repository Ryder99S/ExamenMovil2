using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;

using PM2E15235.Models;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;

namespace PM2E15235
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mapPage : ContentPage
    {
        Double latitud;
        Double Longitud;
        public mapPage()
        {
            InitializeComponent();


        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();



            Longitud = Convert.ToDouble(txtLongitud.Text);
            latitud = Convert.ToDouble(txtLatitud.Text);

            Pin ubicacion = new Pin();
            {
                ubicacion.Label = txtdescripcioncorta.Text;
                ubicacion.Address = txtdescripcionlarga.Text;
                ubicacion.Type = PinType.Place;
                ubicacion.Position = new Position(latitud, Longitud);

            }
            Mapa.Pins.Add(ubicacion);


            var localizacion = await Geolocation.GetLastKnownLocationAsync();

            if (localizacion == null)
            {

                localizacion = await Geolocation.GetLocationAsync();
            }
            Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(ubicacion.Position, Distance.FromKilometers(1)));
        }
    }
}