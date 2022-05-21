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
using System.Windows.Input;

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
                if (MapClickCommand?.CanExecute(null) ?? false)
                {
                    MapClickCommand.Execute(null);
                }
            };

            PinClicked += (s, e) =>
            {
                PinClick = e.Pin;
                if (PinClickCommand?.CanExecute(null) ?? false)
                {
                    PinClickCommand.Execute(null);
                }
            };
            CameraChanged += (s, e) => CameraCurPosition = new CameraPosition(e.Position.Target, e.Position.Zoom);
 
            UiSettings.ZoomControlsEnabled = false;
        }
        public static readonly BindableProperty CameraCurPositionProperty =
            BindableProperty.Create(
            nameof(CameraCurPosition),
            typeof(CameraPosition),
            typeof(BindingMap),
            defaultValue: default(CameraPosition),
            defaultBindingMode: BindingMode.TwoWay);

        public CameraPosition CameraCurPosition
        {
            get => (CameraPosition)GetValue(CameraCurPositionProperty);
            set => SetValue(CameraCurPositionProperty, value);
        }

        public static readonly BindableProperty MyLocationButtonEnabledProperty =
            BindableProperty.Create(
            nameof(MyLocationButtonEnabled),
            typeof(bool),
            typeof(BindingMap),
            defaultValue: default(bool),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (b, oldV, newV) => { ((BindingMap)b).UiSettings
                                                                 .MyLocationButtonEnabled = (bool)newV; });

        public bool MyLocationButtonEnabled
        {
            get => (bool)GetValue(MyLocationButtonEnabledProperty);
            set => SetValue(MyLocationButtonEnabledProperty, value);
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

            if (newValue is Position pos && pos != default(Position))
            {
                try
                {
                    Distance distance = new MapSpan(pos, 0.01, 0.01).Radius;
                MapSpan region = MapSpan.FromCenterAndRadius(pos, distance);
                map.MoveToRegion(region);
                }catch(Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message);
                }

            }

        }
        public static BindableProperty PinsCollectionProperty =
            BindableProperty.Create(
            nameof(PinsCollection),
            typeof(List<PinViewModel>),
            typeof(BindingMap),
            default(List<PinViewModel>),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnPinsCollectionChanged);

        public List<PinViewModel> PinsCollection 
        {
            get { return (List<PinViewModel>)GetValue(PinsCollectionProperty); }
            set { SetValue(PinsCollectionProperty, value); }
        }

        private static void OnPinsCollectionChanged(BindableObject bindable, object oldValue, object newValue)
        {

            var map = bindable as BindingMap;
            map.Pins.RestPins(newValue as List<PinViewModel>);
            // if(oldValue is ObservableCollection<Pin> old)
            //{
            //    old.CollectionChanged -= map.OnObsPinsCollectionChanged;
            //}
            //if (newValue is ObservableCollection<Pin> new_)
            //{
            //    new_.CollectionChanged += map.OnObsPinsCollectionChanged;
            //}

        }

        //private void OnObsPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Reset)
        //    {
        //        Pins.Clear();
        //    }
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {

        //        Pins.Add(e.NewItems[0] as Pin);
        //    }

        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        Pins.Remove(e.OldItems[0] as Pin);
        //    }
        //}

        public static BindableProperty PinClickCommandProperty =
            BindableProperty.Create(
            nameof(PinClickCommand),
            typeof(ICommand),
            typeof(BindingMap),
            default(ICommand),
            defaultBindingMode: BindingMode.TwoWay);

        public ICommand PinClickCommand
        {
            get { return (ICommand)GetValue(PinClickCommandProperty); }
            set { SetValue(PinClickCommandProperty, value); }
        }

        public static BindableProperty PinClickProperty =
            BindableProperty.Create(
            nameof(PinClick),
            typeof(Pin),
            typeof(BindingMap),
            default(Pin),
            defaultBindingMode: BindingMode.TwoWay);

        public Pin PinClick
        {
            get { return (Pin)GetValue(PinClickProperty); }
            set { SetValue(PinClickProperty, value); }
        }

        public static BindableProperty MapClickCommandProperty =
            BindableProperty.Create(
            nameof(MapClickCommand),
            typeof(ICommand),
            typeof(BindingMap),
            default(ICommand),
            defaultBindingMode: BindingMode.TwoWay);

        public ICommand MapClickCommand
        {
            get { return (ICommand)GetValue(MapClickCommandProperty); }
            set { SetValue(MapClickCommandProperty, value); }
        }
    }
}
