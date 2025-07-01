namespace ObiletCase.Services
{
    public class ObiletApiService
    {
        private readonly HttpClient _httpClient;

        public ObiletApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ObiletClient");
        }

        public async Task<string> GetSessionAsync()
        {
            var requestBody = new
            {
            };

            var response = await _httpClient.PostAsJsonAsync("/client/getsession", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result; 
            }

            return String.Empty;
        }
    }
}
