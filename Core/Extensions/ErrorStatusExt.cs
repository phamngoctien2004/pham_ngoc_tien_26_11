using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ErrorStatusExt
    {
        public static (string Message, HttpStatusCode StatusCode) GetDetail(this ErrorStatus errorStatus)
        {
            return errorStatus switch
            {
                ErrorStatus.UnAuthorization => ("Permission Denied", HttpStatusCode.Unauthorized),
                ErrorStatus.UnAuthentication => ("User Unauthentication", HttpStatusCode.Unauthorized),
                ErrorStatus.FileNotEmpty => ("File cannot be  empty", HttpStatusCode.BadRequest),
                ErrorStatus.FileTooLarge => ("File size exceeds the maximum limit", HttpStatusCode.RequestEntityTooLarge),
                ErrorStatus.FileNotAllowed => ("File format not allowed", HttpStatusCode.UnsupportedMediaType),
                _ => ("Unknown Error", HttpStatusCode.InternalServerError)

            };
        }
    }
}
