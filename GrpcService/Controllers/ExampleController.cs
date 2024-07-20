using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize]
    public class ExampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() =>
            Ok(new { Message = "Hello from ExampleController v1.0" });

        [HttpGet, MapToApiVersion("2.0")]
        public IActionResult GetV2() =>
            Ok(new { Message = "Hello from ExampleController v2.0" });
    }
}