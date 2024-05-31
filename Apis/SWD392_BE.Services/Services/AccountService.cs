using Microsoft.Extensions.Configuration;
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

        public AccountService(IAccountRepository accountRepo, JWTTokenHelper jWTTokenHelper, IConfiguration configuration)
        {
            _accountRepo = accountRepo;
            _jWTTokenHelper = jWTTokenHelper;
            _configuration = configuration;
        }
        public async Task<LoginResModel> Login(LoginReqModel user)
        {
            try
            {
                // Retrieve user by username
                var getUser = await _accountRepo.GetUserByUserName(user.UserName);
                if (getUser == null)
                {
                    throw new Exception("UserName does not exist");
                }

                // Check if the user account is active
                if (getUser.Status < 1 || getUser.Status > 3)
                {
                    throw new Exception("Account is blocked from the system");
                }

                // Verify the password
                bool isMatch = PasswordHasher.VerifyPassword(user.Password, getUser.Password);
                if (!isMatch)
                {
                    throw new Exception("Incorrect password");
                }

                // Generate JWT token
                var token = _jWTTokenHelper.GenerateToken(getUser.UserId, getUser.Name, getUser.Role.ToString());

                // Generate RefreshToken
                string refreshToken = _jWTTokenHelper.GenerateRefreshToken(getUser);

                // Set ExpiredTime
                DateTime expiredTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:TokenValidityInMinutes"]));

                // Update user with refresh token and expired time
                getUser.AccessToken = token;
                getUser.RefreshToken = refreshToken;
                getUser.ExpiredTime = expiredTime;
                 _accountRepo.Update(getUser);
                _accountRepo.SaveChanges();

                // Create and return LoginResModel
                var loginResModel = new LoginResModel
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiredTime = expiredTime
                };

                return loginResModel;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw; // Rethrow the exception to be handled at the controller level
            }
        }


    }
}
