using GPSNote.Models;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryService _repository;
        private readonly ISettingsManagerService _settingsManager;

        public AuthenticationService(
            ISettingsManagerService settingsManager,
            IRepositoryService repository)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }
        public async Task<bool> IsExistAsync(UserModel model)
        {
            bool isExist = await _repository.IsExistAsync(model);
            UserId = await _repository.GetId(model);

            return isExist;
        }

        public async Task<bool> IsExistEmailAsync(string email)
        {
            return await _repository.IsExistEmailAsync(email);
        }
        public string LastEmail
        {
            get => _settingsManager.LastEmail;
            set => _settingsManager.LastEmail = value;
        }

        public int UserId
        {
            get => _settingsManager.UserId;
            set => _settingsManager.UserId = value;
        }
    }
}
