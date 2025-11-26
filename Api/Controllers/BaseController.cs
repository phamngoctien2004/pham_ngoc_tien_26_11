using Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Domain.Constants.MessageConstant;

namespace Api.Controllers
{
	public class BaseController : ControllerBase
	{
		public async Task<BaseResponseDTO<T>> HandleException<T>(Task<T> task)
		{
			try
			{
				var data = await task;

				return BaseResponseDTO<T>.SuccessResponse(data);
			}
			catch (ApplicationException ex)
			{
				//Log.Logger.Error($"Failed: {ex.Message}\n{ex.StackTrace}");
				return BaseResponseDTO<T>.FailResponse(ex.Message, 200);
			}
			catch (UnauthorizedAccessException ex)
			{
				//Log.Logger.Error($"Failed: {ex.Message}\n{ex.StackTrace}");
				return BaseResponseDTO<T>.FailResponse(ex.Message, 401);
			}
			catch (KeyNotFoundException ex)
			{
				//Log.Logger.Error($"Failed: {ex.Message}\n{ex.StackTrace}");
				return BaseResponseDTO<T>.FailResponse(ex.Message, 404);
			}
			catch (Exception ex)
			{
				Log.Logger.Error($"Failed: {ex.Message}\n{ex.StackTrace}");
				return BaseResponseDTO<T>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500);
			}
		}
	}
}
