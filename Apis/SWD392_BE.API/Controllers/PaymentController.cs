using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.ViewModels.PaymentModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
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

        [HttpPost("create-payment-url")]
        public IActionResult CreatePaymentUrl([FromBody] VnPayPaymentRequest model)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == null)
            {
                return BadRequest("Unable to determine IP address.");
            }

            var paymentUrl = _vnPayService.CreatePaymentUrl(model, ipAddress);
            return Ok(new { paymentUrl });
        }

        [HttpGet("vnpay_return")]
        public async Task<IActionResult> VnPayReturn()
        {
            SortedList<string, string> responseData = new SortedList<string, string>();
            foreach (var key in Request.Query.Keys)
            {
                responseData.Add(key, Request.Query[key]);
            }

            if (responseData.ContainsKey("vnp_SecureHash"))
            {
                string vnp_SecureHash = Request.Query["vnp_SecureHash"];
                bool isValidSignature = _vnPayService.ValidateSignature(vnp_SecureHash, responseData);

                if (isValidSignature)
                {
                    var result = await _vnPayService.ProcessPaymentResponse(responseData);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new ResultModel { IsSuccess = false, Code = 97, Message = "Invalid signature" });
                }
            }
            else
            {
                return BadRequest(new ResultModel { IsSuccess = false, Code = 97, Message = "Missing secure hash" });
            }
        }
    }
}