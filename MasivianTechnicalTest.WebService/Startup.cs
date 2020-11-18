using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MasivianTechnicalTest.DataAccess.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasivianTechnicalTest.Domain.Contracts;
using Microsoft.OpenApi.Models;

namespace MasivianTechnicalTest.WebService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
            });
            services.AddScoped<IDataAccess, MasivianTechnicalTest.DataAccess.Redis.Implementations.Client>();
            services.AddScoped<IRoulette, MasivianTechnicalTest.Domain.Implementations.Roulette>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });         
            //});
            app.UseEndpoints(endpoints =>
            {
                /*
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Roulette}/{action=Create}"
                    );
                */
                endpoints.MapControllers();
            });
        }
    }
}
