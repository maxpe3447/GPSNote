using GPSNote.Models;
using GPSNote.Services.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.PinManager
{
    public class PinManager : IPinManager
    {

        public PinManager(IRepository repository)
        {
            _Repository = repository;
        }
        public Task<int> DeleteAsync<T>(T entity) where T : IEntity
        {
           return _Repository.DeleteAsync(entity);
        }

        public Task<List<PinModel>> GetAllPinsAsync(int userId)
        {
            return _Repository.GetAllPinsAsync(userId);
        }

        public Task<int> InsertAsync<T>(T entity)
        {
            return _Repository.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : IEntity
        {
            return _Repository.UpdateAsync(entity);
        }

        private IRepository _Repository { get; }
    }
}
