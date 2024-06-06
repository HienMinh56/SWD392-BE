using AutoMapper;
using Microsoft.Extensions.Configuration;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Helper;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Sercurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepo, IConfiguration configuration, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _configuration = configuration;
            _mapper = mapper;
        }


        public async Task<ResultModel> AddNewUser(RegisterReqModel model, ClaimsPrincipal userCreate)
        {
            ResultModel result = new ResultModel();
            try
            {
                // Check if passwords match
                if (model.Password != model.ConfirmPassword)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Passwords do not match";
                    return result;
                }

                // Check if user already exists
                var existingUser = _accountRepo.Get(u => u.UserName == model.UserName || u.Email == model.Email || u.Phone == model.Phone);
                if (existingUser != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "User already exists";
                    return result;
                }

                // Map the request model to the user entity
                var user = _mapper.Map<User>(model);

                // Generate the next user ID
                user.UserId = await _accountRepo.GetNextUserId();

                // Hash the password using PasswordHasher
                user.Password = PasswordHasher.HashPassword(model.Password);

                // Set other properties (e.g., CreatedDate, Status, etc.)
                user.CreatedBy = userCreate.FindFirst("UserName")?.Value;
                user.CreatedDate = DateTime.UtcNow;
                user.Status = 1; // Assuming 1 is the default status for an active user

                // Add the user to the repository and save changes
                _accountRepo.Add(user);
                await _accountRepo.SaveChangesAsync();
                result.IsSuccess = true;
                result.Code = 200;
                return result;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = e.Message;
                return result;
            }
        }


        public async Task<ResultModel> MobileRegister(RegisterReqModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                // Check if passwords match
                if (model.Password != model.ConfirmPassword)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Passwords do not match";
                    return result;
                }

                // Check if user already exists
                var existingUser = _accountRepo.Get(u => u.UserName == model.UserName || u.Email == model.Email || u.Phone == model.Phone);
                if (existingUser != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "User already exists";
                    return result;
                }

                // Map the request model to the user entity
                var user = _mapper.Map<User>(model);

                // Generate the next user ID
                user.UserId = await _accountRepo.GetNextUserId();

                // Hash the password using PasswordHasher
                user.Password = PasswordHasher.HashPassword(model.Password);

                // Set other properties (e.g., CreatedDate, Status, etc.)

                user.CreatedDate = DateTime.UtcNow;
                user.Role = 2; // Set role to 2 by default
                user.Status = 1; // Assuming 1 is the default status for an active user

                // Add the user to the repository and save changes
                _accountRepo.Add(user);
                await _accountRepo.SaveChangesAsync();
                result.IsSuccess = true;
                result.Code = 200;
                return result;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = e.Message;
                return result;
            }
        }
    }
}
