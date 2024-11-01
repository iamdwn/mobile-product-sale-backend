using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.Models;

namespace ProductSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("get-vietqr/{paymentId}")]
        public async Task<IActionResult> GetVietQR(int paymentId)
        {
            var payment = await _paymentService.GetPaymentById(paymentId);
            if (payment == null) return NotFound();

            string qrUrl = _paymentService.GenerateVietQRUrl(payment);
            return Ok(new { qrUrl });
        }

        [HttpPost]
        public async Task<Payment> CreatePayment(PaymentReq req)
        {
            return await _paymentService.CreatePayment(req);
        }

        [HttpPut]
        public async Task UpdatePayment(PaymentReq req)
        {
            await _paymentService.CreatePayment(req);
        }

        [HttpDelete]
        public async Task RemovePayment(int paymentId)
        {
            await _paymentService.RemovePayment(paymentId);
        }

        [HttpPost("complete-payment")]
        public async Task CompletePayment(int paymentId)
        {
            await _paymentService.CompletePayment(paymentId);
        }
    }
}
