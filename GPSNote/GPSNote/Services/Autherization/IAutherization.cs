using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Autherization
{
    public interface IAutherization
    {
        Task<int> CreateAccount(UserModel user);
    }
}
