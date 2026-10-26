using Models;
using MongoDB.Driver;

namespace DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private static IMongoCollection<Category> Categories =>
            MongoContext.Database.GetCollection<Category>("Categories");

        private static IMongoCollection<Podcast> Podcasts =>
            MongoContext.Database.GetCollection<Podcast>("Podcasts");

        public Task CreateAsync(Category category) =>
            MongoContext.RunInTransactionAsync(session => Categories.InsertOneAsync(session, category));

        public async Task<List<Category>> GetAllAsync() =>
            await Categories.Find(_ => true).ToListAsync();

        public async Task<Category?> GetByIdAsync(string id) =>
            await Categories.Find(c => c.Id == id).FirstOrDefaultAsync();

        public Task RenameAsync(string id, string oldName, string newName) =>
            MongoContext.RunInTransactionAsync(async session =>
            {
                await Categories.UpdateOneAsync(session, c => c.Id == id,
                    Builders<Category>.Update.Set(c => c.Name, newName));

                await Podcasts.UpdateManyAsync(session, p => p.Category == oldName,
                    Builders<Podcast>.Update.Set(p => p.Category, newName));
            });

        public Task DeleteAsync(string id, string name) =>
            MongoContext.RunInTransactionAsync(async session =>
            {
                await Categories.DeleteOneAsync(session, c => c.Id == id);

                await Podcasts.UpdateManyAsync(session, p => p.Category == name,
                    Builders<Podcast>.Update.Set(p => p.Category, string.Empty));
            });
    }
}