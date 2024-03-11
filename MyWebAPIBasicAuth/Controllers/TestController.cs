using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
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

        [HttpGet("SetCookie/{key}/{value}"), AllowAnonymous]
        public IActionResult SetCookie(string key, string value)
        {
            Response.Cookies.Append(key, value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(12),
                HttpOnly = true,
                Secure = false
            });
            return Ok("ok");
        }

        [HttpGet("GetCookie/{key}"), AllowAnonymous]
        public IActionResult GetCookie(string key)
        {
            string value = Request.Cookies[key]; 
            return Ok(value);
        }
    }
}


//ALTER PROC[dbo].[pUser3]
//@login NVARCHAR(200),
//@pwd NVARCHAR(200),
//@res_out INT OUT
//AS
//BEGIN
//    SET @res_out = (
//        SELECT CASE WHEN EXISTS (
//            SELECT 1
//            FROM User3
//            WHERE [login] = @login AND[pwd] = @pwd
//        ) THEN 1 ELSE 0 END
//    )
//END