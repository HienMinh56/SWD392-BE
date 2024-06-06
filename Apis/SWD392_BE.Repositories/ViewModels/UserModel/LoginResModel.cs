using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.ViewModels.UserModel
{
    public class LoginResModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? ExpiredTime { get; set; }

    }
}
