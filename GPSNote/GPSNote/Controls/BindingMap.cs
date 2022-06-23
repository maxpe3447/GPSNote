using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using GPSNote.Models;
using Xamarin.Forms.GoogleMaps;
using GPSNote.Extansion;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Input;
using GPSNote.Resources;

namespace GPSNote.Controls
{
    public class BindingMap : Map
    {
        [Obsolete]
        public BindingMap() : base()
        {
            MapClicked += OnMapClicked;
            PinClicked += OnPinClicked;
            CameraChanged += OnCameraChanged;

            UiSettings.ZoomControlsEnabled = false;
        }

        #region  -- Public Properties -- 
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
            set => SetValue(ClickPositionProperty, value);
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
        #endregion

        #region  -- Private --  

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
                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert(UserMsg.ErrorGoingToArea);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
        private static void OnPinsCollectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = bindable as BindingMap;
            map.Pins.RestPins(newValue as List<PinViewModel>);
        }

        private void OnCameraChanged(object sender, CameraChangedEventArgs e)
        {
            CameraCurPosition = new CameraPosition(e.Position.Target, e.Position.Zoom);
        }

        private void OnPinClicked(object sender, PinClickedEventArgs e)
        {
            PinClick = e.Pin;
            if (PinClickCommand?.CanExecute(null) ?? false)
            {
                PinClickCommand.Execute(null);
            }
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            ClickPosition = e.Point;
            if (MapClickCommand?.CanExecute(null) ?? false)
            {
                MapClickCommand.Execute(null);
            }
        }

        #endregion
    }
}
