﻿using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Ayatta.Web
{
    public class Startup
    {
        protected IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.CookieName = "x-xsrf-token";
                options.FormFieldName = "x-xsrf-token";
            });

            services.AddDistributedMemoryCache();

            services.AddDefaultStorage();

            //services.AddWeed();

            services.AddSession(options =>
            {
                options.CookieName = "x-session";
            });
            services.AddCart();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = "x-auth",
                LoginPath = "/sign-in",
                LogoutPath = "/sign-out",
                AccessDeniedPath = "/account/denied",
                ReturnUrlParameter = "redirect",
                AutomaticAuthenticate = true
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }
}