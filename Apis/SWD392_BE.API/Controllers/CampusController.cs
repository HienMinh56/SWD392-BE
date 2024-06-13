﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_BE.Services.Interfaces;

namespace SWD392_BE.API.Controllers
{
    [Route("api/v1/campus")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private readonly ICampusService _campusService;

        public CampusController(ICampusService campusService)
        {
            _campusService = campusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListCampus()
        {
            var result = await _campusService.getListCampus();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}