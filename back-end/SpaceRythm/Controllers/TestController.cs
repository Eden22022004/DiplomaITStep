using Microsoft.AspNetCore.Mvc;

namespace SpaceRythm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var context = HttpContext;
            return Ok($"Server is running! Request from: {context.Connection.RemoteIpAddress}");
        }
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("Server is running!");
        //}
    }
}
