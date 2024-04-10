using ASP_Ekz.Model;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ASP_Ekz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymController : ControllerBase
    {
        //string conStr = @"Server=DESKTOP-S23LER7;Database=ASP_Ekz;Trusted_Connection=True;TrustServerCertificate=Yes;";

        string conStr = @"Server=206-11\SQLEXPRESS;Database=ASP_Ekz;Trusted_Connection=True;TrustServerCertificate=Yes;";

        [HttpGet("AdminOrUser")]
        public ActionResult AdminOrUser(string email, string pwd)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<CategoryUser>("pIsAdmin", new { email, pwd }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }
    }
}
