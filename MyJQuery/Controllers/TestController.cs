﻿using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJQuery.Model;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MyJQuery.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        string conStr = @"Server=DESKTOP-S23LER7;Database=ASP_NET;Trusted_Connection=True;TrustServerCertificate=Yes;";

        [HttpGet("SayHello")]
        public ActionResult SayHello()
        {
            AddCorsHeaders();
            using (SqlConnection db = new SqlConnection(conStr))
            {
                db.Open();
                var res = db.Query<City>("pCity", commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpGet("GetCityAll")]
        public ActionResult GetCityAll()
        {
            AddCorsHeaders();
            List<City> list = new List<City>()
            {
                new City { id=1, name = "Astana"},
                new City { id=2, name = "Almaty"},
                new City { id=3, name = "Aktau"},
                new City { id=4, name = "Qostanay"}
            };
            return Ok(list);
        }

        private void AddCorsHeaders()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        }
    }
}