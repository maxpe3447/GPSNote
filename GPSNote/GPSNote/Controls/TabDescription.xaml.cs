using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static readonly BindableProperty HeightProperty =
            BindableProperty.Create(nameof(Height),
                                    typeof(double),
                                    typeof(TabDescription),
                                    default(double),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnHeightChanged);

        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        private static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabDesc = (TabDescription)bindable;
             tabDesc.grid.HeightRequest= (double)newValue;
            
        }
    }
}