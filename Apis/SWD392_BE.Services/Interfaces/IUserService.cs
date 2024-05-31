using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ResultModel> ViewAllUsers();
        public User GetUserById(string id);
        public User GetUserByUserName(string userName);
    }
}
