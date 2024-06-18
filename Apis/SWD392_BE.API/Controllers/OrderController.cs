using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;

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

        #region Get All orders
        /// <summary>
        /// Get list of order by filter
        /// </summary>
        /// <returns>A list of orders</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders( string? userId, DateTime? createdDate, int? status, string? storeName, string? sessionId)
        {
            var result = await _order.getOrders(userId, createdDate,status, storeName, sessionId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion
    }
}
