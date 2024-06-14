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
    [Route("api/v1/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;



        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;

        }

        #region Add a new user
        /// <summary>
        /// Add a new user by admin
        /// </summary>
        /// <returns>Status of action</returns>
        [HttpPost]
        public async Task<ActionResult<ResultModel>> AddNewUser(RegisterReqModel model)
        {
            try
            {
                var currentUser = HttpContext.User;
                var registerResult = await _accountService.AddNewUser(model, currentUser);
                var result = new ResultModel
                {
                    IsSuccess = true,
                    Code = 200,
                    Data = registerResult
                };
                return Ok(result); // Return 200 OK with the registration response
            }
            catch (Exception ex)
            {
                var result = new ResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = ex.Message
                }; // Return 400 Bad Request with the error message
                return StatusCode(500, result);
            }
        }
        #endregion

        #region Regis Account
        /// <summary>
        /// Register a new account
        /// </summary>
        /// <returns>Status of action</returns>
        [HttpPost("mobile")]
        public async Task<ActionResult<ResultModel>> MobileRegister(CreateMobileViewModel model)
        {
            try
            {


                var mobileRegisterResult = await _accountService.MobileRegister(model);
                var result = new ResultModel
                {
                    IsSuccess = true,
                    Code = 200,
                    Data = mobileRegisterResult
                };
                return Ok(result);// Return the response from the common Register method
            }
            catch (Exception ex)
            {
                var result = new ResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = ex.Message
                }; // Return 400 Bad Request with the error message
                return StatusCode(500, result);
            }
        }
        #endregion
    }

}
