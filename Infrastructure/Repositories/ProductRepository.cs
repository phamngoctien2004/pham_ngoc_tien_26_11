using Application.IRepository;
using Domain.Entities;
using Infrastructure.Persistence;
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
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			return product;
        }

        public async Task<List<Product>> GetAllAsync() =>
			await _context.Products.ToListAsync();

		public async Task<Product> AddAsync(Product product)
		{
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();

			if(product == null)
			{
				throw new Exception("Product could not be added.");
            }
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
    }
}
