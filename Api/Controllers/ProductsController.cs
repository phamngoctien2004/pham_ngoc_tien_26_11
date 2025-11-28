using Application.DTOs.Common;
using Application.DTOs.Request.Product;
using Application.DTOs.Response.Product;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/products")]
	[Authorize]
	public class ProductsController : BaseController
	{
		private readonly IProductService _service;

		public ProductsController(IProductService service)
		{
			_service = service;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAll([FromQuery] ProductParams productParams)
		{
			var products = await _service.GetAllProductsAsync(productParams);
			return Ok(products);
            //return Ok(BaseResponseDTO<List<ProductResponse>>.SuccessResponse(products, null, "Get all product successfully"));
        }

        [HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var product = await _service.GetProductAsync(id);
			return Ok(BaseResponseDTO<ProductResponse>.SuccessResponse(product!, null,"Get detail product successfully"));
		}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> Create([FromBody] AddProductReq req)
		{
			var product = await _service.AddProductAsync(req);
			return StatusCode(201, BaseResponseDTO<ProductResponse>.SuccessResponse(product, null, "Get detail product successfully"));
		}

		[HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update([FromBody] UpdateProductReq req)
		{
			var product = await _service.UpdateProductAsync(req);
            return StatusCode(201, BaseResponseDTO<ProductResponse>.SuccessResponse(product, null, "Get detail product successfully"));
        }

		[HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
		{
			await _service.DeleteProductAsync(id);
            return NoContent();
		}
    }
}
