using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Droid.Renderers;
using GPSNote.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]
namespace Droid.Renderers
{
    public class BorderEntryRenderer : EntryRenderer
    {
        public BorderEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var newElement = e.NewElement as BorderEntry;
                var nativeEditText = (global::Android.Widget.EditText)Control;
                var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                shape.Paint.Color = newElement.BorderColor.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                shape.Paint.StrokeWidth = (int)newElement?.StrokeWidth;
                nativeEditText.Background = shape;

            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(BorderEntry.BorderColor) ||
               e.PropertyName == nameof(BorderEntry.StrokeWidth))
            {
                var newElement = (BorderEntry)sender;
                var nativeEditText = (global::Android.Widget.EditText)Control;
                var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                shape.Paint.Color = newElement.BorderColor.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                shape.Paint.StrokeWidth = (int)newElement?.StrokeWidth;
                nativeEditText.Background = shape;
            }
        }
  
    }
}