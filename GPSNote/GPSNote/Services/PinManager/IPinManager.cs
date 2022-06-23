using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.PinManager
{
    public interface IPinManagerService
    {
        Task<List<PinDataModel>> GetAllPins();

        Task<int> AddAsync<T>(T entity);
        Task<int> UpdateAsync<T>(T entity) where T : IEntity;
        Task<int> DeleteAsync<T>(T entity) where T : IEntity;
    }
}
