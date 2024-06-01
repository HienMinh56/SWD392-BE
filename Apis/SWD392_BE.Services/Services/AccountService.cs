﻿using AutoMapper;
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
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly JWTTokenHelper _jWTTokenHelper;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepo, JWTTokenHelper jWTTokenHelper, IConfiguration configuration, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _jWTTokenHelper = jWTTokenHelper;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ResultModel> Login(LoginReqModel user)
        {
            ResultModel result = new ResultModel();
            try
            {
                var getUser = await _accountRepo.GetUserByUserName(user.UserName);
                if (getUser == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Email is not exist";
                    return result;
                }
                if (getUser.Status != 1 && getUser.Status != 2 && getUser.Status != 3)
                {
                    result.IsSuccess = false;
                    result.Code = (int)HttpStatusCode.Forbidden;
                    result.Message = "Account is blocked from the system";
                    return result;
                }
                bool isMatch = PasswordHasher.VerifyPassword(user.Password, getUser.Password);
                if (!isMatch)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "incorrect Password";
                    return result;
                }
                var token = _jWTTokenHelper.GenerateToken(getUser.UserId, getUser.UserName, getUser.Role.ToString());
                LoginResModel userModel = new LoginResModel()
                {
                    Token = token
                };
                result.IsSuccess = true;
                result.Code = 200;
                result.Data = userModel;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = ex.Message;
                return result;
            }
            return result;

        }

        public async Task<RegisterResModel> AddNewUser(RegisterReqModel model)
        {
            try
            {
                // Check if user already exists
                var existingUser = _accountRepo.Get(u => u.UserName == model.UserName || u.Email == model.Email);
                if (existingUser != null)
                {
                    throw new Exception("User already exists.");
                }

                // Map the request model to the user entity
                var user = _mapper.Map<User>(model);

                // Generate the next user ID
                user.UserId = await _accountRepo.GetNextUserId();

                // Hash the password using PasswordHasher
                user.Password = PasswordHasher.HashPassword(model.Password);

                // Set other properties (e.g., CreatedDate, Status, etc.)
                user.CreatedDate = DateTime.UtcNow;
                user.Status = 1; // Assuming 1 is the default status for an active user

                // Add the user to the repository and save changes
                _accountRepo.Add(user);
                await _accountRepo.SaveChangesAsync();

                // Map the user entity to the response model
                var response = _mapper.Map<RegisterResModel>(user);
                return response;
            }
            catch (Exception e)
            {
                var errorResponse = new RegisterResModel
                {
                    ErrorMessage = e.Message // Set the error message to the exception message
                };

                return errorResponse;
            }
        }

        public async Task<RegisterResModel> MobileRegister(RegisterReqModel model)
        {
            try
            {
                // Check if user already exists
                var existingUser = _accountRepo.Get(u => u.UserName == model.UserName || u.Email == model.Email);
                if (existingUser != null)
                {
                    throw new Exception("User already exists.");
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

                // Map the user entity to the response model
                var response = _mapper.Map<RegisterResModel>(user);
                return response;
            }
            catch (Exception e)
            {
                var errorResponse = new RegisterResModel
                {
                    ErrorMessage = e.Message // Set the error message to the exception message
                };

                return errorResponse;
            }
        }
    }
}
