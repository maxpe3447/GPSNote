using GPSNote.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.Controls
{
    public partial class CancelLine : ContentView
    {
        public CancelLine()
        {
            InitializeComponent();

            bIsClear.ImageSource = ImageSource.FromFile(ImageNames.Cancel);

            bIsClear.Clicked += (s, e) =>
            {
                Text = string.Empty;
            };

            eText.TextChanged += (s, e) =>
            {
                Text = e.NewTextValue;
                bIsClear.IsVisible = bIsClear.IsEnabled = !string.IsNullOrEmpty(Text);
                TextChangedEv?.Invoke(this,  new TextChangedEventArgs(e.OldTextValue, e.NewTextValue));
            };


            eText.Unfocused += (s, e) =>
            {
                bIsClear.IsVisible = bIsClear.IsEnabled = false;
                UnFocusedEv?.Invoke(this, new FocusEventArgs(e.VisualElement, e.IsFocused));
            };

            eText.Focused += (s, e) =>
              {
                  bIsClear.IsVisible = bIsClear.IsEnabled = !string.IsNullOrEmpty(Text);
                  FocusedEv?.Invoke(this, new FocusEventArgs(e.VisualElement, e.IsFocused));
              };
        }

        public delegate void TextChangedHandler(object sender, TextChangedEventArgs e);
        public event TextChangedHandler TextChangedEv;

        public delegate void FocusedHandler(object sender, FocusEventArgs e);
        public event FocusedHandler FocusedEv;

        public delegate void UnFocusedHandler(object sender, FocusEventArgs e);
        public event UnFocusedHandler UnFocusedEv;

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(CancelLine),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TextChanged);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.Text = newValue?.ToString();
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(CancelLine),
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
            var control = bindable as CancelLine;

            control.eText.FontSize = (double)newValue;
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(CancelLine),
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
            var control = bindable as CancelLine;

            control.eText.Placeholder = newValue?.ToString(); ;
        }

        public static readonly BindableProperty MarginProperty = BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(CancelLine),
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
            var control = bindable as CancelLine;
            var mrg = (Thickness)newValue;
            control.grid.Margin = mrg;//new Thickness(mrg.Left, mrg.Top, mrg.Right - 40, mrg.Bottom);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(CancelLine),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: BorderColorChanged);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        private static void BorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.BorderColor = (Color)newValue;
        }
        //
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(CancelLine),
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
            var control = bindable as CancelLine;

            control.eText.TextColor = (Color)newValue;
        }

        public static readonly BindableProperty KeyBoardProperty =
            BindableProperty.Create(nameof(KeyBoard),
                                    typeof(Keyboard),
                                    typeof(CancelLine),
                                    default(Keyboard),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnKeyBoardChanged);

        public Keyboard KeyBoard
        {
            get { return (Keyboard)GetValue(KeyBoardProperty); }
            set { SetValue(KeyBoardProperty, value); }
        }

        #region -- Private properties -- 
        private static void OnKeyBoardChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cancelLine = (CancelLine)bindable;
            cancelLine.eText.KeyBoard = (Keyboard)newValue;
        }
        #endregion
    }
}