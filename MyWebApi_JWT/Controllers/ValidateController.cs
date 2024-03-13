using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using MyWebApi_JWT.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [AllowAnonymous]
        [HttpPost, Route("GetToken")]
        public ActionResult GetToken(UserModel model)
        {
            //using (SqlConnection db = new SqlConnection(@"Server=206-11\SQLEXPRESS;Database=testDB;Trusted_Connection=True;TrustServerCertificate=Yes;"))
            //{
            //    DynamicParameters p = new DynamicParameters(model);
            //    int count = db.ExecuteScalar<int>("pUserValidate", p, commandType: CommandType.StoredProcedure);
            //    if (count == 0)
            //        return Unauthorized("Гуляй Вася");
            //}

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] 
            {
                      new Claim("myRole", "admin"),
                      new Claim("dateBirth", "2000-01-01")
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
            config["Jwt:Issuer"],
            claims,
            //null,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: credentials);

            var sToken = new JwtSecurityTokenHandler().WriteToken(token);

            ReturnStatus result = new ReturnStatus()
            {
                status = StatusEnum.OK,
                result = sToken
            };
            return Ok(result);
        }

        [HttpGet, Authorize, Route("GetTest/{name}")]
        public ActionResult GetTest(string name)
        {
            return Ok("Hello " + name + " " + User.FindFirst("myRole")?.Value);
        }
    }
}
