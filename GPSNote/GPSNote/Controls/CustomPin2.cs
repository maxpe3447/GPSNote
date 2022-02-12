using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GPSNote.Controls
{
    public class CustomPin2 : ContentView
    {

        private readonly ControlTemplate _icon = new ControlTemplate(typeof(IconTemplate));

        public CustomPin2()
        {
            
        }

        public static BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(CustomPin), propertyChanged: IconChanged);

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }


        private static void IconChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomPin2)bindable;

            //view.IsVisible = (view.Icon != null);
            view.ControlTemplate = view._icon;
        }
    }

    class IconTemplate : ContentView
    {
        public IconTemplate()
        {
            var icon = new Image();
            icon.SetBinding(Image.SourceProperty, new TemplateBinding("Icon"));
            icon.SetBinding(Image.StyleProperty, new TemplateBinding("Iconstyle"));
            Content = new StackLayout
            {
                Children =
                {
                    new StackLayout
                    {
                        Children = {
                            icon, 
                            new Label { Text ="wdwdw"}
                        }
                    },
                    new ContentPresenter()
                }
            };
        }
    }
}