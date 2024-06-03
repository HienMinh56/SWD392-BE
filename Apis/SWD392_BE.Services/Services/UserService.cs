using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ResultModel> ViewAllUsers()
        {
            var result = new ResultModel();
            try
            {
                var users = await _userRepository.GetAllUsers();
                result.Data = users;
                result.Message = "Success";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ResultModel> DeleteUser(string userName)
        {
            var result = new ResultModel();
            try
            {
                var user = await _userRepository.GetUserByUserName(userName);
                if (user == null)
                {
                    result.Message = "UserName not found";
                    result.IsSuccess = false;
                    result.Data = null;
                    return result;
                }
                var disabledUser = await _userRepository.DisableUser(user);
                result.Message = "Disable user successfully";
                result.IsSuccess = true;
                result.Data = disabledUser;
                return result;
            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }

    }
}
