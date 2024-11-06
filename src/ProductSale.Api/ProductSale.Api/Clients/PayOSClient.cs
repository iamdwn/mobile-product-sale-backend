using Newtonsoft.Json;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using System.Text;

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

        //public async Task<string> CreatePaymentAsync(decimal amount, string orderId, string description)
        //{
        //    var requestUrl = "https://product-sale.iamdwn.dev/api/Payment/create";

        //    var paymentRequest = new
        //    {
        //        clientId = _clientId,
        //        apiKey = _apiKey,
        //        checksum = CalculateChecksum(orderId, amount),
        //        amount = amount,
        //        orderId = orderId,
        //        description = description,
        //    };

        //    var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(requestUrl, content);
        //    response.EnsureSuccessStatusCode();

        //    var jsonResponse = await response.Content.ReadAsStringAsync();
        //    var responseObject = JsonSerializer.Deserialize<PayOSPaymentResponseDTO>(jsonResponse);

        //    return responseObject?.QrCodeUrl;
        //}

        //private string CalculateChecksum(string orderId, decimal amount)
        //{
        //    var rawData = $"{_clientId}{orderId}{amount}{_checksumKey}";
        //    using var sha256 = System.Security.Cryptography.SHA256.Create();
        //    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        //    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //}

        public async Task<PayOSPaymentResponseDTO> CreatePaymentRequest(PayOSPaymentRequestDTO paymentRequest)
        {
            var url = "https://api-merchant.payos.vn/v2/payment-requests";

            //paymentRequest.Checksum = GenerateChecksum(paymentRequest);

            // Chuyển đổi đối tượng yêu cầu thành JSON
            var jsonData = JsonConvert.SerializeObject(paymentRequest);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Thêm các header cần thiết cho API (chẳng hạn như API Key, Client ID)
            content.Headers.Add("ClientId", _clientId);
            content.Headers.Add("ApiKey", _apiKey);

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PayOSPaymentResponseDTO>(responseContent);
            }

            throw new Exception("Failed to create payment request.");
        }
    }
}
