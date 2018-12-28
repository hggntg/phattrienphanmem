using Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineReading.APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOcelot(Configuration);
        }

        private async Task<bool> GenerateErrorResponse(DownstreamContext Context, int StatusCode, string ContentType, string Status, string Message)
        {
            var StateResponse = CustomHttpResponse.State();
            Context.HttpContext.Response.StatusCode = StatusCode;
            Context.HttpContext.Response.ContentType = ContentType;
            StateResponse.Status = Status;
            StateResponse.Message = Message;
            var Response = JsonConvert.SerializeObject(StateResponse);
            var ResponseBytes = Encoding.UTF8.GetBytes(Response);
            await Context.HttpContext.Response.Body.WriteAsync(ResponseBytes, 0, ResponseBytes.Length);
            return true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            var Configuration = new OcelotPipelineConfiguration
            {
                PreAuthenticationMiddleware = async (Context, Next) =>
                {
                    await Next.Invoke();
                    List<string> ExceptPaths = new List<string>();
                    ExceptPaths.Add("/api/auth/login");
                    ExceptPaths.Add("/api/account/signup");
                    ExceptPaths.Add("/api/file/{filename}");
                    ExceptPaths.Add("/api/tag/list");
                    ExceptPaths.Add("/api/tag/detail/{id}");
                    ExceptPaths.Add("/api/category/list");
                    ExceptPaths.Add("/api/category/detail/{id}");
                    ExceptPaths.Add("/api/story/public-list");
                    ExceptPaths.Add("/api/story/public-detail/{id}");
                    string Path = Context.HttpContext.Request.Path.ToString();
                    if (!Utilities.CheckExceptPaths(ExceptPaths, Path))
                    {
                        var Token = Context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "Token").Value.ToString();

                        if (string.IsNullOrEmpty(Token))
                        {
                            await this.GenerateErrorResponse(Context, 403, "application/json", "Forbidden", "Missing token");
                        }
                        else
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
                                var CurrentTicks = DateTime.Now.Ticks;
                                var ExpireTicks = JsonToken.Payload.ValidTo.Ticks;
                                if (ExpireTicks <= CurrentTicks)
                                {
                                    await this.GenerateErrorResponse(Context, 401, "application/json", "Unauthorized", "Token Expired");
                                    return;
                                }
                                else
                                {
                                    Context.DownstreamRequest.Headers.Add("Token", "Bearer " + Token);
                                }

                            }
                            catch (Exception)
                            {
                                await this.GenerateErrorResponse(Context, 401, "application/json", "Unauthorized", "Token Invalid");
                                return;
                            }

                        }
                    }
                }
            };
            app.UseCors(builder => {
                builder.WithHeaders("Token", "Content-Type");
                builder.WithMethods("OPTIONS", "GET", "POST", "PUT", "DELETE");
                builder.WithOrigins("*");
            });
            await app.UseOcelot(Configuration);
        }
    }
}
