using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PurseApp.CurrencyIntegration;
using PurseApp.Repositories;

namespace PurseApp
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
            services.AddDbContext<PurseAppDbContext>(s =>
                s.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPurseRepository, PurseRepository>();
            services.AddScoped<IAccountRepository,AccountRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            
            services.AddHttpClient<ICurrencyApi, CurrencyApi>(client =>
            {
                client.BaseAddress = new Uri("http://www.cbr.ru");
                client.Timeout = TimeSpan.FromMinutes(1);
            }) ;
            
            services.AddControllers();
            services.AddSwaggerGen(s => s.SwaggerDoc("v1", new OpenApiInfo{Title = "Purse App V1", Version = "v1"}));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}