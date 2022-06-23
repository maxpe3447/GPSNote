using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.Settings
{
    public interface ISettingsManagerService
    {
        string LastEmail { get; set; }

        double LastLatitude { get; set; }

        double LastLongitude { get; set; }

        double CameraZoom { get; set; }

        int IsDarkTheme { get; set; }

        int UserId { get; set; }
    }
}
