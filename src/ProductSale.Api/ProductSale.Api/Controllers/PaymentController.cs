using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.DTO.RequestModel;

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

        [HttpGet("get-status/{paymentId}")]
        public async Task<bool> GetPaymentStatus(int paymentId)
        {
            return await _paymentService.GetPaymentStatus(paymentId);
        }

        [HttpPut]
        public async Task UpdatePayment(PayOSPaymentRequestDTO req)
        {
            await _paymentService.UpdatePayment(req);
        }

        [HttpDelete("{paymentId}")]
        public async Task RemovePayment(int paymentId)
        {
            await _paymentService.RemovePayment(paymentId);
        }

        [HttpPost("complete-payment/{paymentId}")]
        public async Task CompletePayment(int paymentId)
        {
            await _paymentService.CompletePayment(paymentId);
        }

        [HttpPost("payos")]
        public async Task<IActionResult> CreatePayOSPayment([FromBody] PayOSPaymentRequestDTO request)
        {
            var qrCodeUrl = await _paymentService.CreatePayOSPaymentAsync(request);
            return Ok(new { QrCodeUrl = qrCodeUrl });
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPayOSPayment(long orderCode, string reason = "")
        {
            var cancelResponse = await _paymentService.CancelPayOSPaymentAsync(orderCode, reason);
            return Ok(cancelResponse);
        }
    }
}
