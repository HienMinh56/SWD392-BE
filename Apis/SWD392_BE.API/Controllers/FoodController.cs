using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.ViewModels.FoodModel;
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

        [HttpGet("GetFoods")]
        public async Task<IActionResult> GetListFood(string storeId)
        {
            var result = await _foodService.getListFood(storeId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("filterFood")]
        public async Task<IActionResult> FilterFood([FromQuery] int? cate)
        {
            var foods = await _foodService.FilterFoodsAsync(cate);
            return Ok(foods);
        }
        [HttpPost("AddFood")]
        public async Task<IActionResult> AddFood(string storeId, List<List<FoodViewModel>> foodLists)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.addFood(storeId, foodLists, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut("UpdateFood")]
        public async Task<IActionResult> UpdateFood(string id, UpdateFoodViewModel model)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.UpdateFoodAsync(id, model, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPatch("deleteFood")]
        public async Task<IActionResult> DeleteFood([FromBody] DeleteFoodReqModel request)
        {
            var currentUser = HttpContext.User;
            var result = await _foodService.DeleteFood(request, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}