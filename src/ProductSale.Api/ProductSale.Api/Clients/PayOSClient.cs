using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using ProductSale.Api.Clients.Interfaces;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using System.Text;

namespace ProductSale.Api.Clients
{
    public class PayOSClient : IPayOSClient
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

        public async Task<PayOSPaymentResponseDTO> CreatePaymentRequest(PayOSPaymentRequestDTO paymentRequest)
        {
            var url = "https://api-merchant.payos.vn/v2/payment-requests";

            var jsonData = JsonConvert.SerializeObject(paymentRequest);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            content.Headers.Add("Client ID", _clientId);
            content.Headers.Add("Api Key", _apiKey);

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PayOSPaymentResponseDTO>(responseContent);
            }

            throw new Exception("Failed to create payment request.");
        }

        public async Task<string> PayOSAsync(PayOSPaymentRequestDTO req)
        {
            //var url = "https://api-merchant.payos.vn/v2/payment-requests";

            PayOS payOS = new PayOS(_clientId, _apiKey, _checksumKey);

            //PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation((long)req.OrderId);

            //if (paymentLinkInformation != null)
            //{
            //    return paymentLinkInformation.canceledAt;
            //}

            long orderCode = GenerateRandomPaymentCode();

            long.TryParse(req.TransactionId, out long transId);

            PaymentData paymentData = new PaymentData(
                transId,
                (int)req.Amount,
                req.Note,
                new List<ItemData>(),
                "https://product-sale.iamdwn.dev/cancel",
                "https://product-sale.iamdwn.dev/"
              );

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
            return createPayment.checkoutUrl;
        }

        public async Task<object> CancelPayOSAsync(long orderCode, string reason = "")
        {
            try
            {
                PayOS payOS = new PayOS(_clientId, _apiKey, _checksumKey);
                PaymentLinkInformation canceledInfo = await payOS.cancelPaymentLink(orderCode, reason);
                if (canceledInfo.status.Equals("canceled")) throw new Exception("This transaction is canceled");
                return new { Success = true, Message = "Canceled successfully", Data = canceledInfo };
            }
            catch (Exception ex)
            {
                return new { Success = false, Message = "Canceled failed", Error = ex.Message };
            }
        }

        public long GenerateRandomPaymentCode()
        {
            Random random = new Random();

            int lower = random.Next(10000000, 99999999);
            int upper = random.Next(10000000, 99999999);

            long randomLong = ((long)lower * 100000000) + upper;

            return randomLong;
        }

        public async Task<string> CheckPayOSAsync(long orderCode)
        {
            PayOS payOS = new PayOS(_clientId, _apiKey, _checksumKey);

            PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(orderCode);

            if (paymentLinkInformation == null) return "Not found";
            switch (paymentLinkInformation.status)
            {
                case "PENDING":
                    return "PENDING";
                case "CANCELED":
                    return "CANCELED";
                case "PAID":
                    return "PAID";
                default: return "NOT FOUND";
            }
        }
    }
}
