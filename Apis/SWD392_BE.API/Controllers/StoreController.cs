using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.ViewModels.StoreModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;

namespace SWD392_BE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet("GetStores")]
        public async Task<IActionResult> GetListStore()
        {
            var result = await _storeService.getListStore();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("AddStore")]
        public async Task<IActionResult> AddStore(StoreViewModel storeReq)
        {
            var currentUser = HttpContext.User;
            var result = await _storeService.addStore(storeReq, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{storeId}")]
        public async Task<IActionResult> UpdateStore(string storeId,  UpdateStoreViewModel storeReq)
        {
            var currentUser = HttpContext.User;
            var result = await _storeService.UpdateStoreAsync(storeId, storeReq, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
