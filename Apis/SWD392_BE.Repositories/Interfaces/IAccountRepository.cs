using SWD392_BE.Repositories.Models;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<User> GetUserByUserName(string userName);

        public Task<User> CheckLogin(string email, string password);
    }
}
