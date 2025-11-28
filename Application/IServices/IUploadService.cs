using Application.DTOs.Request.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IUploadService
    {
        Task<string> Upload(UploadReq file);
    }
}
