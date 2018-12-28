using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Base.Middleware
{
    class CheckTokenMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly List<string> ExceptPaths;
        private readonly string RedirectOnErrorPath;

        public CheckTokenMiddleware(RequestDelegate Next, List<string> ExceptPaths, string RedirectOnErrorPath)
        {
            this.Next = Next;
            this.ExceptPaths = ExceptPaths;
            this.RedirectOnErrorPath = RedirectOnErrorPath;
        }

        public async Task Invoke(HttpContext Context)
        {
            var Cookies = Context.Request.Cookies;
            var Path =  Context.Request.Path.ToString().ToLower();
            if (!Utilities.CheckExceptPaths(ExceptPaths, Path))
            {
                if (Cookies.Count > 0)
                {
                    if (!Cookies.ContainsKey("accessToken"))
                    {
                        Context.Response.Redirect(RedirectOnErrorPath);
                    }
                    else
                    {
                        try
                        {
                            var Token = Cookies["accessToken"];
                            var Handler = new JwtSecurityTokenHandler();
                            var JsonToken = Handler.ReadJwtToken(Token);
                            if(JsonToken.ValidTo.Ticks <= DateTime.Now.Ticks)
                            {
                                Context.Response.Redirect(RedirectOnErrorPath);
                            }
                            Context.Request.Headers.Add("Token", "Bearer " + Token);
                        }
                        catch (Exception)
                        {
                            Context.Response.Redirect(RedirectOnErrorPath);
                        }
                    }
                }
                else
                {
                    Context.Response.Redirect(RedirectOnErrorPath);
                }
            }
            await Next(Context);
        }
    }
}
