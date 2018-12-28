using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAdmin.Services;
using System;
using System.IO;
using Base.Model;
using Base.APIBuilder;
using AutoMapper;
using Base.Middleware.Extensions;

namespace WebAdmin
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
            var AppAPISettingsSection = Configuration.GetSection("AppAPISettings");
            var AppAPISettings = AppAPISettingsSection.Get<AppAPISettings>();
            services.AddMvc();
            services
                .AddHttpClient<AccountService, AccountServiceImp>(client =>
                {
                    client.BaseAddress = new Uri(Path.Combine(AppAPISettings.Host, AppAPISettings.Services["Account"]));
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policy.GetRetryPolicy())
                .AddPolicyHandler(Policy.GetCircuitBreakerPolicy());
            services
                .AddHttpClient<AuthService, AuthServiceImp>(client =>
                {
                    client.BaseAddress = new Uri(Path.Combine(AppAPISettings.Host, AppAPISettings.Services["Auth"]));
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policy.GetRetryPolicy())
                .AddPolicyHandler(Policy.GetCircuitBreakerPolicy());
            services
                .AddHttpClient<StoryService, StoryServiceImp>(client =>
                {
                    client.BaseAddress = new Uri(Path.Combine(AppAPISettings.Host, AppAPISettings.Services["Story"]));
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policy.GetRetryPolicy())
                .AddPolicyHandler(Policy.GetCircuitBreakerPolicy());
            services
                .AddHttpClient<CategoryService, CategoryServiceImp>(client =>
                {
                    client.BaseAddress = new Uri(Path.Combine(AppAPISettings.Host, AppAPISettings.Services["Category"]));
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policy.GetRetryPolicy())
                .AddPolicyHandler(Policy.GetCircuitBreakerPolicy());
            services
                .AddHttpClient<TagService, TagServiceImp>(client =>
                {
                    client.BaseAddress = new Uri(Path.Combine(AppAPISettings.Host, AppAPISettings.Services["Tag"]));
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(Policy.GetRetryPolicy())
                .AddPolicyHandler(Policy.GetCircuitBreakerPolicy());
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var CheckTokeningSection = Configuration.GetSection("CheckTokening");
            var CheckTokening = CheckTokeningSection.Get<CheckTokening>();

            app.UseStaticFiles();
            app.UseCheckTokenMiddleware(CheckTokening.ExceptPaths, CheckTokening.RedirectOnErrorPath);
            app.UseMvc(routes =>
            {
                routes
                .MapRoute(
                    "login",
                    "login",
                    new { controller = "Account", action = "Login" }
                )
                .MapRoute(
                    "userUpdate",
                    "user/update",
                    new { controller = "User", action = "Update" }
                )
                .MapRoute(
                    "userCreate",
                    "user/create",
                    new { controller = "User", action = "Create" }
                )
                .MapRoute(
                    "userDetail",
                    "user/{Id}",
                    new { controller = "User", action = "Detail" }
                )
                .MapRoute(
                    "categoryChoose",
                    "story/choose-category",
                    new { controller = "Story", action = "ChooseCategory" }
                )
                .MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
