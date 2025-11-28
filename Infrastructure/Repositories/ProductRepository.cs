using Application.DTOs.Request.Product;
using Application.IRepository;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _context;

		public ProductRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Product?> GetByIdAsync(int id)
		{
            var product = await _context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(p => p.Id == id);
			return product;
        }

        public async Task<List<Product>> GetAllAsync() =>
			await _context.Products.Include(p => p.Category).ToListAsync();

		public async Task<Product> AddAsync(Product product)
		{
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return product;
		}


		public async Task<Product> SaveChange(Product product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
			return product;
        }

        public async Task DeleteAsync(int id)
        {
			var product = new Product { Id = id};
			 _context.Products.Remove(product);
			await _context.SaveChangesAsync();
        }

        public async Task<(List<Product>, int)> GetAllAsync(ProductParams param)
        {
			var query = _context.Products.AsQueryable();
			
			query = query.ApplyFilter(param).ApplySort(param.SortColumn, param.SortDirection);

			// count total element
			var count = await query.CountAsync();
			var items = await query.Skip((param.PageNumber - 1) * param.PageSize)
				.Take(param.PageSize)
				.Include(p => p.Category)
				.ToListAsync();
			return (items, count);
        }
    }
}
