using AutoMapper;
using SWD392_BE.Repositories.Entities;
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
            //CreateMap<User, UserResModel>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
        }
    }
}
