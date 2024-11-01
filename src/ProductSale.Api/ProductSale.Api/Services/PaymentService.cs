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

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<Payment> CreatePayment(PaymentReq req)
        {
            var newPayment = new Payment()
            {
                OrderId = req.OrderId,
                Amount = req.Amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.PENDING.ToString()
            };

            _unitOfWork.PaymentRepository.Insert(newPayment);
            _unitOfWork.Save();

            return newPayment;
        }

        public string GenerateVietQRUrl(Payment payment)
        {
            string bankId = PaymentMethod.MBBANK.ToString();
            string templateId = Environment.GetEnvironmentVariable("TemplateId");
            string bankAccount = Environment.GetEnvironmentVariable("BankAccount");
            string accountName = Environment.GetEnvironmentVariable("AccountName");
            string amount = payment.Amount.ToString("F0");

            string qrUrl = $"https://api.vietqr.io/image/{bankId}-{bankAccount}-{templateId}.jpg?accountName={bankAccount}&amount={amount}&addInfo=test";

            return qrUrl;
        }

        public async Task<Payment> GetPaymentById(int paymentId)
        {
            return _unitOfWork.PaymentRepository.GetByID(paymentId);
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

        public async Task UpdatePayment(PaymentReq req)
        {
            var existingPayment = GetPaymentById(req.PaymentId);
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
