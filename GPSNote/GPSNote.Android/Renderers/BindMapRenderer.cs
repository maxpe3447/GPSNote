using Android.Content;
using Android.Gms.Maps;
using Android.Graphics;
using Android.Graphics.Drawables;
using Droid.Renderers;
using GPSNote;
using GPSNote.Controls;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms.Platform.Android;
using static Android.Gms.Maps.GoogleMap;

[assembly: ExportRenderer(typeof(BindMap), typeof(BindMapRenderer))]
namespace Droid.Renderers 
{
    public class BindMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        public BindMapRenderer(Context context) : base(context)
        {
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.MyLocationEnabled = true;
            googleMap.UiSettings.MyLocationButtonEnabled = true;
        }

        

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.GoogleMaps.Map> e)
        {
            base.OnElementChanged(e);

            
            if (Control != null)
            {
                

            }

            // Configure Entry properties
            if (e.NewElement != null)
            {
                //e.NewElement.la
            }
        }
       
        
    }
}
