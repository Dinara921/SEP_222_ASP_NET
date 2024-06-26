﻿using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using MyWebAPIBasicAuth.Model;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace MyWebAPIBasicAuth.Auth
{
    public class BasicAuth : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        static string conStr = @"Server=206-11\SQLEXPRESS;Database=TestDB;Integrated Security=True;TrustServerCertificate=Yes";
        public BasicAuth(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No section Authorization");
            try
            {
                // "user1;password"
                var value = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credValue = Convert.FromBase64String(value.Parameter);
                var credArray = Encoding.UTF8.GetString(credValue).Split(':');
                var login = credArray[0];
                var psw = credArray[1];

                using (SqlConnection db = new SqlConnection(conStr))
                {
                    db.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@login", login);
                    parameters.Add("@pwd", psw);
                    parameters.Add("@res_out", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    db.Execute("pUser3", parameters, commandType: CommandType.StoredProcedure);

                    int cred = parameters.Get<int>("@res_out");

                    if (cred == 1)
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, login),
                            new Claim("pwd", psw)
                        };

                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Login Or Password Incorrect");
                    }
                }
            }
            catch (Exception err)
            {
                return AuthenticateResult.Fail(err.Message);
            }
        }
    }
}
