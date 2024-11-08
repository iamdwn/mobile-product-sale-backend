using ProductSale.Api.Clients.Interfaces;
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
            var existingOrder = _unitOfWork.OrderRepository.Get(
                filter: o => o.OrderId.Equals(req.OrderId),
                noTracking: true
                ).FirstOrDefault();
            if (existingOrder?.CartId == null) return "";

            var existingPayment = _unitOfWork.PaymentRepository.Get(
               filter: o => o.OrderId.Equals(req.OrderId),
               noTracking: true
               ).FirstOrDefault();
            if (existingPayment != null) return "https://product-sale.iamdwn.dev/failure";

            var existingCart = _unitOfWork.CartRepository.GetByID(existingOrder.CartId);
            if (existingCart == null) return "https://product-sale.iamdwn.dev/failure";

            req.Amount = existingCart.TotalPrice;

            var orderCode = GenerateRandomPaymentCode();

            req.TransactionId = orderCode.ToString();

            var qrCodeUrl = await _payOSClient.PayOSAsync(req);
            if (string.IsNullOrEmpty(qrCodeUrl)) return "https://product-sale.iamdwn.dev/failure";

            _unitOfWork.PaymentRepository.Insert(new Payment
            {
                OrderId = req.OrderId,
                Amount = req.Amount,
                PaymentStatus = PaymentStatus.PENDING.ToString(),
                PaymentDate = DateTime.Now,
                TransactionId = req.TransactionId
            });

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

        public async Task<string> CheckStatusPayOSPaymentAsync(int orderId)
        {
            var existingPayment = _unitOfWork.PaymentRepository.Get(
                    filter: o => o.OrderId.Equals(orderId),
                    noTracking: true
                    ).FirstOrDefault();

            long.TryParse(existingPayment.TransactionId, out long transId);

            return await _payOSClient.CheckPayOSAsync(transId);
        }

        public long GenerateRandomPaymentCode()
        {
            Random random = new Random();

            int lower = random.Next(10000000, 99999999);
            int upper = random.Next(10000000, 99999999);

            long randomLong = ((long)lower * 100000000) + upper;

            return randomLong;
        }
    }
}
