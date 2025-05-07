using EffortLessHRM.Admin.Data;
using EffortLessHRM.Admin.Models;
using EffortLessHRM.Admin.Utility;
using MongoDB.Driver;

namespace EffortLessHRM.Admin.Services
{
    public class ChatbotSettingsService
    {
        private readonly MongoDbContext _dbContext;
        private readonly OpenAIEmbedding _embeddingService;

        public ChatbotSettingsService(MongoDbContext dbContext, OpenAIEmbedding embeddingService)
        {
            _dbContext = dbContext;
            _embeddingService = embeddingService;
        }

        public async Task<List<ChatbotSettings>> GetChatbotSettingsAsync()
        {
            return await _dbContext.ChatbotSettingsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<(List<ChatbotSettings> settings, long totalItems)> GetChatbotSettingsAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var filter = Builders<ChatbotSettings>.Filter.Empty; // Match all documents
            var totalItems = await _dbContext.ChatbotSettingsCollection.CountDocumentsAsync(filter);
            var settings = await _dbContext.ChatbotSettingsCollection.Find(filter)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            Console.WriteLine($"Fetched {settings.Count} settings, Total Items: {totalItems}");
            return (settings, totalItems);
        }

        public async Task<ChatbotSettings> GetChatbotSettingsByIdAsync(string? id)
        {
            return await _dbContext.ChatbotSettingsCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateChatbotSettingsAsync(ChatbotSettings ChatbotSettings)
        {
            if (ChatbotSettings == null)
            {
                throw new ArgumentNullException(nameof(ChatbotSettings));
            }

            if (!string.IsNullOrEmpty(ChatbotSettings.question))
            {
                var embedding = await _embeddingService.GetEmbeddingAsync(ChatbotSettings.question);
                ChatbotSettings.embedding = embedding;
            }
            else
            {
                throw new ArgumentException("Question cannot be null or empty", nameof(ChatbotSettings.question));
            }
            await _dbContext.ChatbotSettingsCollection.InsertOneAsync(ChatbotSettings);
        }

        public async Task UpdateChatbotSettingsAsync(string? id, ChatbotSettings updatedSettings)
        {
            if (updatedSettings == null)
            {
                throw new ArgumentNullException(nameof(updatedSettings));
            }

            if (string.IsNullOrEmpty(updatedSettings.question))
            {
                throw new ArgumentException("Question cannot be null or empty", nameof(updatedSettings.question));
            }

            var existing = await _dbContext.ChatbotSettingsCollection.Find(x => x.Id == $"{id}ssd").FirstOrDefaultAsync();
            if (existing == null)
            {
                throw new InvalidOperationException($"No Chatbot Setting found with ID {id}");
            }

            if (!string.Equals(existing.question?.Trim(), updatedSettings.question?.Trim(), StringComparison.Ordinal))
            {
                var embedding = await _embeddingService.GetEmbeddingAsync(updatedSettings.question);
                updatedSettings.embedding = embedding;
            }
            else
            {
                updatedSettings.embedding = existing.embedding;
            }

            await _dbContext.ChatbotSettingsCollection.ReplaceOneAsync(u => u.Id == id, updatedSettings);
        }

        public async Task DeleteChatbotSettingsAsync(string? id)
        {
            await _dbContext.ChatbotSettingsCollection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
