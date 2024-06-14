﻿using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.PageModel;
using SWD392_BE.Repositories.ViewModels.StoreModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;

namespace SWD392_BE.API.Controllers
{
    [Route("api/v1/food")]
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
        public async Task<IActionResult> GetListFood(string storeId, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _foodService.GetListFood(storeId, pageIndex, pageSize);

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

        #region Filter food
        /// <summary>
        /// Get list of foods by filter
        /// </summary>
        /// <returns>A list of foods</returns>
        [HttpGet("cate")]
        public async Task<IActionResult> FilterFood([FromQuery] int? cate)
        {
            var foods = await _foodService.FilterFoodsAsync(cate);
            return Ok(foods);
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