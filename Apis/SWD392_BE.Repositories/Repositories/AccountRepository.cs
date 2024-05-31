using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CampusFoodSystemContext _dbContext;

        public AccountRepository(CampusFoodSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<User> CheckLogin(string email, string password)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.Equals(email) && x.Password.Equals(password));
        }


    }
}
