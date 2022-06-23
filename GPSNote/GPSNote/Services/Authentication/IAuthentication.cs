using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> IsExistAsync(UserModel model);
        Task<bool> IsExistEmailAsync(string email);

        string LastEmail { get; set; }

        int UserId { get; set; }
    }
}
