using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using MyReact2.Abstract;
using MyReact2.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MyReact2.Service
{
    public class CityService : ICity
    {
        string conStr = @"Server=207-3;Database=testDB;Integrated Security=True;TrustServerCertificate=Yes";
        public string AddOrEdit(City model)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                DynamicParameters p = new DynamicParameters(model);
                return db.ExecuteScalar<string>("pCity", p, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<City> GetCityAll(string id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                return db.Query<City>("pCity;2", new { id = id }, commandType: CommandType.StoredProcedure); 
            }        
        }
    }
}
