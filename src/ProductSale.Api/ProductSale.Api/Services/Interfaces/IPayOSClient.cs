using ProductSale.Data.DTO.RequestModel;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IPayOSClient
    {
        Task<string> PayOSAsync(PayOSPaymentRequestDTO request);
        Task<object> CancelPayOSAsync(long orderCode, string reason = "");
        //Task<PayOSPaymentResponseDTO> VerifyPaymentAsync(string paymentId);
    }
}
