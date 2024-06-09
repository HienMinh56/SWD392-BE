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
        public async Task<ResultModel> UpdateUser(string userId, UpdateUserViewModel model, ClaimsPrincipal userUpdate)
        {
            var result = new ResultModel();
            try
            {
                var existingUser = _userRepository.Get(x => x.UserId == userId);
                if (existingUser == null)
                {
                    result.IsSuccess = false;
                    result.Code = 404;
                    result.Message = "Can not find user";
                    return result;
                }
                // Check if email already exists
                var existingEmail = _userRepository.Get(x => x.Email == model.Email && x.UserId != userId);
                if (existingEmail != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Email already exists";
                    return result;
                }
                // Check if Phone already exists
                var existingPhone = _userRepository.Get(x => x.Phone == model.Phone);
                if (existingPhone != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Phone already exists";
                    return result;
                }
                // Map the ViewModel to the existing userid entity
                _mapper.Map(model, existingUser);

                // Update the additional fields
                existingUser.Name = model.Name;
                if (existingUser.Password != "")
                {
                    existingUser.Password = PasswordHasher.HashPassword(existingUser.Password);
                }
                existingUser.Email = model.Email;
                existingUser.CampusId = model.CampusId;
                existingUser.Phone = model.Phone;
                existingUser.Role = model.Role;
                existingUser.Balance = model.Balance;
                existingUser.ModifiedBy = userUpdate.FindFirst("UserName")?.Value;
                existingUser.ModifiedDate = DateTime.Now;
                _userRepository.Update(existingUser);
                _userRepository.SaveChanges();
                result.IsSuccess = true;
                result.Code = 200;
                result.Message = "Update User Success";
                return result;

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
