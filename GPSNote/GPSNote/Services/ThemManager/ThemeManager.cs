using GPSNote.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.ThemeManager
{
    public class ThemeManager : IThemeManager
    {
        readonly private ISettingsManager _settingsManager;

        public ThemeManager(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager; 
        }
        public bool IsDarkTheme 
        {
            get => _settingsManager.IsDarkTheme; 
            set => _settingsManager.IsDarkTheme = value; 
        }
    }
}
