using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ResultModel> ViewAllUsers();

        public Task<ResultModel> DeleteUser(DeleteUserReqModel request);
        public User GetUserById(string id);
        public User GetUserByUserName(string userName);
        public Task<ResultModel> UpdateUser(string userId ,UpdateUserViewModel model, ClaimsPrincipal userUpdate);
    }
}
