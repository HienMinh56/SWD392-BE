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
        public Task<ResultModel> GetUserList(int? status, string? campusName);
        public Task<ResultModel> DeleteUser(DeleteUserReqModel request, ClaimsPrincipal userDelete);
        public User GetUserById(string id);
        public User GetUserByUserName(string userName);
        public Task<ResultModel> SearchUserByKeyword(string keyword);
        public Task<ResultModel> UpdateUser(string userId ,UpdateUserViewModel model, ClaimsPrincipal userUpdate);
        public Task<ResultModel> GetUsersSortedByCreatedDateAscending();
        public Task<ResultModel> GetUsersSortedByCreatedDateDescending();
        public Task<ResultModel> EditUser(string userId, EditUserViewModel model, ClaimsPrincipal userUpdate);
    }
}
