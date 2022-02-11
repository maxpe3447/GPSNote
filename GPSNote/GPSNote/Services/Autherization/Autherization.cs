using GPSNote.Models;
using GPSNote.Services.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Autherization
{
    public class Autherization : IAutherization
    {
        public Autherization(IRepository repository)
        {
            _Repository = repository;
        }
        public Task<int> CreateAccount(UserModel user)
        {
            return _Repository.InsertAsync(user);
        }

        #region -- Private --
        private IRepository _Repository { get; }
        #endregion
    }
}
