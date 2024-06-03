using Microsoft.AspNetCore.CookiePolicy;
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
    public class UserRepository : IUserRepository
    {
        private readonly CampusFoodSystemContext _dbContext;
        public UserRepository(CampusFoodSystemContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users
                .Select(x => new User
                {
                    UserId = x.UserId,
                    Name = x.Name,
                    UserName = x.UserName,
                    Password = x.Password,
                    Email = x.Email,
                    CampusId = x.CampusId,
                    Phone = x.Phone,
                    Role = x.Role,
                    Balance = x.Balance,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _dbContext.Users
                .Where(x => x.UserName == userName)
                .Select(x => new User
                {
                    UserId = x.UserId,
                    Name = x.Name,
                    UserName = x.UserName,
                    Password = x.Password,
                    Email = x.Email,
                    CampusId = x.CampusId,
                    Phone = x.Phone,
                    Role = x.Role,
                    Balance = x.Balance,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<User> DisableUser(User user)
        {
            user.Status = 0;
            _dbContext.Users.Attach(user);
            _dbContext.Entry(user).Property(u => u.Status).IsModified = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;

        }

    }
}
