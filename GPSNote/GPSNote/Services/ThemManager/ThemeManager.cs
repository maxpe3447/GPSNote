using GPSNote.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.ThemeManager
{
    public class ThemeManagerService : IThemeManagerService
    {
        private readonly ISettingsManagerService _settingsManager;

        public ThemeManagerService(ISettingsManagerService settingsManager)
        {
            _settingsManager = settingsManager; 
        }
        public int IsDarkTheme 
        {
            get => _settingsManager.IsDarkTheme; 
            set => _settingsManager.IsDarkTheme = value; 
        }
    }
}
