using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Controls
{
    public class BorderEntry:Entry
    {

        #region -- Public properties -- 

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), 
                                    typeof(Color), 
                                    typeof(BorderEntry), 
                                    default(Color),
                                    defaultBindingMode: BindingMode.TwoWay);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }


        public static readonly BindableProperty KeyBoardProperty =
            BindableProperty.Create(nameof(KeyBoard), 
                                    typeof(Keyboard), 
                                    typeof(BorderEntry), 
                                    default(Keyboard),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnKeyBoardChanged);

        public Keyboard KeyBoard
        {
            get { return (Keyboard)GetValue(KeyBoardProperty); }
            set { SetValue(KeyBoardProperty, value); }
        }
        #endregion

        #region -- Private properties -- 
        private static void OnKeyBoardChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var entry = (BorderEntry)bindable;
            entry.Keyboard = (Keyboard)newValue;

        }
        #endregion
    }
}
