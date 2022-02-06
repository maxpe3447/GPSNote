﻿using GPSNote.Models;
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
        Task<List<T>> GetAllAsync<T>() where T : new();
        Task<bool> IsExistAsync(UserModel model);
    }
}
