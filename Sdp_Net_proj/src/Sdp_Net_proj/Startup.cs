using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Sdp_Net_proj.Services;
using Sdp_Net_proj.Models;

namespace Sdp_Net_proj
{
    public class Startup
    {

        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        //private IRipository

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json"); // a way to add environment variables
                                             //.AddEnvironmentVariables(); // allow you to override environment variables.

        _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development"))
            {
                services.AddScoped<IMailService,MailService>();
            }
            else
            {
                services.AddScoped<IMailService, MailService>();// Implement a real Mail Service - maybe will need changes.
            }

            services.AddDbContext<WorldContext>();

            services.AddLogging();

            services.AddMvc().
                AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }
                    );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
             ILoggerFactory factory)
        {

            if (env.IsDevelopment()) // display error data when in development.
            {
                app.UseDeveloperExceptionPage();
                factory.AddDebug(LogLevel.Information);
            }
            else
            {
                factory.AddDebug(LogLevel.Error);
            }

            app.UseStaticFiles();

            app.UseMvc(config =>
            {
                config.MapRoute( // this method take a pattern of a URL and map it to a method.
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { Controller = "App", action = "index" }
                    );
            });
        }
    }
}
