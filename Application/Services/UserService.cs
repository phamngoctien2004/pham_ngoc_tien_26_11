using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Application.IRepository;
using Application.IServices;
using Application.Mappers;
using AutoMapper;
using BCrypt.Net;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository UserRepository, IMapper mapper)
        {
            _userRepository = UserRepository;
            _mapper = mapper;
        }

        public async Task<User> AddUser(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                var passwordHashed = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = passwordHashed;
            }
            return await _userRepository.AddAsync(user);
        }
        public async Task<User> AddUser(AddUserReq userRequest)
        {
            var user = _mapper.Map<User>(userRequest);
            return await AddUser(user);
        }
        public async Task<User> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            return user;
        }
    }
}
