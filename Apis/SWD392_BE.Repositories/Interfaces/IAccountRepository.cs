using SWD392_BE.Repositories.Entities;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<User> GetUserByUserName(string userName);

        public Task<User> CheckLogin(string userName, string password);
    }
}
