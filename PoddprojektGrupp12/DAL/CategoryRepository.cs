using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using MongoDB.Driver;

namespace DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository()
        {
            string connectionString = "mongodb+srv://simonpetterson20_db_user:Petterson20@orumongodb.fud0i2g.mongodb.net/?appName=OruMongoDB";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("PoddAppDB");

            _categories = database.GetCollection<Category>("Categories");
        }

        public async Task CreateAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            await _categories.ReplaceOneAsync(c => c.Id == category.Id, category);
        }

        // UPPDATERAD: Nu raderar vi baserat på Name istället för Id
        public async Task DeleteAsync(string categoryName)
        {
            await _categories.DeleteOneAsync(c => c.Name == categoryName);
        }
    }
}