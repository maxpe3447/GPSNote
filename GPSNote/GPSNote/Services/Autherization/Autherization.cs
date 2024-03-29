﻿using GPSNote.Models;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GPSNote.Services.Autherization
{
    public class AutherizationService : IAutherizationService
    {
        readonly private IRepositoryService _repository;


        public AutherizationService(
            IRepositoryService repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAccount(UserModel user)
        {
            return await _repository.InsertAsync(user);
        }       
    }
}
