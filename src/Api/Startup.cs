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
using Microsoft.OpenApi.Models;

namespace Tutorial.Api
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
            services.AddLogging(builder => builder.AddSeq());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tutorial.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tutorial.Api v1"));
            }

            lifetime.ApplicationStarted.Register(() => logger.LogInformation("Application started"));
            lifetime.ApplicationStopping.Register(() => logger.LogInformation("Application stopping"));
            lifetime.ApplicationStopped.Register(() => logger.LogInformation("Application stopped"));

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                try
                {
                    logger.LogInformation("[{Type}] {Path}", "Request", context.Request.Path);
                    await next();
                    logger.LogInformation("[{Type}] {Path}", "Response", context.Request.Path);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "[{Type}] {Path}", "Error", context.Request.Path);
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
