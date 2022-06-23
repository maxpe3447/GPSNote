using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Services.ThemeManager
{
    public interface IThemeManagerService
    {
        int IsDarkTheme { get; set; }
    }
}
