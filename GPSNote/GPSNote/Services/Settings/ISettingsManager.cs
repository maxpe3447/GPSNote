using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.Settings
{
    public interface ISettingsManager
    {
        string LastEmail { get; set; }
        string LastPassword { get; set; }

        double LastLatitude { get; set; }
        double LastLongitude { get; set; }

        double CameraZoom { get; set; }
    }
}
