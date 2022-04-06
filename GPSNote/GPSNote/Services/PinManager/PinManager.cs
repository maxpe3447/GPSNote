using GPSNote.Models;
using GPSNote.Services.Repository;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GPSNote.Extansion;

namespace GPSNote.Services.PinManager
{
    public class PinManager : IPinManager
    {
        private readonly IRepository _repository;

        public PinManager(IRepository repository)
        {
            _repository = repository;
        }
        public Task<int> DeleteAsync<T>(T entity) where T : IEntity
        {
           return _repository.DeleteAsync(entity);
        }

        public List<PinDataModel> GetAllPins(int userId)
        {
            return _repository.GetAllPinsAsync(userId)
                              .Result
                              .Where(x => x.UserId == userId)
                              .ToList();

        }

        public Task<int> InsertAsync<T>(T entity)
        {
            return _repository.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : IEntity
        {
            return _repository.UpdateAsync(entity);
        }
    }
}
