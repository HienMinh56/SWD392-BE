using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;
using System.Security.Claims;

namespace SWD392_BE.API.Controllers
{
    [Route("api/v1/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        #region Get All orders
        /// <summary>
        /// Get list of order by filter
        /// </summary>
        /// <returns>A list of orders</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders(string? userId, string? userName, DateTime? createdDate, int? status, string? storeName, string? sessionId)
        {
            var result = await _orderService.getOrders(userId, userName, createdDate, status, storeName, sessionId);

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

            var result = await _orderService.getTotalOrderAmount(startDate, endDate);

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
                    result = await _orderService.getOrderAmountPerDayInMonth(year, month);
                    break;

                case "week":
                    if (month == 0) return BadRequest(new { message = "Month is required for type 'week'." });
                    result = await _orderService.getOrderAmountPerWeekInMonth(year, month);
                    break;

                case "month":
                    result = await _orderService.getOrderAmountPerMonthInYear(year);
                    break;

                default:
                    return BadRequest(new { message = "Invalid type. Valid types are 'day', 'week', 'month'." });
            }

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Create Order
        /// <summary>
        /// Create a new order with food items and quantities.
        /// </summary>
        /// <param name="foodItems">A list of food items and their quantities.</param>
        /// <returns>The result of the order creation process.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<FoodItemModel> foodItems)
        {
            if (foodItems == null || !foodItems.Any())
            {
                return BadRequest(new { message = "Food items list cannot be empty." });
            }

            // Convert the incoming model to the expected tuple format for the service layer
            var orderItems = foodItems.Select(fi => (fi.FoodId, fi.Quantity)).ToList();

            var result = await _orderService.CreateOrderAsync(orderItems);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Update Order Status
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, int status)
        {
            var currentUser = HttpContext.User;
            var result = await _orderService.updateOrderStatus(orderId, status, currentUser);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Get Total Order vs User
        /// <summary>
        /// Get the total Order vs User in the system.
        /// </summary>
        /// <returns>The total orders and user.</returns>
        [HttpGet("dataDashboard")]
        public async Task<IActionResult> GetTotalOrderAndUserCount()
        {
            var orderResult = await _orderService.GetTotalOrderCountAsync();
            var userCount = await _userService.GetTotalUserCountAsync();

            if (orderResult.IsSuccess)
            {
                var result = new
                {
                    TotalOrders = orderResult.Data,
                    TotalUsers = userCount
                };
                return Ok(new ResultModel
                {
                    Data = result,
                    IsSuccess = true,
                    Message = "Success",
                    Code = 200
                });
            }
            else
            {
                return BadRequest(orderResult);
            }
        }
        #endregion
    }
}
