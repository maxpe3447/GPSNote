
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

            bIsClear.ImageSource = (ImageSource.FromFile("ic_clear"));

            bIsClear.Clicked += ONBIsClearClicked;
            eText.TextChanged += OnEText_TextChanged;
            eText.Unfocused += OnEText_Unfocused;
            eText.Focused += OnEText_Focused; 
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

        public static new readonly BindableProperty MarginProperty = BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(CancelLine),
            defaultValue: default(Thickness),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: MarginChanged);

        public new Thickness Margin
        {
            get => (Thickness)GetValue(MarginProperty);
            set => SetValue(MarginProperty, value);
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
        
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(int),
            typeof(CancelLine),
            defaultValue: default(int),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: StrokeWidthChanged);

        public int StrokeWidth
        {
            get => (int)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = 
            BindableProperty.Create(nameof(TextColor),
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

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily),
                                    typeof(string),
                                    typeof(CancelLine),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnFontFamilyChanged);

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(CancelLine),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: PlaceholderColorChanged);

        public Color PlaceholderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        #region -- Private properties -- 

        private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.Text = newValue?.ToString();
        }

        private static void FontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.FontSize = (double)newValue;
        }

        private static void PlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.Placeholder = newValue?.ToString(); ;
        }

        private static void MarginChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;
            var mrg = (Thickness)newValue;
            control.grid.Margin = mrg;
        }

        private static void BorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.BorderColor = (Color)newValue;
        }

        private static void StrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CancelLine).eText.StrokeWidth = (int)newValue;
        }

        private static void TextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.TextColor = (Color)newValue;
        }

        private static void OnKeyBoardChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cancelLine = (CancelLine)bindable;
            cancelLine.eText.KeyBoard = (Keyboard)newValue;
        }

        private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cancelLine = (CancelLine)bindable;
            cancelLine.eText.FontFamily= newValue.ToString();
        }

        private static void PlaceholderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CancelLine;

            control.eText.PlaceholderColor = (Color)newValue;
        }

        private void OnEText_Focused(object sender, FocusEventArgs e)
        {
            bIsClear.IsVisible = bIsClear.IsEnabled = !string.IsNullOrEmpty(Text);
            FocusedEv?.Invoke(this, new FocusEventArgs(e.VisualElement, e.IsFocused));
        }

        private void OnEText_Unfocused(object sender, FocusEventArgs e)
        {
            bIsClear.IsVisible = bIsClear.IsEnabled = false;
            UnFocusedEv?.Invoke(this, new FocusEventArgs(e.VisualElement, e.IsFocused));
        }

        private void OnEText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = e.NewTextValue;
            bIsClear.IsVisible = bIsClear.IsEnabled = !string.IsNullOrEmpty(Text);
            TextChangedEv?.Invoke(this, new TextChangedEventArgs(e.OldTextValue, e.NewTextValue));
        }

        private void ONBIsClearClicked(object sender, EventArgs e)
        {
            Text = string.Empty;
        }

        #endregion
    }
}