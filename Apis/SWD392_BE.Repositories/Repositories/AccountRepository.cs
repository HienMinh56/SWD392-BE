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
        public async Task<string> GetUserId()
        {
            var lastUserId = await _dbContext.Users
                .OrderByDescending(u => u.UserId)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastUserId))
            {
                return "USER001";
            }

            int number = int.Parse(lastUserId.Substring(4));
            return $"USER{number + 1:000}";
        }
        public async Task<bool> UserExists(string userName, string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.UserName == userName || u.Email == email);
        }

        public async Task AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public List<Campus> GetCampuses()
        {
            return _dbContext.Campuses.ToList();
        }
    }
}
