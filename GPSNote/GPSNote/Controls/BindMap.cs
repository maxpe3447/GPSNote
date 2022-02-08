using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using GPSNote.Models;

namespace GPSNote.Controls
{
    public class BindMap : Map
    {
        public BindMap(MapSpan region) : base(region)
        {
            MapClicked += (s, e) => TestPosition = e.Position;
        }
        public BindMap() : base()
        {
            
        }
        public static readonly BindableProperty TestPositionProperty = 
            BindableProperty.Create(
            nameof(TestPosition),
            typeof(Position),
            typeof(BindMap),
            defaultValue: default(Position),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TestPosChang);
        
        public Position TestPosition
        {
            get => (Position)GetValue(TestPositionProperty);
            set
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(Pins.Count.ToString());
                SetValue(TestPositionProperty, value); 
            }
        }

        private static void TestPosChang(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (BindMap)bindable;
            
            control.TestPosition = (Position)newValue;
        }

        //////////////////////////////////////
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
                Distance distance = map.VisibleRegion.Radius;
                MapSpan region = MapSpan.FromCenterAndRadius(pin.Position, distance);
                map.MoveToRegion(region);
            }
        }
    }
}
