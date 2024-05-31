using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IAccountService _accountService;


        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            bool isLoginSuccessful = await _accountService.Login(email, password);

            if (!isLoginSuccessful)
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isAddUserSuccessful = await _accountService.Register(registerModel.UserName, registerModel.Password, registerModel.Email, registerModel.Phone, registerModel.CampusId, registerModel.Name);

            if (isAddUserSuccessful)
            {
                return Ok(new { message = "Add user successfully." });
            }
            else
            {
                return Conflict(new { message = "Username or email already exists." });
            }
        }
    }

}
