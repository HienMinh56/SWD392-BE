using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Models;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;

        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }
        public async Task<bool> Login(string email, string password)
        {
            var user = await _accountRepo.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            bool isPasswordValid = user.Password == password;
            return isPasswordValid;

        }
        public static string HashAndTruncatePassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                password = BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();
            }

            // Truncate hash to 16 characters
            password = password.Substring(0, 16);

            return password;
        }
        public async Task<bool> Register(string userName, string password, string email, int phone, string campusId, string name)
        {
            // Kiểm tra nếu username hoặc email đã tồn tại
            if (await _accountRepo.UserExists(userName, email))
            {
                return false; // User đã tồn tại
            }

            // Lấy UserId mới
            var newUserId = await _accountRepo.GetUserId();
            
            // Tạo đối tượng User mới
            User newUser = new User
            {
                UserId = newUserId,
                UserName = userName,
                Password = HashAndTruncatePassword(password), // Hash mật khẩu trước khi lưu
                Email = email,
                Phone = phone,
                CampusId = campusId,
                Name = name, // Gán giá trị cho thuộc tính Name
                CreatedDate = DateTime.Now,
                Status = 1, // Trạng thái active
                Balance = 0, // Số dư ban đầu
                Role = 1 // Vai trò mặc định, có thể thay đổi tùy theo yêu cầu
            };

            // Lưu user mới vào cơ sở dữ liệu
            await _accountRepo.AddUser(newUser);

            return true;
        }
        public List<Campus> GetCampuses()
        {
            return _accountRepo.GetCampuses();
        }
    }
}
