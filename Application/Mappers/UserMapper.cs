using Application.DTOs.Request.Auth;
using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserResponse>();
            CreateMap<AddUserReq, User>();
            CreateMap<RegisterRequest, User>();
        }
    }
}
