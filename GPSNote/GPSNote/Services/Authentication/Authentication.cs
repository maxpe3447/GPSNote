using GPSNote.Models;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;

        public Authentication(
            ISettingsManager settingsManager,
            IRepository repository)
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
