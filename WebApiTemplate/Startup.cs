using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WebApiTemplate.Middlewares;
using WebApiTemplate.Models;

namespace WebApiTemplate
{
    public class Startup
    {
        
        private readonly string _contentRootPath;
        private readonly bool _isDevelop;
        
        public Startup(IWebHostEnvironment env)
        {
            _contentRootPath = env.ContentRootPath;
            _isDevelop = env.IsDevelopment();
            var factory = LoggerFactory.Create(o => o.AddSerilog());
            var logger = factory.CreateLogger(typeof(Startup));
            IConfigurationBuilder configuration = new ConfigurationBuilder();
            configuration.SetBasePath($"{_contentRootPath}/Configs");
            string configPath;
            if (_isDevelop)
            {
                configPath = "app.dev.yaml";
            }
            else
            {
                configPath = "app.prod.yaml";
            }
            
            configuration.AddYamlFile(configPath);
            
            logger.LogInformation("Using config: {cfg}", configPath);
            Settings settings = new Settings();
            
            configuration.Build().Bind(settings);

        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseCors(e =>
            {
                e.AllowAnyHeader();
                e.AllowAnyMethod();
                e.AllowAnyOrigin();
            });
            
            app.UseErrorLogging();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}