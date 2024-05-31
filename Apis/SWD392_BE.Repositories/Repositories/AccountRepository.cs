using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Repositories
{
    public class AccountRepository : GenericRepository<User>, IAccountRepository
    {
        private readonly CampusFoodSystemContext _dbContext;

        public AccountRepository(CampusFoodSystemContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName.Equals(userName));
        }

        public async Task<User> CheckLogin(string userName, string password)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName.Equals(userName) && x.Password.Equals(password));
        }


    }
}
