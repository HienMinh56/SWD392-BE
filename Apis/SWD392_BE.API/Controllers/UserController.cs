using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;

using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWD392_BE.API.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList(int? status, string? campusName)
        {
            var result = await _userService.GetUserList(status, campusName);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserReqModel request)
        {
            var currentUser = HttpContext.User;
            var result = await _userService.DeleteUser(request, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<ResultModel>> UpdateUser(string userId, UpdateUserViewModel model)
        {
            try
            {
                var currentUser = HttpContext.User;
                var updateResult = await _userService.UpdateUser(userId, model, currentUser);
                var result = new ResultModel
                {
                    IsSuccess = true,
                    Code = 200,
                    Data = updateResult
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new ResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = ex.Message
                };
                return StatusCode(500, result);
            }
        }

        [HttpGet("{keyword}")]
        public async Task<ActionResult<ResultModel>> SearchUserByKeyword(string keyword)
        {
            var result = await _userService.SearchUserByKeyword(keyword);
            return result.IsSuccess ? Ok(result) : BadRequest(result);

        }

        [HttpGet("ascending")]
        public async Task<IActionResult> GetUsersSortedByCreatedDateAscending()
        {
            var result = await _userService.GetUsersSortedByCreatedDateAscending();
            return result.IsSuccess ? Ok(result) : StatusCode(result.Code, result);
        }

        [HttpGet("descending")]
        public async Task<IActionResult> GetUsersSortedByCreatedDateDescending()
        {
            var result = await _userService.GetUsersSortedByCreatedDateDescending();
            return result.IsSuccess ? Ok(result) : StatusCode(result.Code, result);
        }
    }
}

