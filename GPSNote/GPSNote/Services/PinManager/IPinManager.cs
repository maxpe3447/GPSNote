using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.PinManager
{
    public interface IPinManager
    {
        Task<List<PinModel>> GetAllPinsAsync(int userId);
        Task<int> InsertAsync<T>(T entity);
        Task<int> UpdateAsync<T>(T entity) where T : IEntity;
        Task<int> DeleteAsync<T>(T entity) where T : IEntity;
    }
}
