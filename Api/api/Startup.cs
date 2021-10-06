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
using Microsoft.EntityFrameworkCore;
using api.Contexts;
using System.Text.Json;
using api.Services;
using AutoMapper;
using api.Helpers;
using Microsoft.IdentityModel.Tokens;
using JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using api.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace api
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
            // services.AddControllers(); //AddNewtonsoftJson(options =>
                //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);
            //services.AddMvc();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//Za koriscenje automapera
            services.AddDbContext<FarmieContext>(options=>{
            options.UseSqlServer(Configuration.GetConnectionString("Connection")).EnableSensitiveDataLogging();
            });
            services.AddScoped<IFarmieRepository, FarmieRepository>();
            services.AddControllers();
            services.AddMvc().AddJsonOptions(options=>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
            }).AddMvcOptions(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddCors(options=>{
                options.AddPolicy("CORS", builder=>{
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IFarmieRepository>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetUserById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // }).AddJwtBearer(options =>
            //     {
            //         options.Authority = "https://farmie-dev.eu.auth0.com/";
            //         options.Audience = "http://localhost:5001";
            //     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // else
            // {
            //      app.UseExceptionHandler("/Home/Error");
            // }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CORS");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
          // app.UseExceptionHandler("/Home/Error");

           //app.UseStaticFiles();

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //     name: "default",
            //     template: "{controller=Home}/{action=Index}/{id?}");
            // });
        }
    }
}
