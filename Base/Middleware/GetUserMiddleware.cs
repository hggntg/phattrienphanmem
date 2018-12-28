using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Middleware
{
    class GetUserMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly List<string> ExceptPaths;

        public GetUserMiddleware(RequestDelegate Next, List<string> ExceptPaths)
        {
            this.Next = Next;
            this.ExceptPaths = ExceptPaths;
        }

        public async Task Invoke(HttpContext Context)
        {
            var Token = Context.Request.Headers.FirstOrDefault(h => h.Key == "Token").Value.ToString();
            string Path = Context.Request.Path.ToString();
            if (!Utilities.CheckExceptPaths(ExceptPaths, Path))
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    try
                    {
                        if (!Token.Contains("Bearer") || ((Token.Contains("Bearer") && !Token.StartsWith("Bearer"))))
                        {
                            throw new Exception("Invalid Token");
                        }
                        Token = Token.Replace("Bearer", "").Trim();
                        var Handler = new JwtSecurityTokenHandler();
                        var JsonToken = Handler.ReadJwtToken(Token);
                    }
                    catch (Exception)
                    {
                        Context.Response.StatusCode = 401;
                        var StateResponse = CustomHttpResponse.State();
                        StateResponse.Status = "Unauthorized";
                        StateResponse.Message = "Token Invalid";
                        var ResponseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(StateResponse));
                        await Context.Response.Body.WriteAsync(ResponseBytes, 0, ResponseBytes.Length);
                        return;
                    }
                    await Next(Context);
                }
                else
                {
                    Context.Response.StatusCode = 403;
                    var StateResponse = CustomHttpResponse.State();
                    StateResponse.Status = "Forbidden";
                    StateResponse.Message = "Missing token";
                    var ResponseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(StateResponse));
                    await Context.Response.Body.WriteAsync(ResponseBytes, 0, ResponseBytes.Length);
                }
            }
            else
            {
                await Next(Context);
            }
        }
    }
}
