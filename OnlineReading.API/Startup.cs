using System.Collections.Generic;
using Base.Middleware.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineReading.API.Infrastructure;
using AutoMapper;
using OnlineReading.API.Services;
using System;
using Base.APIBuilder;
using System.IO;

namespace OnlineReading.API
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
            var IdentityAPISettingsSection = this.Configuration.GetSection("IdentityAPISettings");
            var IdentityAPISettings = IdentityAPISettingsSection.Get<AppAPISettings>();
            services.AddDbContext<StoryContext>();
            services.AddHttpClient<AccountService, AccountServiceImp>(client =>
                {
                    client.BaseAddress = new Uri(Path.Combine(IdentityAPISettings.Host, IdentityAPISettings.Services["Account"]));
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policy.GetRetryPolicy())
                .AddPolicyHandler(Policy.GetCircuitBreakerPolicy());
            services.AddAutoMapper();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            List<string> ExceptPaths = new List<string>();
            ExceptPaths.Add("/api/tag/list");
            ExceptPaths.Add("/api/tag/detail/{id}");
            ExceptPaths.Add("/api/category/list");
            ExceptPaths.Add("/api/category/detail/{id}");
            ExceptPaths.Add("/api/story/public-list");
            ExceptPaths.Add("/api/story/public-detail/{id}");
            app.UseGetUserMiddleware(ExceptPaths)
               .UseMvc();
        }
    }
}
