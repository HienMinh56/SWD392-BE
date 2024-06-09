using AutoMapper;
using Google.Apis.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWD392_BE.Repositories.Entities;
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

        public async Task<ResultModel> GetUserList(int? status, string? campusName)
        {
            var result = new ResultModel();
            try
            {
                var users = await _userRepository.GetUsers();

                if (status.HasValue)
                {
                    users = users.Where(u => u.Status == status.Value).ToList();
                }

                if (!string.IsNullOrEmpty(campusName))
                {
                    users = users.Where(u => u.Campus.Name == campusName).ToList();
                }

                if (!users.Any())
                {
                    result.Message = "Data not found";
                    result.IsSuccess = false;
                    result.Code = 404;
                }
                else
                {
                    var userViewModels = users.Select(u => new ListUserViewModel
                    {
                        UserId = u.UserId,
                        Name = u.Name,
                        UserName = u.UserName,
                        Password = u.Password,
                        Email = u.Email,
                        Campus = u.Campus.Name,
                        Phone = u.Phone,
                        Role = u.Role,
                        Balance = u.Balance,
                        Status = u.Status
                    }).ToList();

                    result.Data = userViewModels;
                    result.Message = "Success";
                    result.IsSuccess = true;
                    result.Code = 200;
                }
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

        public User SearchUser(string keyword)
        {
            var user = _userRepository.Get(u => u.UserName.Contains(keyword.Trim())
                                                || u.Email.Contains(keyword.Trim())
                                                || u.Phone.Contains(keyword.Trim()));
            if (user != null)
            {
                return user;
            }
            return null;
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
                    result.Message = "UserName not found or deleted";
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

        public async Task<ResultModel> SearchUserByKeyword(string keyword)
        {
            var result = new ResultModel();
            try
            {
                var user = SearchUser(keyword);
                result.IsSuccess = true;
                result.Code = 200;
                result.Data = user;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                result.Code = 404;
                return result;
            }
        }
    }
}
