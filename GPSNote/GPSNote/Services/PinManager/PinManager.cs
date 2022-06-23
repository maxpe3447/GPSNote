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
    public class PinManagerService : IPinManagerService
    {
        private readonly IRepositoryService _repository;
        private readonly ISettingsManagerService _settingsManager;

        public PinManagerService(IRepositoryService repository,
                          ISettingsManagerService settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }
        public Task<int> DeleteAsync<T>(T entity) where T : IEntity
        {
           return _repository.DeleteAsync(entity);
        }

        public async Task<List<PinDataModel>> GetAllPins()
        {
            var pins = _repository.GetAllPinsAsync(_settingsManager.UserId);

            return (await pins)
                       .Where(x => x.UserId == _settingsManager.UserId)
                       .ToList();

        }

        public Task<int> AddAsync<T>(T entity)
        {
            return _repository.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : IEntity
        {
            return _repository.UpdateAsync(entity);
        }
    }
}
