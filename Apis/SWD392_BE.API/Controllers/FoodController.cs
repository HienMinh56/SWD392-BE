﻿using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.StoreModel;
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

        [HttpGet("GetFoods")]
        public async Task<IActionResult> GetListFood()
        {
            var result = await _foodService.getListFood();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("AddFood")]
        public async Task<IActionResult> AddFood(string storeId, List<FoodViewModel> foods)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.addFood(storeId, foods, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut("{FoodId}")]
        public async Task<IActionResult> UpdateFood(string foodId, [FromBody] FoodViewModel model)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.updateFood(foodId, model, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}