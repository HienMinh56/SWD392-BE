using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Interfaces
{
    public interface IAccountService
    {
        //public Task<ResultModel> Login(LoginModel User);

        public Task<bool> Login(string email, string password);

    }
}
