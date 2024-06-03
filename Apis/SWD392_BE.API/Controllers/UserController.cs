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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("view-all-users")]
        public async Task<IActionResult> ViewAllUsers()
        {
            var result = await _userService.ViewAllUsers();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("delete-user")]
        public async Task<IActionResult> DeleteUser([FromBody] string userName)
        {
            var result = await _userService.DeleteUser(userName);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        [HttpPut("updateUser")]
        public async Task<ActionResult<ResultModel>> UpdateUser(UpdateUserViewModel user)
        {
            try
            {
                var updateResult = await _userService.UpdateUser(user);
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
    }
}

