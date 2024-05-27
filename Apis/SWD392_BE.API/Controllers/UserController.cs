using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;

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
    }
}
