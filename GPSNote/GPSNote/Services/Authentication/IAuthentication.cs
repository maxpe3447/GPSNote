using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.Authentication
{
    public interface IAuthentication
    {
        bool IsExist(UserModel model);

        string LastEmail { get; set; }

        int UserId { get; set; }
    }
}
