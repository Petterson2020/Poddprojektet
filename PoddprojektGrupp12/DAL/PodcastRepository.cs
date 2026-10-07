using MongoDB.Driver;

namespace DAL
{
    public class PodcastRepository
    {
        public PodcastRepository()
        {
            // Ersätt med ditt riktiga lösenord
            string connectionString = "mongodb+srv://simonpetterson20_db_user:Petterson20@orumongodb.fud0i2g.mongodb.net/?appName=OruMongoDB";

            MongoClient client = new MongoClient(connectionString);

            // Byt ut PoddAppDB mot vad du vill kalla din testdatabas
            IMongoDatabase database = client.GetDatabase("PoddAppDB");
        }
    }
}