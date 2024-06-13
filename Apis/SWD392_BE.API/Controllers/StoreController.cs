using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.StoreModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;

namespace SWD392_BE.API.Controllers
{
    [Route("api/v1/store")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStoresByStatusAreaAndSession([FromQuery] int? status, [FromQuery] string? areaName, [FromQuery] string? sessionId)
        {
            var stores = await _storeService.GetStoresAsync(status, areaName, sessionId);
            return Ok(stores);
        }

        

        [HttpPost]
        public async Task<IActionResult> AddStore(StoreViewModel storeReq)
        {
            var currentUser = HttpContext.User;
            var result = await _storeService.addStore(storeReq, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStore(string storeId,  UpdateStoreViewModel storeReq)
        {
            var currentUser = HttpContext.User;
            var result = await _storeService.UpdateStoreAsync(storeId, storeReq, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteStore([FromBody] DeleteStoreReqModel request)
        {
            var currentUser = HttpContext.User;
            var result = await _storeService.DeleteStore(request, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
