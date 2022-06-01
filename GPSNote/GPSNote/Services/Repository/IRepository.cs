using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Repository
{
    public interface IRepository
    {
        Task<int> InsertAsync<T>(T entity);
        Task<int> UpdateAsync<T>(T entity) where T : IEntity;
        Task <int> DeleteAsync<T>(T entity) where T : IEntity;
        Task<List<PinDataModel>> GetAllPinsAsync(int userId);
        bool IsExist(UserModel model, out int id);
        bool IsExistEmail(string email);
    }
}
