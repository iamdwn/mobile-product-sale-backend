using ProductSale.Data.DTO.ResponseModel;
using System.Text;
using System.Text.Json;

namespace ProductSale.Api.Clients
{
    public class PayOSClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;

        public PayOSClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _clientId = Environment.GetEnvironmentVariable("ClientId");
            _apiKey = Environment.GetEnvironmentVariable("ApiKey");
            _checksumKey = Environment.GetEnvironmentVariable("ChecksumKey");
        }

        public async Task<string> CreatePaymentAsync(decimal amount, string orderId, string description)
        {
            var requestUrl = "https://api.payos.io/v1/payments";

            var paymentRequest = new
            {
                clientId = _clientId,
                apiKey = _apiKey,
                checksum = CalculateChecksum(orderId, amount),
                amount = amount,
                orderId = orderId,
                description = description,
            };

            var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<PayOSPaymentResponseDTO>(jsonResponse);

            return responseObject?.QrCodeUrl;
        }

        private string CalculateChecksum(string orderId, decimal amount)
        {
            var rawData = $"{_clientId}{orderId}{amount}{_checksumKey}";
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
