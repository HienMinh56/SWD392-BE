﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Repositories.Entities;
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


        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqModel User)
        {
            ResultModel result = await _accountService.Login(User);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }

}