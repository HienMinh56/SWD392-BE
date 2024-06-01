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
                    Status = x.Status,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                })
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
