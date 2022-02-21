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
    }
}