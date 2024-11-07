using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.Enums;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayOSClient _payOSClient;

        public PaymentService(IUnitOfWork unitOfWork, IPayOSClient payOSClient)
        {
            _unitOfWork = unitOfWork;
            _payOSClient = payOSClient;
        }

        public async Task CompletePayment(int paymentId)
        {
            var existingPayment = GetPaymentById(paymentId);
            if (existingPayment == null)
            {
                throw new Exception("Not Found");
            }

            var updatedPayment = existingPayment.Result;
            updatedPayment.PaymentDate = DateTime.Now;
            updatedPayment.PaymentStatus = PaymentStatus.COMPLETED.ToString();

            _unitOfWork.PaymentRepository.Update(updatedPayment);
            _unitOfWork.Save();
        }

        public string GenerateVietQRUrl(Payment payment)
        {
            string bankId = PaymentMethod.MBBANK.ToString();
            string templateId = Environment.GetEnvironmentVariable("TemplateId");
            string bankAccount = Environment.GetEnvironmentVariable("BankAccount");
            string accountName = Environment.GetEnvironmentVariable("AccountName");

            string amount = payment.Amount.ToString("F0");

            string qrUrl = $"https://api.vietqr.io/image/{bankId}-{bankAccount}-{templateId}.jpg?amount={amount}";

            return qrUrl;
        }

        public async Task<Payment> GetPaymentById(int paymentId)
        {
            return _unitOfWork.PaymentRepository.GetByID(paymentId);
        }

        public async Task<bool> GetPaymentStatus(int paymentId)
        {
            var payment = GetPaymentById(paymentId).Result;
            return payment.PaymentStatus.Equals(PaymentStatus.COMPLETED.ToString())
                ? true : payment.PaymentStatus.Equals(PaymentStatus.FAILED.ToString())
                ? true : false;
        }

        public async Task<string> CreatePayOSPaymentAsync(PayOSPaymentRequestDTO req)
        {
            var existingOrder = _unitOfWork.OrderRepository.GetByID(req.OrderId);

            if (existingOrder == null)
            {
                return "";
            }

            var existingCart = _unitOfWork.CartRepository.GetByID(existingOrder.CartId);

            if (existingCart == null)
            {
                return "";
            }

            req.Amount = existingCart.TotalPrice;

            var qrCodeUrl = await _payOSClient.PayOSAsync(req);

            if (string.IsNullOrEmpty(qrCodeUrl))
            {
                return "";
            }

            var payment = new Payment
            {
                OrderId = req.OrderId,
                Amount = req.Amount,
                PaymentStatus = PaymentStatus.PENDING.ToString(),
                PaymentDate = DateTime.Now
            };
            _unitOfWork.PaymentRepository.Insert(payment);
            _unitOfWork.Save();

            return qrCodeUrl;
        }

        public async Task<object> CancelPayOSPaymentAsync(long orderCode, string reason = "")
        {
            return await _payOSClient.CancelPayOSAsync(orderCode, reason);
        }

        public async Task RemovePayment(int paymentId)
        {
            var existingPayment = GetPaymentById(paymentId);
            if (existingPayment == null)
            {
                throw new Exception("Not Found");
            }

            var deletedPayment = existingPayment.Result;
            deletedPayment.PaymentDate = DateTime.Now;
            deletedPayment.PaymentStatus = PaymentStatus.FAILED.ToString();

            _unitOfWork.PaymentRepository.Update(deletedPayment);
            _unitOfWork.Save();
        }

        public async Task UpdatePayment(PayOSPaymentRequestDTO req)
        {
            var existingPayment = GetPaymentById(req.PaymentId ?? 0);
            if (existingPayment == null)
            {
                throw new Exception("Not Found");
            }

            var updatedPayment = existingPayment.Result;
            updatedPayment.OrderId = req.OrderId ?? updatedPayment.OrderId;
            updatedPayment.Amount = req.Amount;
            updatedPayment.PaymentDate = DateTime.Now;

            _unitOfWork.PaymentRepository.Update(updatedPayment);
            _unitOfWork.Save();
        }
    }
}
