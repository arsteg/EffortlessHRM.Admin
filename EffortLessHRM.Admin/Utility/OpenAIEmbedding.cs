using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using EffortLessHRM.Admin.Models;

namespace EffortLessHRM.Admin.Utility
{
    public class OpenAIEmbedding
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _model;

        public OpenAIEmbedding(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
            _endpoint = configuration["OpenAI:Endpoint"];
            _model = configuration["OpenAI:Model"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<float>> GetEmbeddingAsync(string inputText)
        {
            var requestBody = new
            {
                input = inputText,
                model = _model
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_endpoint}embeddings", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API Error: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var embeddingResponse = JsonSerializer.Deserialize<EmbeddingResponse>(responseJson);

            return embeddingResponse?.data.FirstOrDefault()?.embedding ?? new List<float>();
        }
    }
}
