using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApi_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        IConfiguration config;
        public ValidateController(IConfiguration config) 
        {
            this.config = config;
        }


    }
}
