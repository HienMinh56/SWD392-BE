using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.ViewModels.PaymentModel;
using SWD392_BE.Services.Interfaces;

namespace SWD392_BE.API.Controllers
{

    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("vnpay-payment")]
        public IActionResult CreatePayment([FromBody] VnPayPaymentRequest model)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, ipAddress);
            return Ok(new { paymentUrl });
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            SortedList<string, string> responseData = new SortedList<string, string>();

            foreach (string key in Request.Query.Keys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    responseData.Add(key, Request.Query[key]);
                }
            }

            string inputHash = Request.Query["vnp_SecureHash"];
            if (_vnPayService.ValidateSignature(inputHash, responseData))
            {
                var result = await _vnPayService.ProcessPaymentResponse(responseData);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }

            return BadRequest(new { message = "Invalid signature" });
        }
    }
}
