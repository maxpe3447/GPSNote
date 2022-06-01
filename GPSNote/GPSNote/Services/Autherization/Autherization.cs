using GPSNote.Models;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Autherization
{
    public class Autherization : IAutherization
    {
        readonly private IRepository _repository;


        public Autherization(
            IRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreateAccount(UserModel user)
        {
            return _repository.InsertAsync(user);
        }       
    }
}
