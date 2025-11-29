using Application.DTOs.Common;
using Application.Exceptions;
using AutoMapper;
using Core.Extensions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Middlewares
{
    public class BussinessExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BussinessExceptionHandler> _logger;

        public BussinessExceptionHandler(ILogger<BussinessExceptionHandler> logger)
        {
            _logger = logger; 
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = BaseResponseDTO<string>.FailResponse("Internal Server Error");

            var statusCode = StatusCodes.Status500InternalServerError;

            // Xử lý lỗi bussiness
            if(exception is AppException appExp)
            {
                var (message, status) = appExp.ErrorStatus.GetDetail();
                response = BaseResponseDTO<string>.FailResponse(message, (int) appExp.ErrorStatus);
                statusCode = (int) status;
                _logger.LogWarning("Business Error: {Code} - {Message}", appExp.ErrorStatus, message);
            }
            else
            {
                _logger.LogError(exception, "System Exception: {Message}", exception.Message);
            }
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
