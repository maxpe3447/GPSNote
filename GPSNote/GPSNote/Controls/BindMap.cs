using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;
//using Xamarin.Forms.Maps;
using GPSNote.Models;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Controls
{
    public class BindMap : Map
    {
        [Obsolete]
        public BindMap(/*MapSpan region*/) : base(/*region*/)
        {
            MapClicked += (s, e) =>
            {
               
                ClickPosition = e.Point;
            };
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition( new Position(47.824734, 35.1625), 13 ));
            
        }
        
        public static readonly BindableProperty MyLocationButtonEnabledProperty =
            BindableProperty.Create(
            nameof(MyLocationButtonEnabled),
            typeof(bool),
            typeof(BindMap),
            defaultValue: default(bool),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (b, oldV, newV) => { ((BindMap)b).UiSettings.MyLocationButtonEnabled = (bool)newV; });

        public bool MyLocationButtonEnabled
        {
            get => (bool)GetValue(MyLocationButtonEnabledProperty);
            set
            {

                SetValue(MyLocationButtonEnabledProperty, value);
            }
        }

        public static readonly BindableProperty ClickPositionProperty =
            BindableProperty.Create(
            nameof(ClickPosition),
            typeof(Position),
            typeof(BindMap),
            defaultValue: default(Position),
            defaultBindingMode: BindingMode.TwoWay);

        public Position ClickPosition
        {
            get => (Position)GetValue(ClickPositionProperty);
            set
            {
                
                SetValue(ClickPositionProperty, value);
            }
        }

        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create(
            nameof(SelectedItem),
            typeof(Pin),
            typeof(BindMap),
            null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged
);
        public Pin SelectedItem
        {
            get { return (Pin)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = (BindMap)bindable;
            var pin = newValue as Pin;
            if (pin != null)
            {
                Distance distance = map.VisibleRegion?.Radius ?? new MapSpan(pin.Position, 0.1, 0.1).Radius;
                MapSpan region = MapSpan.FromCenterAndRadius(pin.Position, distance);
                map.MoveToRegion(region);

                //pin.Icon = BitmapDescriptorFactory.FromBundle("ic_pin.png");//ImageSource.FromFile().;
                map.Pins.Add(pin);
                
            }

        }
    }
}
