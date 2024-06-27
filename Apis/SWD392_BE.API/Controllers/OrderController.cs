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
        /// <param name="startDate">The start date for the calculation.</param>
        /// <param name="endDate">The end date for the calculation.</param>
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

        #region Get data for chart 
        /// <summary>
        /// Get data for chart by day, week, month
        /// </summary>
        /// <param name="type">The type of data to retrieve: day, week, month</param>
        /// <param name="year">The year for the calculation </param>
        /// <param name="month">The month for the calculation (required for day and week types)</param>
        /// <returns>Amount 7 day in week, all week in month, all month in year</returns>
        [HttpGet("data-chart")]
        public async Task<IActionResult> GetOrderAmounts(string type, int year, int month = 0)
        {
            ResultModel result;

            switch (type.ToLower())
            {
                case "day":
                    if (month == 0) return BadRequest(new { message = "Month is required for type 'day'." });
                    result = await _order.getOrderAmountPerDayInMonth(year, month);
                    break;

                case "week":
                    if (month == 0) return BadRequest(new { message = "Month is required for type 'week'." });
                    result = await _order.getOrderAmountPerWeekInMonth(year, month);
                    break;

                case "month":
                    result = await _order.getOrderAmountPerMonthInYear(year);
                    break;

                default:
                    return BadRequest(new { message = "Invalid type. Valid types are 'day', 'week', 'month'." });
            }

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion
    }
}
