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
            MapClicked += (s, e) => ClickPosition = e.Position;
        }
        public BindMap() : base()
        {
            
        }
        public static readonly BindableProperty ClickPositionProperty = 
            BindableProperty.Create(
            nameof(ClickPosition),
            typeof(Position),
            typeof(BindMap),
            defaultValue: default(Position),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: ClickPositionChang);
        
        public Position ClickPosition
        {
            get => (Position)GetValue(ClickPositionProperty);
            set
            {
                SetValue(ClickPositionProperty, value); 
            }
        }

        private static void ClickPositionChang(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (BindMap)bindable;
            
            control.ClickPosition = (Position)newValue;
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
                Distance distance = map.VisibleRegion?.Radius ?? new MapSpan(pin.Position, 0.1, 0.1).Radius;
                MapSpan region = MapSpan.FromCenterAndRadius(pin.Position, distance);
                map.MoveToRegion(region);
            }
            
        }
    }
}
