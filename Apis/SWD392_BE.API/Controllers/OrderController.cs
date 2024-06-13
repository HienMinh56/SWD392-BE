using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Services.Interfaces;

namespace SWD392_BE.API.Controllers
{
        [Route("api/v1/order")]
        [ApiController]
        public class OrderController : ControllerBase
        {
            private readonly IOrderService _order;

            public OrderController(IOrderService order)
            {
                _order = order;
            }

        [HttpGet]
        public async Task<IActionResult> getOrder()
        {
            var result = await _order.getOrder();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
