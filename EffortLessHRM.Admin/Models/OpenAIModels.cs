namespace EffortLessHRM.Admin.Models
{
    public class OpenAIModels
    {
    }

    public class EmbeddingResponse
    {
        public List<EmbeddingData> data { get; set; }
    }

    public class EmbeddingData
    {
        public List<float> embedding { get; set; }
    }
}
