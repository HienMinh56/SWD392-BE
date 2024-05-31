using SWD392_BE.Repositories.Models;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<User> GetUserByEmail(string email);

        public Task<User> CheckLogin(string email, string password);
        public Task<string> GetUserId();
        public Task<bool> UserExists(string userName, string email);
        public Task AddUser(User user);
        public List<Campus> GetCampuses();
    }
}
