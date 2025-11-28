using Application.DTOs.Request.Product;
using Core.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
	public interface IUserRepository
    {
		Task<User?> GetByIdAsync(int id);
		Task<User?> GetByEmail(string email);
		Task<List<User>> GetAllAsync();
        //Task<(List<User> , int)> GetAllAsync(ProductParams param);
        Task<User> AddAsync(User product);
		Task<User> SaveChange(User product);
		Task DeleteAsync(int id);
    }
}
