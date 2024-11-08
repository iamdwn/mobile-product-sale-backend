using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task UpdatePayment(PayOSPaymentRequestDTO req);
        Task RemovePayment(int paymentId);
        Task<Payment> GetPaymentById(int paymentId);
        string GenerateVietQRUrl(Payment payment);
        Task CompletePayment(int paymentId);
        Task<bool> GetPaymentStatus(int paymentId);
        Task<string> CreatePayOSPaymentAsync(PayOSPaymentRequestDTO request);
        Task<object> CancelPayOSPaymentAsync(long orderCode, string reason = "");
        Task<string> CheckStatusPayOSPaymentAsync(int orderId);
        Task<int> GetPaymentId(int orderId);
    }
}
