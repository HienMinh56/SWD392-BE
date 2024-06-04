using AutoMapper;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.MapperProfile
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, ListUserViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CampusId, opt => opt.MapFrom(src => src.CampusId))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<RegisterReqModel, User>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Ignore UserId since it will be generated
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) // Ignore CreatedDate since it will be set separately
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Ignore CreatedBy since it will be set separately
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore()) // Ignore ModifiedDate
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore()) // Ignore ModifiedBy
            .ForMember(dest => dest.DeletedDate, opt => opt.Ignore()) // Ignore DeletedDate
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore()) // Ignore DeletedBy
            .ForMember(dest => dest.Balance, opt => opt.Ignore()) // Ignore Balance
            .ForMember(dest => dest.Campus, opt => opt.Ignore()) // Ignore Campus since it will be handled differently
            .ForMember(dest => dest.Orders, opt => opt.Ignore()) // Ignore Orders
            .ForMember(dest => dest.Transactions, opt => opt.Ignore());

            CreateMap<User, DeleteUserReqModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        }
    }
}
