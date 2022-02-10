using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Controls
{
    public class BorderEntry:Entry
    {
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BorderEntry), Color.Gray);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
    }
}
