using Dapper;
using DapperToObject.Model;
using Newtonsoft.Json;
using RestSharp;
using System.Collections;
using System.Data.SqlClient;

namespace DapperToObject
{
    internal class Program
    {
        static string conStr = @"Server=206-11\SQLEXPRESS;Database=TestDB;Integrated Security=True;TrustServerCertificate=Yes";

        static void Main(string[] args)
        {
            //test_1("1");

            var client = new RestClient();
            var request = new RestRequest("http://localhost:5198/music/GetAllOrCategoryMusic?category=1", Method.Get);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        static void test_1(string id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                string sql = "SELECT id, name FROM Country WHERE id = " + id;
                sql += "SELECT id, name, country_id FROM City WHERE id = " + id;
                var multy = db.QueryMultiple(sql);
                Country country = multy.Read<Country>().FirstOrDefault();
                var citi = multy.Read<City>().ToList();
                //country.city = new List<City>();
                foreach (var item in citi)
                {
                    country.city.Add(item); 
                    Console.WriteLine(country.name);
                    Console.WriteLine(item.name);
                }
            }
        }
        static void test_2(string id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var sql = "select co.*, city.* from country co join city on co.id = city.country_id where co.id = 1 for json auto, Without_Array_Wrapper";
                var json = db.ExecuteScalar<string>(sql);
                var country =JsonConvert.DeserializeObject<Country>(json);
            }
        }

        static void test_3(string id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var sql3 = @"select co.id co_id, co.name co_name, ci.id ci_id, ci.name ci_name, ci.country_id
                from country co join city ci on co.id = ci.country_id
                where co.id = " + id;

                var result = db.Query<dynamic>(sql3);

                var co = (from p in result
                          select new Model.Country
                          {
                              id = p.co_id,
                              name = p.co_name,
                              city = (from z in result
                                      select new Model.City
                                      {
                                          id = p.ci_id,
                                          name = p.ci_name,
                                          country_id = p.country_id
                                      }).ToList()
                          }).FirstOrDefault();
            }
        }

        static void test_4(string id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var sql3 = @"select co.id co_id, co.name co_name, ci.id ci_id, ci.name ci_name, ci.country_id
                from country co join city ci on co.id = ci.country_id
                where co.id = " + id;

                var result = db.Query<dynamic>(sql3);

                var json = JsonConvert.SerializeObject(
                 result
                     .Select
                     (z => new
                            {
                                id = z.co_id,
                                name = z.co_name,
                                city = result
                                            .Select(z => new
                                            {
                                                id = z.ci_id,
                                                name = z.ci_name,
                                                country_id = z.country_id
                                            })

                            })
                            .FirstOrDefault()
                     );
                var country = JsonConvert.DeserializeObject<Model.Country>(json);
            }
        }
    }
}
