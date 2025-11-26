namespace Application.DTOs.Common
{
	public class BaseRequestDTO
	{
		public int ActionBy { get; set; }
		public int LanguageKey { get; set; }
		public bool IsAdmin { get; set; }
	}

	public class BaseRequestDTO<T> : BaseRequestDTO
	{
		public T? Request { get; set; }
	}
}
