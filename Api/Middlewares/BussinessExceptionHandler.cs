using Application.DTOs.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Middlewares
{
    public class BussinessExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = BaseResponseDTO<string>.FailResponse(exception.Message);

            // Xử lý status sau
                //logic xử lý
            //
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
