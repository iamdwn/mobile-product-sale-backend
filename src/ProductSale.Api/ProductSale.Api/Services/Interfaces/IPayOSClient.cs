using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IPayOSClient
    {
        Task<PayOSPaymentResponseDTO> CreatePaymentAsync(PaymentReq request);
        Task<PayOSPaymentResponseDTO> VerifyPaymentAsync(string paymentId);
    }
}
