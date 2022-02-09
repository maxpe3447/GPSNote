﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using System.Threading.Tasks;
using System.IO;
using GPSNote.Models;

namespace GPSNote.Services.Repository
{
    public class Repository : IRepository
    {
        private Lazy<SQLiteAsyncConnection> database;
        public Repository()
        {
            database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mapnote.db3");
                var database = new SQLiteAsyncConnection(path);
                database.CreateTableAsync<PinModel>();
                database.CreateTableAsync<UserModel>();
                return database;
            });
        }
        public Task<int> DeleteAsync<T>(T entity)
        {
            return database.Value.DeleteAsync(entity);
        }

        public Task<List<PinModel>> GetAllPinsAsync(int userId)
        {
          var tables = database.Value.QueryAsync<PinModel>($"SELECT * FROM {nameof(PinModel)} WHERE {nameof(PinModel.UserId)} = ?;", userId);

            return tables;
        }

        public Task<int> InsertAsync<T>(T entity)
        {
            return database.Value.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity)
        {
            return database.Value.UpdateAsync(entity);
        }
        public bool IsExistAsync(UserModel model, out int id)
        {
            var table = database.Value.QueryAsync<UserModel>(
                $"SELECT * FROM {nameof(UserModel)} WHERE {nameof(UserModel.Email)} = ? AND {nameof(UserModel.Password)} = ?;",
                model.Email, model.Password);

            //var lst = database.Value.Table<UserModel>();

            id = (table.Result.Count > 0) ? table.Result[0].Id : 0;
            
            return table.Result.Count > 0;
        }

    }
}