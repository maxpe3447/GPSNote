using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GPSNote.Services.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public string LastEmail
        {
            get => Preferences.Get(nameof(LastEmail), string.Empty);
            set => Preferences.Set(nameof(LastEmail), value);
        }

        public double LastLatitude 
        { 
            get => Preferences.Get(nameof(LastLatitude), 47.824734);
            set => Preferences.Set(nameof(LastLatitude), value);
        }
        public double LastLongitude 
        { 
            get => Preferences.Get(nameof(LastLongitude), 35.1625);
            set => Preferences.Set(nameof(LastLongitude), value);
        }
        public double CameraZoom 
        { 
            get => Preferences.Get(nameof(CameraZoom), 13.0);
            set => Preferences.Set(nameof(CameraZoom), value);
        }
        public int IsDarkTheme 
        { 
            get => Preferences.Get(nameof(IsDarkTheme), Convert.ToInt32((App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)));
            set => Preferences.Set(nameof(IsDarkTheme), value);
        }
        public int UserId 
        {
            get => Preferences.Get(nameof(UserId), default(int)); 
            set => Preferences.Set(nameof(UserId), value);
        }
    }
}
