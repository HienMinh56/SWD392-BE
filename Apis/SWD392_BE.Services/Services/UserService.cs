using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Sercurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ResultModel> ViewAllUsers()
        {
            var result = new ResultModel();
            try
            {

                var users = _userRepository.GetAll().Where(u => u.Status == 1).ToList();
                var viewModels = _mapper.Map<List<ListUserViewModel>>(users);

                result.Data = viewModels;
                result.Message = "Success";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }

        public User GetUserById(string id)
        {
            try
            {
                var user = _userRepository.Get(x => x.UserId == id);

                if (user == null)
                {
                    // Handle the case where the user is not found, e.g., return null or throw an exception
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public User GetUserByUserName(string userName)
        {
            var user = _userRepository.Get(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        private int checkNameAndEmail(string name, string email, string userId)
        {
            var users = _userRepository.GetAll();
            if (users.FirstOrDefault(c => c.Name == name && c.UserId != userId) != null)
            {
                return 0;
            }
            else if (users.FirstOrDefault(d => d.Email == email && d.UserId != userId) != null)
            {
                return -1;
            }
            return 1;
        }
        public async Task<ResultModel> UpdateUser(string userId,UpdateUserViewModel model, ClaimsPrincipal userUpdate)
        {
            var result = new ResultModel();
            try
            {
                var user = _userRepository.Get(x => x.UserId == userId);
                var check = checkNameAndEmail(user.Name, user.Email, user.UserId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Code = 404;
                    result.Message = "Can not find user";
                    return result;
                }
                else if (check == 0)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "User Name already existed";
                    return result;
                }
                else if (check == -1)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Email already existed";
                    return result;
                }
                else if (check == 1)
                {
                    user.Name = user.Name;
                    if (user.Password != "")
                    {
                        user.Password = PasswordHasher.HashPassword(user.Password);
                    }
                    user.Email = user.Email;
                    user.CampusId = user.CampusId;
                    user.Phone = user.Phone;
                    user.Role = user.Role;
                    user.Balance = user.Balance;
                    user.ModifiedBy = userUpdate.FindFirst("UserName")?.Value;
                    user.ModifiedDate = DateTime.Now;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                    result.IsSuccess = true;
                    result.Code = 200;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = ex.Message;
                return result;
            }
            return result;
        }
        public async Task<ResultModel> DeleteUser(DeleteUserReqModel request)
        {
            var result = new ResultModel();
            try
            {
                var user = GetUserById(request.UserId);
                if (user == null)
                {
                    result.Message = "UserName not found";
                    result.Code = 404;
                    result.IsSuccess = false;
                    result.Data = null;
                    return result;
                }

                user.Status = user.Status = 2;
                _userRepository.Update(user);
                _userRepository.SaveChanges();

                result.Message = "Delete user successfully";
                result.Code = 200;
                result.IsSuccess = true;
                result.Data = user;
            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }

    }
}
