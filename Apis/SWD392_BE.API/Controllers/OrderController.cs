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
        /// Get list of orders
        /// </summary>
        /// <returns>A list of orders</returns>
        [HttpGet]
        public async Task<IActionResult> getAllOrder()
        {
            var result = await _order.getAllOrder();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion
        #region Get orders by userId
        /// <summary>
        /// Get list of orders by userId
        /// </summary>
        /// <returns>A list of orders</returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            var currentUser = HttpContext.User;
            var result = await _order.GetOrderByUserIdAsync(userId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.Code, new ResultModel
                {
                    IsSuccess = result.IsSuccess,
                    Code = result.Code,
                    Message = result.Message
                });
            }

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = 200,
                Data = result.Data
            });
        }
        #endregion
    }
}
