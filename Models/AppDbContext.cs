using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BirthdayBot.Models
{
    public class AppDbContext
    {
        IMongoDatabase _database;

        public AppDbContext()
        {
            string connectionString = "mongodb+srv://LMichael:123443215@cluster0-fhg01.azure.mongodb.net/test?retryWrites=true&w=majority";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase("BirthdayBot");
        }

        private IMongoCollection<User> Users
        {
            get { return _database.GetCollection<User>("Users"); }
        }

        public async Task<IEnumerable<User>> GetUsers(string name, long chatId)
        {
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty;

            if (!String.IsNullOrWhiteSpace(name))
            {
                filter &= builder.Regex("Name", new BsonRegularExpression(name));

                filter &= builder.Eq("ChatId", chatId);
            }

            return await Users.Find(filter).ToListAsync();
        }

        public async Task<User> GetUser(string id)
        {
            return await Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task Create(User p)
        {
            await Users.InsertOneAsync(p);
        }

        public async Task Update(User p)
        {
            await Users.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
        }

        public async Task Remove(string id)
        {
            await Users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
    }
}

