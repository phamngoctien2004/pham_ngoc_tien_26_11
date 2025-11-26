using Application.DTOs.Common;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Api.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class InternalAuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var request = context.HttpContext.Request;
			string? incomingToken = null;

			if (request.Headers.TryGetValue("Token", out var tokenHeader))
			{
				incomingToken = tokenHeader.FirstOrDefault();
			}

			// Không có token
			if (string.IsNullOrEmpty(incomingToken))
			{
				//Log.Logger.Error("Missing Token!");

				throw new ApplicationException("Missing Token!");

				//return;
			}

			// Token không hợp lệ
			if (incomingToken != AppSettings.InternalToken)
			{
				//Log.Logger.Error("Invalid Token!");
				throw new ApplicationException("Invalid Token!");
				//return;
			}
		}
	}
}
 