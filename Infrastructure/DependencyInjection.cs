using Application.IRepository;
using Application.IServices.ExternalServices;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

			services.AddScoped<IProductRepository, ProductRepository>();

			services.AddScoped<IService1, Service1>();

			return services;
		}
	}
}
