using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCookie_HW.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class CookieController : ControllerBase
    {
        [HttpGet("SetCookie")]
        public IActionResult SetCookie()
        {
            DateTime now = DateTime.Now;
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"current_datetime", now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"month_number", now.Month},
                {"week_number", GetIsoWeekNumber(now)},
                {"day_of_week", now.DayOfWeek.ToString()},
                {"current_season", GetSeason(now)},
                {"ordinal_day_of_year", now.DayOfYear}
            };

            Response.Cookies.Append("cookie", Newtonsoft.Json.JsonConvert.SerializeObject(data));

            return Ok("Data saved in cookie.");
        }

        [HttpGet("GetCookie")]
        public IActionResult GetCookie()
        {
            if (Request.Cookies.TryGetValue("cookie", out string cookieData))
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(cookieData);
                return Ok(data);
            }

            return NotFound("Cookie data not found.");
        }

        private int GetIsoWeekNumber(DateTime date)
        {
            return System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private string GetSeason(DateTime date)
        {
            int month = date.Month;

            if (month >= 3 && month <= 5)
                return "Spring";
            else if (month >= 6 && month <= 8)
                return "Summer";
            else if (month >= 9 && month <= 11)
                return "Autumn";
            else
                return "Winter";
        }
    }
}
