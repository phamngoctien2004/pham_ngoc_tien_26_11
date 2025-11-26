using Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InternalController : BaseController
	{

		[HttpGet]
		[InternalAuthorize]
		public IActionResult GetSecretData()
		{
			return Ok("Success - valid internal token!");
		}
	}
}
