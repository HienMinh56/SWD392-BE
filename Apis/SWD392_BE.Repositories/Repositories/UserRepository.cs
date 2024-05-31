using Microsoft.AspNetCore.CookiePolicy;
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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CampusFoodSystemContext context) : base(context)
        {
        }

        public async Task<List<User>> GetAllUsers()
        {
            using var context = new CampusFoodSystemContext();
            return await context.Users
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
                    Status = x.Status
                })
                .AsNoTracking()
                .ToListAsync();

        }
    }
}
