using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.Controls
{
    public partial class CustomPin : ContentView
    {
        public CustomPin()
        {
            InitializeComponent();

            
        }

        public static BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(CustomPin)/*, propertyChanged: IconVisible*/);

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(CustomPin));

        public bool IsVisible
        {
            get { return (bool)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void IconVisible(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomPin)bindable;

            view.IsVisible = (view.Icon != null);
        }

    }
}