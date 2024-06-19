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
        public async Task<IActionResult> GetOrders( string? userId, string? userName, DateTime? createdDate, int? status, string? storeName, string? sessionId)
        {
            var result = await _order.getOrders(userId, userName, createdDate, status, storeName, sessionId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Get Total Order Amount
        /// <summary>
        /// Get the total order amount between specified dates
        /// </summary>
        /// <param name="startDate">The start date for the calculation (required).</param>
        /// <param name="endDate">The end date for the calculation (required).</param>
        /// <returns>The total order amount</returns>

        [HttpGet("totalAmount")]
        public async Task<IActionResult> GetTotalOrderAmount(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new { message = "Start date must be less than or equal end date." });
            }

            var result = await _order.getTotalOrderAmount(startDate, endDate);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion
    }
}
