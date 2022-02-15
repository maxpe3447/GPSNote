using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;
//using Xamarin.Forms.Maps;
using GPSNote.Models;
using Xamarin.Forms.GoogleMaps;
using GPSNote.Extansion;
using System.Collections.Specialized;
using System.Collections;

namespace GPSNote.Controls
{
    public class BindingMap : Map
    {
        [Obsolete]
        public BindingMap() : base()
        {
            MapClicked += (s, e) =>
            {
               
                ClickPosition = e.Point;
            };
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition( new Position(47.824734, 35.1625), 13 ));
            
            UiSettings.ZoomControlsEnabled = false;
        }
        
        public static readonly BindableProperty MyLocationButtonEnabledProperty =
            BindableProperty.Create(
            nameof(MyLocationButtonEnabled),
            typeof(bool),
            typeof(BindingMap),
            defaultValue: default(bool),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (b, oldV, newV) => { ((BindingMap)b).UiSettings.MyLocationButtonEnabled = (bool)newV; });

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
            typeof(BindingMap),
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

        public static BindableProperty GoToPositionProperty =
            BindableProperty.Create(
            nameof(GoToPosition),
            typeof(Position),
            typeof(BindingMap),
            default(Position),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnGoToPositionChanged);

        public Position GoToPosition
        {
            get { return (Position)GetValue(GoToPositionProperty); }
            set { SetValue(GoToPositionProperty, value); }
        }

        private static void OnGoToPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = (BindingMap)bindable;

            if (newValue is Position pos && pos != null)
            {
                Distance distance = new MapSpan(pos, 0.01, 0.01).Radius;
                MapSpan region = MapSpan.FromCenterAndRadius(pos, distance);
                map.MoveToRegion(region);

            }

        }
        public static BindableProperty PinsCollectionProperty =
            BindableProperty.Create(
            nameof(PinsCollection),
            typeof(ObservableCollection<Pin>),
            typeof(BindingMap),
            default(ObservableCollection<Pin>),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnPinsCollectionChanged);

        public ObservableCollection<Pin> PinsCollection 
        {
            get { return (ObservableCollection<Pin>)GetValue(PinsCollectionProperty); }
            set { SetValue(PinsCollectionProperty, value); }
        }

        private static void OnPinsCollectionChanged(BindableObject bindable, object oldValue, object newValue)
        {

            var map = bindable as BindingMap;
             if(oldValue is ObservableCollection<Pin> old)
            {
                old.CollectionChanged -= map.OnObsPinsCollectionChanged;
            }
            if (newValue is ObservableCollection<Pin> new_)
            {
                new_.CollectionChanged += map.OnObsPinsCollectionChanged;
            }

        }

        private void OnObsPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Pins.Clear();
            }
            if (e.NewItems != null && e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems is IList)
                {
                    Pins.Add(((ObservableCollection<Pin>)sender).Last());
                }
            }
        }
    }
}
