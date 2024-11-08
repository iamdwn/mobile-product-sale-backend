using ProductSale.Data.DTO.RequestModel;

namespace ProductSale.Api.Clients.Interfaces
{
    public interface IPayOSClient
    {
        Task<string> PayOSAsync(PayOSPaymentRequestDTO request);
        Task<object> CancelPayOSAsync(long orderCode, string reason = "");
        Task<string> CheckPayOSAsync(long orderCode);
        //Task<PayOSPaymentResponseDTO> VerifyPaymentAsync(string paymentId);
    }
}
