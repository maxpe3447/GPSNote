using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordLine : ContentView
    {
        public PasswordLine()
        {
            InitializeComponent();
            bIsShowPass.Clicked += (s, e) =>
              {
                  eText.IsPassword = !eText.IsPassword;

                  if (eText.IsPassword)
                  {
                      bIsShowPass.ImageSource = ImageSource.FromFile("ic_eye.png");
                  }
                  else
                  {
                      bIsShowPass.ImageSource = ImageSource.FromFile("ic_eye_off.png");
                  }
              };

            eText.TextChanged += (s, e) =>
              {
                  TextPassword = e.NewTextValue;
              };
        }
        public static readonly BindableProperty TextPasswordProperty = BindableProperty.Create(
            nameof(TextPassword),
            typeof(string),
            typeof(PasswordLine),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TextPasswordChanged);

        public string TextPassword
        {
            get => (string)GetValue(TextPasswordProperty);
            set => SetValue(TextPasswordProperty, value);
        }

        private static void TextPasswordChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.eText.Text = newValue?.ToString(); 
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(PasswordLine),
            defaultValue: default(double),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: FontSizeChanged);

        public double FontSize
        {
            get => (double)GetValue(TextPasswordProperty);
            set => SetValue(TextPasswordProperty, value);
        }

        private static void FontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.eText.FontSize = (double)newValue;
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(PasswordLine),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: PlaceholderChanged);

        public string Placeholder
        {
            get => GetValue(TextPasswordProperty)?.ToString();
            set => SetValue(TextPasswordProperty, value);
        }

        private static void PlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.eText.Placeholder = newValue?.ToString(); ;
        }

        public static readonly BindableProperty MarginProperty = BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(PasswordLine),
            defaultValue: default(Thickness),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: MarginChanged);

        public Thickness Margin
        {
            get => (Thickness)GetValue(MarginProperty);
            set => SetValue(MarginProperty, value);
        }

        private static void MarginChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.grid.Margin = (Thickness)newValue;
        }
    }
}