using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNote.Extansion
{
    public static  class ListExtansion
    {
        public static void AddRange(this IList<Pin> oldPins, IList newPins)
        {
            foreach (var pin in newPins)
            {
                oldPins.Add((Pin)pin);
            }
        }
        
    }
}
