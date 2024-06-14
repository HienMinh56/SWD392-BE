using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.PageModel;
using SWD392_BE.Repositories.ViewModels.StoreModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;

namespace SWD392_BE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }
        #region Get list foods
        /// <summary>
        /// Get list of foods
        /// </summary>
        /// <returns>A list of foods</returns>
        [HttpGet]
        public async Task<IActionResult> GetListFoodAsync([FromQuery] string storeId, [FromQuery] int pageIndex , [FromQuery] int pageSize , [FromQuery] int? cate)
        {
            var result = await _foodService.GetListFoodsAsync(storeId, pageIndex, pageSize, cate);

            if (result.IsSuccess)
            {
                // If successful, return the paged result
                var pagedResult = (PagedResultViewModel<Food>)result.Data;
                return Ok(new
                {
                    TotalItems = pagedResult.TotalItems,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize,
                    TotalPages = pagedResult.TotalPages,
                    Items = pagedResult.Items
                });
            }
            else
            {
                // If failed, return BadRequest with the error message
                return BadRequest(result.Message);
            }
        }
        #endregion
        #region Add foods
        /// <summary>
        /// Add new foods into store
        /// </summary>
        /// <returns>Status of action</returns>
        [HttpPost]
        public async Task<IActionResult> AddFood(string storeId, List<List<FoodViewModel>> foodLists)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.addFood(storeId, foodLists, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Update Food
        /// <summary>
        /// Update a food
        /// </summary>
        /// <returns>Status of action</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateFood(string id, UpdateFoodViewModel model)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.UpdateFoodAsync(id, model, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Delete food
        /// <summary>
        /// Delete a food
        /// </summary>
        /// <returns>Status of action</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteFood([FromBody] DeleteFoodReqModel request)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.DeleteFood(request, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion
    }
}