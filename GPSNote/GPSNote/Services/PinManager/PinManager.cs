using GPSNote.Models;
using GPSNote.Services.Repository;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GPSNote.Extansion;
using GPSNote.Services.Settings;

namespace GPSNote.Services.PinManager
{
    public class PinManager : IPinManager
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;

        public PinManager(IRepository repository,
                          ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }
        public Task<int> DeleteAsync<T>(T entity) where T : IEntity
        {
           return _repository.DeleteAsync(entity);
        }

        public List<PinDataModel> GetAllPins()
        {
            return _repository.GetAllPinsAsync(_settingsManager.UserId)
                              .Result
                              .Where(x => x.UserId == _settingsManager.UserId)
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
