using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DAL
{
    internal static class MongoContext
    {
        private const string ConnectionString = "mongodb+srv://simonpetterson20_db_user:Petterson20@orumongodb.fud0i2g.mongodb.net/?appName=OruMongoDB";

        private const string DatabaseName = "PoddAppDB";

        private static MongoClient? _client;

        public static MongoClient Client => _client ??= new MongoClient(ConnectionString);

        public static IMongoDatabase Database => Client.GetDatabase(DatabaseName);

        public static async Task RunInTransactionAsync(Func<IClientSessionHandle, Task> work)
        {
            using var session = await Client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await work(session);
                await session.CommitTransactionAsync();
            }
            catch
            {
                try { await session.AbortTransactionAsync(); }
                catch { /* ignorera, vi vill kasta det ursprungliga felet */ }
                throw;
            }
        }
    }
}