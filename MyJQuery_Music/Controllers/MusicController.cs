using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJQuery.Model;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
                var res = db.Query<Music>("pMusic", new { category }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpGet("DeleteMusic")]
        public ActionResult DeleteMusic(int id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<Music>("pMusic;3", new { id }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpPost("AddOrEditMusic")]
        public ActionResult AddOrEditMusic([FromBody] Music2 music)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                try
                {
                    db.Execute("pMusic;2", new { id = music.id, name = music.name, category_id = music.category_id, duration = music.duration }, commandType: CommandType.StoredProcedure);
                    return Ok("Ok");
                }

                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        [HttpPost("DownloadFormatMusic")]
        public ActionResult DownloadFormatMusic([FromBody] Format f)
        {
            if (f.format == 1)
            {
                using (var ms = new MemoryStream())
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.AddWorksheet("report");
                        ws.Cell(1, 1).Value = "Id";
                        ws.Cell(1, 1).Style.Font.Bold = true;
                        ws.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        ws.Cell(1, 2).Value = "Name";
                        ws.Cell(1, 2).Style.Font.Bold = true;
                        ws.Cell(1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        ws.Cell(1, 3).Value = "Category";
                        ws.Cell(1, 3).Style.Font.Bold = true;
                        ws.Cell(1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        ws.Cell(1, 4).Value = "Duration";
                        ws.Cell(1, 4).Style.Font.Bold = true;
                        ws.Cell(1, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        ws.RangeUsed().SetAutoFilter();
                        ws.Columns("A", "B").AdjustToContents();
                        ws.SheetView.FreezeRows(1);

                        wb.SaveAs(ms);
                        ms.Position = 0;
                        ms.Flush();
                        var bytes = ms.ToArray();

                        return File(bytes,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "report_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".xlsx");
                    }
                }
            }
            else if (f.format == 2)
            {
                List<Music> musicList;
                using (var db = new SqlConnection(conStr))
                {
                    string query = "SELECT id, name, category_id, duration FROM Music";
                    SqlCommand command = new SqlCommand(query, db);
                    db.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        musicList = new List<Music>();
                        while (reader.Read())
                        {
                            Music music = new Music
                            {
                                id = Convert.ToInt32(reader["id"]),
                                name = reader["name"].ToString(),
                                category = reader["category_id"].ToString(),
                                duration = reader["duration"].ToString()
                            };
                            musicList.Add(music);
                        }
                    }
                }

                var builder = new StringBuilder();
                builder.AppendLine("Id,Name,Category,Duration");
                foreach (var music in musicList)
                {
                    builder.AppendLine($"{music.id},{music.name},{music.category},{music.duration}");
                }
                byte[] csvBytes = Encoding.UTF8.GetBytes(builder.ToString());

                return File(csvBytes, "text/csv", "music_report.csv");
            }

            return BadRequest("Unsupported format");
        }
    }
}
