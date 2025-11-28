using Application.DTOs.Common;
using Application.DTOs.Request.Product;
using Application.DTOs.Response.Product;
using Application.IRepository;
using Domain.Entities;

namespace Application.Services
{
	public interface IProductService
	{
		Task<ProductResponse?> GetProductAsync(int id);
		Task<List<ProductResponse>> GetAllProductsAsync();
		Task<BaseResponseDTO<List<ProductResponse>>> GetAllProductsAsync(ProductParams param);
		Task<ProductResponse> AddProductAsync(AddProductReq req);
		Task<ProductResponse> UpdateProductAsync(UpdateProductReq req);
		Task DeleteProductAsync(int id);

	}
}
