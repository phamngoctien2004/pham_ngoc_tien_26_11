using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
	public interface IProductRepository
	{
		Task<Product?> GetByIdAsync(int id);
		Task<List<Product>> GetAllAsync();
		Task<Product> AddAsync(Product product);
		Task<Product> SaveChange(Product product);
		Task DeleteAsync(int id);
    }
}
