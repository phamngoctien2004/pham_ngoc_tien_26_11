using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.DTOs.Request.Upload
{
    public class UploadReq
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Length { get; set; }
        public Stream FileStream { get; set; }

        public static readonly long MaxSize = 10; //mb
        public static readonly string[] AllowedExtension = { ".jpg", ".jpeg", ".png", ".webp" };
    }
}
