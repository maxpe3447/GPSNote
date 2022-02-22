﻿using GPSNote.Resources;
using System;
using Xamarin.Forms;

namespace GPSNote.Controls
{
    public partial class PasswordLine : ContentView
    {
        public PasswordLine()
        {
            InitializeComponent();

            bIsShowPass.ImageSource = (ImageSource)App.Current.Resources[ImageNames.ic_eye_off];
            
            bIsShowPass.Clicked += (s, e) =>
              {
                  eText.IsPassword = !eText.IsPassword;

                  if (eText.IsPassword)
                  {
                      bIsShowPass.ImageSource = (ImageSource)App.Current.Resources[ImageNames.ic_eye_off];
                  }
                  else
                  {
                      bIsShowPass.ImageSource = (ImageSource)App.Current.Resources[ImageNames.ic_eye];
                  }
              };

            eText.TextChanged += (s, e) =>
              {
                  TextPassword = e.NewTextValue;

                  bIsShowPass.IsVisible = bIsShowPass.IsEnabled = !string.IsNullOrEmpty(TextPassword);
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
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
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
            get => GetValue(PlaceholderProperty)?.ToString();
            set => SetValue(PlaceholderProperty, value);
        }

        private static void PlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.eText.Placeholder = newValue?.ToString(); ;
        }

        public static new readonly BindableProperty MarginProperty = BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(PasswordLine),
            defaultValue: default(Thickness),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: MarginChanged);

        public new Thickness Margin
        {
            get => (Thickness)GetValue(MarginProperty);
            set => SetValue(MarginProperty, value);
        }

        private static void MarginChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;
            var mrg = (Thickness)newValue;
            control.grid.Margin = new Thickness(mrg.Left, mrg.Top, mrg.Right - 40, mrg.Bottom);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(PasswordLine),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: BorderColorChanged);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set
            {
                SetValue(BorderColorProperty, value);
                
            }
        }

        private static void BorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.eText.BorderColor = (Color)newValue;
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(PasswordLine),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TextColorChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        private static void TextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.eText.TextColor = (Color)newValue;
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily),
                                    typeof(string),
                                    typeof(PasswordLine),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnFontFamilyChanged);

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var passLine = (PasswordLine)bindable;
            passLine.eText.FontFamily = newValue.ToString();
        }
        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(PasswordLine),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: PlaceholderColorChanged);

        public Color PlaceholderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        private static void PlaceholderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as PasswordLine;

            control.eText.PlaceholderColor = (Color)newValue;
        }
    }
}