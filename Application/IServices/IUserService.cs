using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Application.IRepository;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        Task<User> AddUser(User user);
        Task<User> AddUser(AddUserReq userRequest);

    }
}
