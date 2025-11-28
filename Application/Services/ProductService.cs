using Application.DTOs.Common;
using Application.DTOs.Request.Product;
using Application.DTOs.Response.Product;
using Application.IRepository;
using Application.IServices.ExternalServices;
using Application.Mappers;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using static Domain.Constants.MessageConstant;

namespace Application.Services
{
	public class ProductService : IProductService
	{
		private readonly ILogger<ProductService> _logger;
		private readonly IProductRepository _repository;
		private readonly IService1 _service1;
		private readonly IMapper _mapper;

		public ProductService(ILogger<ProductService> logger, IProductRepository repository, IService1 service1, IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_service1 = service1;
			_mapper = mapper;
        }

		public async Task<ProductResponse?> GetProductAsync(int id)
		{
			_logger.LogError($"GetProductAsync id: {id}");

			var result = await _repository.GetByIdAsync(id);
            if (result == null) throw new KeyNotFoundException(CommonMessage.NOT_FOUND);
            var response = _mapper.Map<ProductResponse>(result);

            return response;
		}

		public async Task<List<ProductResponse>> GetAllProductsAsync()
		{
			return _mapper.Map<List<ProductResponse>>(await _repository.GetAllAsync());
		}

        public async Task<ProductResponse> AddProductAsync(AddProductReq req)
        {
			Product product = _mapper.Map<Product>(req);
			var  response = await _repository.AddAsync(product);
            return _mapper.Map<ProductResponse>(response);
        }
        public async Task<ProductResponse> UpdateProductAsync(UpdateProductReq req)
        {
			var currentProduct = await _repository.GetByIdAsync(req.Id);
			currentProduct!.Name = req.Name;
			currentProduct.Price = req.Price;
			currentProduct.Quantity = req.Quantity;
			currentProduct.Status = req.Status;

            return _mapper.Map<ProductResponse>(await _repository.SaveChange(currentProduct));
        }

        public async Task DeleteProductAsync(int id)
        {
			await _repository.DeleteAsync(id);
        }

        public async Task<BaseResponseDTO<List<ProductResponse>>> GetAllProductsAsync(ProductParams param)
        {
			var result = await _repository.GetAllAsync(param);

			var products = result.Item1;
			var count = result.Item2;

			var metaData = new MetaDataDTO
			{
				Page = param.PageNumber,
				PageSize = param.PageSize,
				Total = count
			};
			return BaseResponseDTO<List<ProductResponse>>.SuccessResponse(
                    _mapper.Map<List<ProductResponse>>(products),
					metaData,
                    "Get products successfully"
            );
        }
    }
}
