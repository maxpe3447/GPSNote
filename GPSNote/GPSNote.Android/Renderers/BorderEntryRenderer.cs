using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using GPSNote.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinSamples.Droid.UI.Renderers;

[assembly: ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]
namespace XamarinSamples.Droid.UI.Renderers
{
    public class BorderEntryRenderer : EntryRenderer
    {
        public BorderEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //Configure native control (TextBox)
            if (Control != null)
            {
                //Control.Background = null;
                var en = e.NewElement as BorderEntry;
                var nativeEditText = (global::Android.Widget.EditText)Control;
                var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                shape.Paint.Color = en.BorderColor.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                shape.Paint.StrokeWidth = 3;
                nativeEditText.Background = shape;

            }

            // Configure Entry properties
            if (e.NewElement != null)
            {
                //e.NewElement.la
            }
        }
            protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(BorderEntry.BorderColor))
            {
                var en = (BorderEntry)sender;
                var nativeEditText = (global::Android.Widget.EditText)Control;
                var fr = new Frame();
                var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                shape.Paint.Color = en.BorderColor.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                shape.Paint.StrokeWidth = 3;
                nativeEditText.Background = shape;
            }
        }
  
    }
}