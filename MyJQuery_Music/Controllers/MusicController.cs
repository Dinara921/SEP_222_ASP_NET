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
        //string conStr = @"Server=DESKTOP-S23LER7;Database=ASP_MusicJQuery;Trusted_Connection=True;TrustServerCertificate=Yes;";
        string conStr = @"Server=206-11\SQLEXPRESS;Database=ASP_Music;Trusted_Connection=True;TrustServerCertificate=Yes;";

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

        [HttpGet("DownloadFormatMusic/{format}/{category}")]
        public ActionResult DownloadFormatMusic(int format, int category)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                try
                {
                    db.Open();

                    var musicList = db.Query<Music>("pMusic", new { category = category }, commandType: CommandType.StoredProcedure).ToList();

                    if (format == 1)
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

                                for (int i = 0; i < musicList.Count; i++)
                                {
                                    var music = musicList[i];
                                    ws.Cell(i + 2, 1).Value = music.id;
                                    ws.Cell(i + 2, 2).Value = music.name;
                                    ws.Cell(i + 2, 3).Value = music.category;
                                    ws.Cell(i + 2, 4).Value = music.duration;
                                }

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
                    else if (format == 2)
                    {

                        var builder = new StringBuilder();
                        builder.AppendLine("Id,Name,Category,Duration");
                        foreach (var music in musicList)
                        {
                            builder.AppendLine($"{music.id},{music.name},{music.category},{music.duration}");
                        }
                        byte[] csvBytes = Encoding.UTF8.GetBytes(builder.ToString());

                        return File(csvBytes, "text/csv", "music_report.csv");
                    }
                    else
                    {
                        return BadRequest("Unsupported format");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

    }
}
