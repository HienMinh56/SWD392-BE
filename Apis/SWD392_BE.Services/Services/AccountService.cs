using SWD392_BE.Repositories.Entities;
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

        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }
        public async Task<ResultModel> Login(LoginReqModel user)
        {
            ResultModel result = new ResultModel();
            try
            {
                var getUser = await _accountRepo.GetUserByEmail(user.Email);
                if (getUser == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Email is not exist";
                    return result;
                }
                if (getUser.Status != 1 && getUser.Status != 2 & getUser.Status != 3)
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
                LoginResModel userModel = new LoginResModel()
                {
                    userId = getUser.UserId,
                    name = getUser.Name,
                    userName = getUser.UserName,
                    email = getUser.Email,
                    campusId = getUser.CampusId,
                    phone = getUser.Phone,
                    role = getUser.Role,
                    balance = getUser.Balance,
                    status = getUser.Status

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

    }
}
