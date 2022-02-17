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
        public string LastPassword
        {
            get => Preferences.Get(nameof(LastPassword), string.Empty);
            set => Preferences.Set(nameof(LastPassword), string.Empty, value);
        }
    }
}
