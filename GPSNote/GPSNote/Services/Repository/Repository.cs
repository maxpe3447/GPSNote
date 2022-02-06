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
                return database;
            });
        }
        public Task<int> DeleteAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
