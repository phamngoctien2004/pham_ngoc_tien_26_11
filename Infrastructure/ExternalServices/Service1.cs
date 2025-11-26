using Application.DTOs.Common;
using Application.IServices.ExternalServices;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.ExternalServices
{
	public class Service1 : IService1
	{
		private readonly ILogger<Service1> _logger;

		public Service1(ILogger<Service1> logger)
		{
			_logger = logger;
		}

		public async Task<List<Product>> GetAllProducts()
		{
			try
			{
				var url = $"{AppSettings.Service1.Url}/product/getall";

				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Add("Token", AppSettings.Service1.Token);

					var response = await client.GetAsync(url);

					//var requestBody = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
					//var response1 = await client.PostAsync(url, requestBody);

					response.EnsureSuccessStatusCode();

					var content = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<BaseResponseDTO<List<Product>>>(content);

					return result?.Data;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed: {ex.Message}\n{ex.StackTrace}");

				return null;
			}
		}
	}
}
