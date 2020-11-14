using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PurseApp.CurrencyIntegration;
using PurseApp.Hangfire;
using PurseApp.Helpers;
using PurseApp.Middlewares;
using PurseApp.Repositories;
using PurseApp.Services;

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
            services.AddDefaultIdentity<IdentityUser>(opt =>
                {
                    //opt.Password.RequireDigit = true;
                    opt.Password.RequiredLength = 1;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                    opt.Lockout.MaxFailedAccessAttempts = 3;
                    opt.User.RequireUniqueEmail = true;
                    opt.SignIn.RequireConfirmedEmail = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PurseAppDbContext>()
                .AddDefaultTokenProviders();
            
            services.ConfigureAuthentication(Configuration);
            
            // services.AddCors();
            services.AddScoped<IPurseRepository, PurseRepository>();
            services.AddScoped<IAccountRepository,AccountRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddHttpClient<ICurrencyApi, CurrencyApi>(client =>
            {
                client.BaseAddress = new Uri("http://www.cbr.ru");
                client.Timeout = TimeSpan.FromMinutes(1);
            });

            services.AddHangfireService(Configuration.GetConnectionString("DefaultConnection"));
            services.AddSwagger();
            services.AddControllers();
            
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfire();
            app.UseSwagger();
            
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
            app.UseCors(s => s.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
