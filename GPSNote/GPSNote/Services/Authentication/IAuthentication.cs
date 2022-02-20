using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.Authentication
{
    public interface IAuthentication
    {
        bool IsExistAsync(UserModel model, out int id);
    }
}
