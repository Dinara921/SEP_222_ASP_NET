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
        string conStr = @"Server=DESKTOP-S23LER7;Database=ASP_NET;Trusted_Connection=True;TrustServerCertificate=Yes;";
        IConfiguration config;
        public ValidateController(IConfiguration config) 
        {
            this.config = config;
        }

        [AllowAnonymous]
        [HttpPost, Route("GetToken")]
        public ActionResult GetToken(UserModel model)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                db.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@login", model.login);
                parameters.Add("@pwd", model.password);
                parameters.Add("@res_out", dbType: DbType.Int32, direction: ParameterDirection.Output);

                db.Execute("pUser3;2", parameters, commandType: CommandType.StoredProcedure);

                int count = parameters.Get<int>("@res_out");

                if (count == 0)
                    return Unauthorized("User Not Found");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] 
            {
                      new Claim("login", model.login),
                      new Claim("password", model.password)
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

        [HttpGet, Authorize, Route("GetTest")]
        public ActionResult GetTest()
        {
            return Ok(User.FindFirst("login")?.Value + " " + User.FindFirst("password")?.Value);
        }
    }
}
