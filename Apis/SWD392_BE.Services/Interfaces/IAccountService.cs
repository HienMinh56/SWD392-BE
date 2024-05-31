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
    public interface IAccountService
    {
        //public Task<ResultModel> Login(LoginModel User);

        public Task<bool> Login(string email, string password);
        public Task<bool> Register(string userName, string password, string email, int phone, string campusId, string name);
    }
}
