using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.Controls
{
    
    public partial class SearchLine : ContentView
    {
        public static readonly BindableProperty TextLineProperty = BindableProperty.Create(
            nameof(TextLine),
            typeof(string),
            typeof(SearchLine),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TextLineChanged);

        public string TextLine
        {
            get => base.GetValue(TextLineProperty)?.ToString();
            set => base.SetValue(TextLineProperty, value);
        }
        private static void TextLineChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;

            control.line.Text = newValue?.ToString();
        }

        public static readonly BindableProperty CommandSearchProperty = BindableProperty.Create(
            nameof(TextLine),
            typeof(ICommand),
            typeof(SearchLine),
            defaultValue: default(Command),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: ComandSearchChanged);

        public ICommand CommandSearch
        {
            get => (ICommand)base.GetValue(TextLineProperty);
            set => base.SetValue(TextLineProperty, value);
        }
        private static void ComandSearchChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;

            control.searchButton.Command = (ICommand)newValue;
        }
        public SearchLine()
        {
            InitializeComponent();

            line.TextChanged += (s, e) => TextLine = e.NewTextValue;
        }
    }
}