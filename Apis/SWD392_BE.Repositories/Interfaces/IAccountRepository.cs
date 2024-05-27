using SWD392_BE.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<User> GetUserByEmail(string email);

        public Task<User> CheckLogin(string email, string password);
    }
}
