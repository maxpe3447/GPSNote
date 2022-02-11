using GPSNote.Models;
using GPSNote.Services.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        public Authentication(IRepository repository)
        {
            _Repository = repository;
        }
        public bool IsExistAsync(UserModel model, out int id)
        {
            return _Repository.IsExistAsync(model, out id);
        }

        #region -- Private --
        IRepository _Repository { get; }
        #endregion
    }
}
