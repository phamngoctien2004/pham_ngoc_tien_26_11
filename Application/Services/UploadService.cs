using Application.DTOs.Request.Upload;
using Application.Exceptions;
using Application.IServices;
using Core.Enums;
using Microsoft.AspNetCore.Hosting;

namespace Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _environment;
        public UploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> Upload(UploadReq file)
        {   
            var ext = ValidateFile(file);
            var pathFile = await CreateFile(file, ext);
            return pathFile;
        }

        private string ValidateFile(UploadReq file)
        {
            if (file == null || file.FileStream == null)
            {
                throw new AppException(ErrorStatus.FileNotEmpty);
            }

            var maxBytes = UploadReq.MaxSize * 1024 * 1024; // 10mb
            if(file.Length > maxBytes)
            {
                throw new AppException(ErrorStatus.FileTooLarge);
            }

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!UploadReq.AllowedExtension.Contains(ext))
            {
                throw new AppException(ErrorStatus.FileNotAllowed);
            }
            return ext;
        }
        private async Task<string> CreateFile(UploadReq file, string ext)
        {
            // tìm thư mục cấu /static (java)
            var webRootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            // tìm đường dẫn thư mục
            var folderPath = Path.Combine(webRootPath, "images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // tạo tên file
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folderPath, fileName);

            await SaveDisk(file.FileStream, fullPath);

            return fileName;
        }
        private async Task SaveDisk(Stream inputStream, string fullPath)
        {
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await inputStream.CopyToAsync(stream);
            }
        }
    }
}
