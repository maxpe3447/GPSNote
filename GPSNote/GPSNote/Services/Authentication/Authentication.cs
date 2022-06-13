using GPSNote.Models;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;

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
        public bool IsExist(UserModel model)
        {
            bool isExist = _repository.IsExist(model, out var id);
            UserId = id;

            return isExist;
        }

        public bool IsExistEmail(string email)
        {
            return _repository.IsExistEmail(email);
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
