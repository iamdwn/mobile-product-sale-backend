using ProductSale.Api.Services.Interfaces;

namespace ProductSale.Api.Services
{
    public class GoMapsProService : IGoMapsProService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AlzaSyJ-TujuvlBIoq23w5Gf1hpMTTz6k5NsZxV";

        public GoMapsProService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetLocationDataAsync(string query)
        {
            var requestUrl = $"https://app.gomaps.pro/apis?query={Uri.EscapeDataString(query)}&key={_apiKey}";
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            //return JObject.Parse(content);
            return content;
        }
    }
}
