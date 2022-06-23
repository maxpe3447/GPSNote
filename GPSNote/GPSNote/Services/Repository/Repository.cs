using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using System.Threading.Tasks;
using System.IO;
using GPSNote.Models;
using System.Linq;

namespace GPSNote.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private SQLiteAsyncConnection database;

        public RepositoryService()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mapnote.db3");
            database = new SQLiteAsyncConnection(path);
            database.CreateTableAsync<PinDataModel>();
            database.CreateTableAsync<UserModel>();
        }
        public Task<int> DeleteAsync<T>(T entity) where T : IEntity
        {
            return database.DeleteAsync(entity);
        }

        public Task<List<PinDataModel>> GetAllPinsAsync(int userId)
        {
        var tables = database.Table<PinDataModel>().ToListAsync();

            return tables;
        }

        public Task<int> InsertAsync<T>(T entity)
        {
            return database.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T: IEntity
        {
            return database.UpdateAsync(entity);
        }

        public async Task<int> GetId(UserModel model)
        {
            var table = await database.Table<UserModel>().ToListAsync();

            return (await IsExistAsync(model))? table.Where(x => x.Email == model.Email && 
                                                x.Password == model.Password).First().Id : 0;
        }

        public async Task<bool> IsExistAsync(UserModel model)
        {
            var table = await database.Table<UserModel>().ToListAsync();
            bool isExist = table.Exists(x => x.Email == model.Email && x.Password == model.Password);
            return isExist;
        }

        public async Task<bool> IsExistEmailAsync(string email)
        {
            var table = await database.Table<UserModel>().ToListAsync();

            return table.Exists(x=>x.Email == email);
        }
    }
}
