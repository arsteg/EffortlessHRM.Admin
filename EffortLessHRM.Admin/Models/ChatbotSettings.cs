using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EffortLessHRM.Admin.Models
{
    [BsonIgnoreExtraElements]
    public class ChatbotSettings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("_id")]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Question is required")]
        public string question { get; set; }

        [Required(ErrorMessage = "At least one answer is required")]
        [MinLength(1, ErrorMessage = "At least one answer is required")]
        public List<ChatAnswer> answer { get; set; }
        public DateTime createdOn { get; set; }
        public bool active { get; set; } = true;

        [BsonElement("embedding")]
        public BsonArray? embeddingRaw { get; set; }

        [BsonIgnore]
        public List<float>? embedding
        {
            get
            {
                if (embeddingRaw != null)
                {
                    return embeddingRaw
                        .Select(b => (float)b.AsDouble)  // safely cast each item to float
                        .ToList();
                }
                return null;
            }
            set
            {
                embeddingRaw = value != null
                    ? new BsonArray(value.Select(v => new BsonDouble(v)))
                    : null;
            }
        }
        public DateTime updatedOn { get; set; }
    }

    public class ChatAnswer
    {
        public string type { get; set; }
        public string content { get; set; }
    }

}
