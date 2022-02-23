using Foundation;
using GPSNote.Controls;
using GPSNote.iOS.Renders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]
namespace GPSNote.iOS.Renders
{
    public class BorderEntryRenderer:EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //Configure Native control (UITextField)
            if (Control != null)
            {
                Control.Layer.BorderWidth = 3;
                Control.Layer.BorderColor = UIColor.Black.CGColor;
            }
        }

        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);
        //    if (e.PropertyName == nameof(BorderEntry.BorderColor))
        //    {
        //        var en = (BorderEntry)sender;
        //        Control.Layer.BorderWidth = 3;
        //        Control.Layer.BorderColor = en.BorderColor.ToCGColor();
        //    }
        //}
    }
}