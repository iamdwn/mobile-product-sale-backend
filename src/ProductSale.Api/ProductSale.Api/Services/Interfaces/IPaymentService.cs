using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(PaymentReq req);
        Task UpdatePayment(PaymentReq req);
        Task RemovePayment(int paymentId);
        Task<Payment> GetPaymentById(int paymentId);
        string GenerateVietQRUrl(Payment payment);
        Task CompletePayment(int paymentId);
        Task<bool> GetPaymentStatus(int paymentId);
    }
}
