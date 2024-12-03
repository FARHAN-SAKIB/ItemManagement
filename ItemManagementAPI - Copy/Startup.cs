using AuthJwt.Services;
using ItemManagementAPI.Models;
using ItemManagementAPI.Services;
using JwtAuth.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Sol_Demo_WebApi.Policies;

namespace ItemManagementAPI
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
            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDBSettings"));
            services.AddSingleton<ItemService>();
            services.AddSingleton<UserService>();
            services.AddCors(o => o.AddPolicy("AllowOrigins", builder =>
            {

                builder.WithOrigins("http://localhost:4200")
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();




            }));
     
            services
                .AddControllers()
                .AddJsonOptions((leSetUp) =>
                {
                    // Pascal Casing
                   // leSetUp.JsonSerializerOptions.PropertyNamingPolicy = null;

                    // Ignore Json Property Null Value from Response
                    leSetUp.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.Configure<AppSettingsModel>(Configuration.GetSection("Jwt"));
            var getSecretKey = Configuration.GetSection("Jwt").Get<AppSettingsModel>();

            services.AddJwtToken(getSecretKey.SecretKey,
                (authOption) =>
                {
                    authOption.AddPolicy("AdminPolicy", (policy) => policy.RequireRole("Admin"));// Role Base
                    authOption.AddPolicy("UserOnly", (policy) => policy.RequireClaim("UserID")); // Claim base
                    authOption.AddPolicy("Over21Only", (policy) => policy.Requirements.Add(new MinimumAgeRequirement(21))); // Policy Base
                }



                ); // Add Jwt Token Service

            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("AllowOrigins");
            app.UseJwtToken(); // Use Jwt Token Middleware

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
