using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using System.Threading.Tasks;
using System.IO;
using GPSNote.Models;

namespace GPSNote.Services.Repository
{
    internal class Repository : IRepository
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

        public Task<List<T>> GetAllAsync<T>() where T : new()
        {
            return database.Value.Table<T>().ToListAsync();
        }

        public Task<int> InsertAsync<T>(T entity)
        {
            return database.Value.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity)
        {
            return database.Value.UpdateAsync(entity);
        }
        public async Task<bool> IsExistAsync<T>(UserModel model)
        {
            var table = database.Value.FindAsync<UserModel>(model);
            return await table != null;
        }

    }
}
