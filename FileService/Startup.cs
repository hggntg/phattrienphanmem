using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Base;
using Base.APIBuilder;
using Base.Middleware.Extensions;
using Service;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.IO;

namespace FileService
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
            services.AddDbContext<FileContext>();
            services
                .AddHttpClient<AccountService, AccountServiceImp>(client =>
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
            ExceptPaths.Add("/api/file/{filename}");
            app
                .UseJustGatewayMiddleware(new HeaderRequirementImp("apigateway", "from:apigateway;to:fileservice", "d406ad44627565b18bedcb6ce3ea3cb3"))
                .UseGetUserMiddleware(ExceptPaths)
                .UseExceptionMiddleware()
                .UseMvc();
        }
    }
}
