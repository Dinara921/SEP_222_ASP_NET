using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJQuery.Model;
using System.Data;
using System.Data.SqlClient;

namespace MyJQuery_Music.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        string conStr = @"Server=DESKTOP-S23LER7;Database=ASP_MusicJQuery;Trusted_Connection=True;TrustServerCertificate=Yes;";
        [HttpGet("GetAllOrCategoryMusic")]
        public ActionResult GetAllOrCategoryMusic(int category)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                db.Open();
                var res = db.Query<Music>("pMusic", new { category }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpGet("DeleteMusic")]
        public ActionResult DeleteMusic(int id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                db.Open();
                var res = db.Query<Music>("pMusic;3", new { id }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpPost("AddOrEditMusic")]
        public ActionResult AddOrEditMusic(int id, string name, string category, string duration )
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                db.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@Name", name);
                parameters.Add("@Category", category);
                parameters.Add("@Duration", duration);

                try
                {
                    db.Execute("pMusic;2", parameters, commandType: CommandType.StoredProcedure);
                    return Ok("Ok");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}
