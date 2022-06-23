using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Repository
{
    public interface IRepositoryService
    {
        Task<int> InsertAsync<T>(T entity);
        Task<int> UpdateAsync<T>(T entity) where T : IEntity;
        Task <int> DeleteAsync<T>(T entity) where T : IEntity;
        Task<List<PinDataModel>> GetAllPinsAsync(int userId);
        Task<int> GetId(UserModel model);
        Task<bool> IsExistAsync(UserModel model);
        Task<bool> IsExistEmailAsync(string email);
    }
}
