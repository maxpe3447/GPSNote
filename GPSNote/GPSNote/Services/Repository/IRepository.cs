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
        Task<int> UpdateAsync<T>(T entity);
        Task <int> DeleteAsync<T>(T entity);
        Task<List<PinModel>> GetAllPinsAsync(int userId);
        bool IsExistAsync(UserModel model, out int id);
    }
}
