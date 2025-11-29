using Application.DTOs.Request.Auth;
using Application.DTOs.Response.Auth;
using Application.DTOs.Response.User;
using Application.Exceptions;
using Application.IServices;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public AuthService(IUserService UserService, IJwtService JwtService, IMapper mapper) 
        { 
            _userService = UserService;
            _jwtService = JwtService;
            _mapper = mapper;
        }

        public async Task<AuthResponse> Login(LoginRequest loginRequest)
        {
            var user = await _userService.GetByEmail(loginRequest.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                throw new AppException(ErrorStatus.UnAuthentication);
            }
            return new AuthResponse()
            {
                AccessToken = _jwtService.GenerateToken(user.Id.ToString(), user.Role.Name, false),
                RefreshToken = _jwtService.GenerateToken(user.Id.ToString(), user.Role.Name, true),
                User = _mapper.Map<UserResponse>(user)
            };
        }

        public async Task<AuthResponse> Register(RegisterRequest registerRequest)
        {
            var user = await _userService.GetByEmail(registerRequest.Email);
            
            if(user != null)
            {
                throw new AppException(ErrorStatus.EmailExisted);
            }
            user = _mapper.Map<User>(registerRequest);
            var newUser = await _userService.AddUser(user);
            return new AuthResponse()
            {
                AccessToken = _jwtService.GenerateToken(user.Id.ToString(), user.Role.Name, false),
                RefreshToken = _jwtService.GenerateToken(user.Id.ToString(), user.Role.Name, true),
                User = _mapper.Map<UserResponse>(newUser)
            };
        }
    }
}
