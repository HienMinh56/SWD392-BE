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

        public AccountService(IAccountRepository accountRepo, JWTTokenHelper jWTTokenHelper)
        {
            _accountRepo = accountRepo;
            _jWTTokenHelper = jWTTokenHelper;
        }

        public async Task<ResultModel> Login(LoginReqModel user)
        {
            ResultModel result = new ResultModel();

            try
            {
                // Retrieve user by username
                var getUser = await _accountRepo.GetUserByUserName(user.UserName);
                if (getUser == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "UserName does not exist";
                    return result;
                }

                // Check if the user account is active
                if (getUser.Status < 1 || getUser.Status > 3)
                {
                    result.IsSuccess = false;
                    result.Code = (int)HttpStatusCode.Forbidden;
                    result.Message = "Account is blocked from the system";
                    return result;
                }

                // Verify the password
                bool isMatch = PasswordHasher.VerifyPassword(user.Password, getUser.Password);
                if (!isMatch)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Incorrect password";
                    return result;
                }

                // Generate JWT token
                var token = _jWTTokenHelper.GenerateToken(getUser.UserId, getUser.Name, getUser.Role.ToString());
                LoginResModel userModel = new LoginResModel { Token = token };

                result.IsSuccess = true;
                result.Code = 200;
                result.Data = userModel;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 500;  // Internal server error
                result.Message = ex.Message;
                return result;
            }
        }
    }
}
