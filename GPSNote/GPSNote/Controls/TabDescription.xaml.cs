using GPSNote.Enums;
using GPSNote.Models.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.Controls
{
    public partial class TabDescription : ContentView
    {
        public TabDescription()
        {
            InitializeComponent();
            grid.HeightRequest = 0;
        }

        public static new readonly BindableProperty HeightProperty =
            BindableProperty.Create(nameof(Height),
                                    typeof(double),
                                    typeof(TabDescription),
                                    default(double),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnHeightChanged);

        public new double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        private static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabDesc = (TabDescription)bindable;
             tabDesc.grid.HeightRequest= (double)newValue;
            
        }
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name),
                                    typeof(string),
                                    typeof(TabDescription),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnNameChanged);

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        private static void OnNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabDesc = (TabDescription)bindable;
            tabDesc.lName.Text = newValue.ToString();

        }

        public static readonly BindableProperty CoordinateProperty =
            BindableProperty.Create(nameof(Coordinate),
                                    typeof(string),
                                    typeof(TabDescription),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnCoordinateChanged);

        public string Coordinate
        {
            get { return (string)GetValue(CoordinateProperty); }
            set { SetValue(CoordinateProperty, value); }
        }

        private static void OnCoordinateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabDesc = (TabDescription)bindable;
            tabDesc.lCoordinate.Text = newValue.ToString();
        }
        public static readonly BindableProperty DescriptionProperty =
            BindableProperty.Create(nameof(Description),
                                    typeof(string),
                                    typeof(TabDescription),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnDescriptionChanged);

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        private static void OnDescriptionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabDesc = (TabDescription)bindable;
            tabDesc.lDescriptions.Text = newValue.ToString();
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily),
                                    typeof(string),
                                    typeof(TabDescription),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnFontFamilyChanged);

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        private static void OnFontFamilyChanged(BindableObject bindable, 
                                                object oldValue, 
                                                object newValue)
        {
            var desc = (TabDescription)bindable;
            desc.lCoordinate.FontFamily = 
                desc.lCoordinate.FontFamily = 
                desc.lName.FontFamily = newValue.ToString();
        }

        public static readonly BindableProperty ShareCommandProperty =
            BindableProperty.Create(nameof(ShareCommand),
                                    typeof(ICommand),
                                    typeof(TabDescription),
                                    defaultValue: default(Command),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ShareCommandChanged);
        public ICommand ShareCommand
        {
            get => (ICommand)base.GetValue(ShareCommandProperty);
            set => base.SetValue(ShareCommandProperty, value);
        }

        private static void ShareCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabDescription)bindable;

            control.bShare.Command = (ICommand)newValue;
        }

        public static readonly BindableProperty WeatherProperty =
            BindableProperty.Create(nameof(FontFamily),
                                    typeof(WeatherModel),
                                    typeof(TabDescription),
                                    default(WeatherModel),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnWeatherChanged);

        public WeatherModel Weather
        {
            get { return (WeatherModel)GetValue(WeatherProperty); }
            set { SetValue(WeatherProperty, value); }
        }
        private static void OnWeatherChanged(BindableObject bindable,
                                                object oldValue,
                                                object newValue)
        {
            var desc = (TabDescription)bindable;

            const byte START_CUT = 0;
            const byte END_CUT= 3;

            desc.lFirstDayName.Text  = DateTime.Now.DayOfWeek.ToString().Substring(START_CUT, END_CUT);
            desc.lSecondDayName.Text = DateTime.Now.AddDays((byte)EDayType.SECOND_DAY).DayOfWeek.ToString().Substring(START_CUT, END_CUT);
            desc.lThirdDayName.Text  = DateTime.Now.AddDays((byte)EDayType.THIRD_DAY).DayOfWeek.ToString().Substring(START_CUT, END_CUT);
            desc.lFourDayName.Text   = DateTime.Now.AddDays((byte)EDayType.FOUR_DAY).DayOfWeek.ToString().Substring(START_CUT, END_CUT);

            if (newValue is WeatherModel weather)
            {
                var firstDayInfo = weather.List[(byte)EDayType.FIRST_DAY];
                var secondDayInfo = weather.List[(byte)EDayType.SECOND_DAY];
                var thirdDayInfo = weather.List[(byte)EDayType.THIRD_DAY];
                var fourthDayInfo = weather.List[(byte)EDayType.FOUR_DAY];

                desc.iFirstDay.Source  = ImageSource.FromFile("_" + firstDayInfo.weather.First().Icon);
                desc.iSecondDay.Source = ImageSource.FromFile("_" + secondDayInfo.weather.First().Icon);
                desc.iThirdDay.Source  = ImageSource.FromFile("_" + thirdDayInfo.weather.First().Icon);
                desc.iFourdDay.Source  = ImageSource.FromFile("_" + fourthDayInfo.weather.First().Icon);

                desc.lFirstDayTemp.Text  = $"{(byte)firstDayInfo.Main.Temp_max}° {(byte)firstDayInfo.Main.Temp_min}°";
                desc.lSecondDayTemp.Text = $"{(byte)secondDayInfo.Main.Temp_max}° {(byte)secondDayInfo.Main.Temp_min}°";
                desc.lThirdDayTemp.Text  = $"{(byte)thirdDayInfo.Main.Temp_max}° {(byte)thirdDayInfo.Main.Temp_min}°";
                desc.lFourDayTemp.Text   = $"{(byte)fourthDayInfo.Main.Temp_max}° {(byte)fourthDayInfo.Main.Temp_min}°";
            }
        }
    }
}