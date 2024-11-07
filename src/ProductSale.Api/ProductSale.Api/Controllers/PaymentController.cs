using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.DTO.RequestModel;

namespace ProductSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IPaymentService paymentService, IUnitOfWork unitOfWork)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
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
            try
            {
                var order = _unitOfWork.OrderRepository.GetByID(request.OrderId);
                if (order == null) return NotFound();

                string url = await _paymentService.CreatePayOSPaymentAsync(request);
                //return Ok(new { qrUrl });
                using HttpClient client = new HttpClient();
                string html = await client.GetStringAsync(url);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode qrNode = doc.DocumentNode.SelectSingleNode("//img[@alt='qrcode']");

                if (qrNode != null)
                {
                    string qrUrl = qrNode.GetAttributeValue("src", "");
                    string[] parts = qrUrl.Split(new string[] { "&amp;" }, StringSplitOptions.None);
                    string decodedUrl = string.Join("&", parts);
                    return Ok(new { decodedUrl });
                }
                else
                {
                    Console.WriteLine("Not found QRCode.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return Ok("");
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPayOSPayment(long orderCode, string reason = "")
        {
            var cancelResponse = await _paymentService.CancelPayOSPaymentAsync(orderCode, reason);
            return Ok(cancelResponse);
        }
    }
}
