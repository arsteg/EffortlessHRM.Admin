using EffortLessHRM.Admin.Configurations;
using EffortLessHRM.Admin.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EffortLessHRM.Admin.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> options)
        {
            var mongoDbSettings = options.Value;
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.DatabaseName);
        }

        public IMongoCollection<ChatbotSettings> ChatbotSettingsCollection =>
            _database.GetCollection<ChatbotSettings>("ChatbotData");
    }
}
