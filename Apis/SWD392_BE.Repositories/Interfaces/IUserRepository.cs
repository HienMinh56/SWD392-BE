using SWD392_BE.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetUsers();
        Task<User> GetUserByUserName(string userName);
        Task<List<User>> GetUsersSortedByCreatedDateAscending();
        Task<List<User>> GetUsersSortedByCreatedDateDescending();

    }
}
