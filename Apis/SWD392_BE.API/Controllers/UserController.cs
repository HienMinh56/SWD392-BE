using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.PageModel;
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

        #region Get list user filter
        /// <summary>
        /// Get list of users by filter
        /// </summary>
        /// <returns>A list of users</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserList([FromQuery]int? status,[FromQuery] string? campusName, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _userService.GetUserList(status, campusName, pageIndex, pageSize);
            // return result.IsSuccess ? Ok(result) : BadRequest(result);
            if (result.IsSuccess)
            {
                // If successful, return the paged result
                var pagedResult = (PagedResultViewModel<ListUserViewModel>)result.Data;
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

        #region Delete user
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <returns>Status of action</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserReqModel request)
        {
            var currentUser = HttpContext.User;
            var result = await _userService.DeleteUser(request, currentUser);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Update User
        /// <summary>
        /// Update a user
        /// </summary>
        /// <returns>Status of action</returns>
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
        #endregion

        #region Search user
        /// <summary>
        /// Search user by keyword
        /// </summary>
        /// <returns>A list of users</returns>
        [HttpGet("{keyword}")]
        public async Task<ActionResult<ResultModel>> SearchUserByKeyword(string keyword)
        {
            var result = await _userService.SearchUserByKeyword(keyword);
            return result.IsSuccess ? Ok(result) : BadRequest(result);

        }
        #endregion

        #region Sort user ascending created date
        /// <summary>
        /// Get list of users by acsending created date 
        /// </summary>
        /// <returns>A list of users</returns>
        [HttpGet("ascending")]
        public async Task<IActionResult> GetUsersSortedByCreatedDateAscending()
        {
            var result = await _userService.GetUsersSortedByCreatedDateAscending();
            return result.IsSuccess ? Ok(result) : StatusCode(result.Code, result);
        }
        #endregion

        #region Sort user descending created date
        /// <summary>
        /// Get list of users by descending created date 
        /// </summary>
        /// <returns>A list of users</returns>
        [HttpGet("descending")]
        public async Task<IActionResult> GetUsersSortedByCreatedDateDescending()
        {
            var result = await _userService.GetUsersSortedByCreatedDateDescending();
            return result.IsSuccess ? Ok(result) : StatusCode(result.Code, result);
        }
        #endregion
    }
}

