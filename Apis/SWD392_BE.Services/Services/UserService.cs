using AutoMapper;
using SWD392_BE.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using SWD392_BE.Services.Sercurity;
using System;
using System.Collections.Generic;
using System.Linq;
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
               
                var users = _userRepository.GetAll().Where(u => u.Status==1).ToList();
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
        public async Task<ResultModel> UpdateUser(UpdateUserViewModel user)
        {
            var result = new ResultModel();
            try
            {
                var existedUser = _userRepository.Get(x => x.UserId.Equals(user.UserId));
                var check = checkNameAndEmail(user.Name, user.Email, user.UserId);
                if (existedUser == null)
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
                    existedUser.Name = user.Name;
                    existedUser.UserName = user.UserName;
                    if (user.Password != "")
                    {
                        existedUser.Password = PasswordHasher.HashPassword(user.Password);
                    }
                    existedUser.Email = user.Email;
                    existedUser.CampusId = user.CampusId;
                    existedUser.Phone = user.Phone;
                    existedUser.Role = user.Role;
                    existedUser.Balance = user.Balance;
                    existedUser.Status = user.Status;
                    existedUser.CreatedDate = DateTime.UtcNow;
                    _userRepository.Update(existedUser);
                    _userRepository.SaveChanges();
                    result.IsSuccess = true;
                    result.Code = 200;
                    return result;
                }
            }            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = ex.Message;
                return result;
            }
            return result;
        }
        public async Task<ResultModel> DeleteUser(string userName)
        {
            var result = new ResultModel();
            try
            {
                var user = await _userRepository.GetUserByUserName(userName);
                if (user == null)
                {
                    result.Message = "UserName not found";
                    result.IsSuccess = false;
                    result.Data = null;
                    return result;
                }
                var disabledUser = await _userRepository.DisableUser(user);
                result.Message = "Disable user successfully";
                result.IsSuccess = true;
                result.Data = disabledUser;
                return result;
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
