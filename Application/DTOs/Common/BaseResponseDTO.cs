using System.Text.Json.Serialization;

namespace Application.DTOs.Common
{
	public class BaseResponseDTO<T>
	{
		public int Code { get; set; } = 0;
		public bool Success { get; set; } = true;
		public string? Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MetaDataDTO? MetaData { get; set; }

		public static BaseResponseDTO<T> SuccessResponse(T data, MetaDataDTO? meta = null, string? message = null, int code = 200)
			=> new() { Data = data, MetaData = meta, Message = message, Code = code, Success = true };

		public static BaseResponseDTO<T> FailResponse(string message, int code = 500)
			=> new() { Message = message, Code = code, Success = false };
  
    }

	public class MetaDataDTO
	{
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 20;
		public int Total { get; set; } = 0;

		public int TotalPage => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)Total / PageSize);
	}
}
