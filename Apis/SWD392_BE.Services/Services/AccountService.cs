using Microsoft.Extensions.Configuration;
using SWD392_BE.Repositories.Helper;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Sercurity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
                var getUser = _accountRepo.Get(u => u.UserName == user.UserName);
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

                // Decode the Access Token to check its expiration time
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(getUser.AccessToken);

                // Extract the expiration time from the Access Token
                var accessTokenExpiration = token.ValidTo;

                // Check if the Access Token has expired
                if (accessTokenExpiration <= DateTime.Now)
                {
                    // Access Token has expired, generate a new one
                    var newAccessToken = _jWTTokenHelper.GenerateToken(
                        getUser.UserId,
                        getUser.Name,
                        getUser.Role.ToString()
                    );

                    // Update user with the new Access Token and its expiration time
                    getUser.AccessToken = newAccessToken;
                    accessTokenExpiration = DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:TokenValidityInMinutes"]));
                }

                // Check if the Refresh Token has expired
                if (getUser.ExpiredTime <= DateTime.Now)
                {
                    // Refresh Token has expired, generate a new one
                    var newRefreshToken = _jWTTokenHelper.GenerateRefreshToken(getUser);

                    // Update user with the new Refresh Token and its expiration time
                    getUser.RefreshToken = newRefreshToken;
                    getUser.ExpiredTime = DateTime.Now.AddDays(int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]));
                }

                // Save changes to the user entity
                _accountRepo.Update(getUser);
                _accountRepo.SaveChanges();

                // Create and return LoginResModel
                var loginResModel = new LoginResModel
                {
                    Token = getUser.AccessToken,
                    RefreshToken = getUser.RefreshToken,
                    ExpiredTime = getUser.ExpiredTime
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
