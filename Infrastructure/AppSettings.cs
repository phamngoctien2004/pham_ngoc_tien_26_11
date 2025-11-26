using Application.DTOs.Config;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
	public static class AppSettings
	{
		private static IConfiguration _configuration;
		private static readonly Dictionary<string, object> _cache = new();

		public static void Initialize(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public static string InternalToken => _configuration.GetSection("InternalToken").Get<string>();
		public static ExternalServiceDTO Service1 => _configuration.GetSection("ExternalServices:Service1").Get<ExternalServiceDTO>();
	}
}
