using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using MongoDB.Driver;

namespace DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly MongoClient _client;

        public CategoryRepository()
        {
            // Sätt upp anslutningen en gång i konstruktorn
            string connectionString = "mongodb+srv://simonpetterson20_db_user:Petterson20@orumongodb.fud0i2g.mongodb.net/?appName=OruMongoDB";
            _client = new MongoClient(connectionString);
            IMongoDatabase database = _client.GetDatabase("PoddAppDB");
            _categories = database.GetCollection<Category>("Categories");
        }

        public async Task CreateAsync(Category category)
        {
            // 1. Starta en session
            using var session = await _client.StartSessionAsync();

            // 2. Starta transaktionen
            session.StartTransaction();
            try
            {
                // 3. Utför databasoperationen och skicka med sessionen
                await _categories.InsertOneAsync(session, category);

                // 4. Commita (bekräfta och spara) transaktionen
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                // Om något går fel, avbryt och rulla tillbaka transaktionen
                await session.AbortTransactionAsync();
                throw; // Kasta vidare felet så att PL/BL kan fånga det
            }
        }

        public async Task<List<Category>> GetAllAsync()
        {
            // Hämtning (Read) kräver ingen transaktion
            return await _categories.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await _categories.ReplaceOneAsync(session, c => c.Id == category.Id, category);
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                // Ändrad till c => c.Id == id
                await _categories.DeleteOneAsync(session, c => c.Id == id);
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}