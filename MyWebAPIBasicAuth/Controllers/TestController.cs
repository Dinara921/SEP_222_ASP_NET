using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace MyWebAPIBasicAuth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class TestController : ControllerBase
    {
        [HttpGet("SayHello")]
        public ActionResult SayHello(string name)
        {
            return Ok($"Hello {name} {User.Identity.Name} {User.FindFirstValue("psw")}");
        }

        [HttpGet("DownloadFile")]
        public ActionResult DownloadFile(string name)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes($"Hello " + User.Identity.Name + " " + User.FindFirst("role"))); 
            return File(ms,
              "text/txt",
              "text.txt");
        }
    }
}
