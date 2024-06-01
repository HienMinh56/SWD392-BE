using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Helper;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWD392_BE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly JWTTokenHelper _jWTToken;


        public AccountController(IAccountService accountService, JWTTokenHelper jWTToken)
        {
            _accountService = accountService;
            _jWTToken = jWTToken;
        }

        public JWTTokenHelper JWTToken { get; }

        [HttpPost("login")]
        public async Task<ActionResult<ResultModel>> Login(LoginReqModel user)
        {
            try
            {
                var loginResult = await _accountService.Login(user);
                var result = new ResultModel
                {
                    IsSuccess = true,
                    Code = 200,
                    Data = loginResult
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

        [HttpPost("AddNewUser")]
        public async Task<IActionResult> WebRegister(RegisterReqModel model)
        {
            try
            {
                var response = await _accountService.AddNewUser(model);
                return Ok("Register Succcessfully"); // Return 200 OK with the registration response
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Return 400 Bad Request with the error message
            }
        }

        [HttpPost("mobileRegister")]
        public async Task<IActionResult> MobileRegister(RegisterReqModel model)
        {
            try
            {
                

                var response = await _accountService.MobileRegister(model);


                // Return the response from the common Register method
                return Ok("Register Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Return 400 Bad Request with the error message
            }
        }
    }

}
