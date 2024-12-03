//using ItemManagementAPI.Models;
//using ItemManagementAPI.Services;
//using Microsoft.Extensions.Options;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using System.Security.Claims;
//using AspNetCore.Identity.MongoDbCore.Models;
//using Microsoft.AspNetCore.Identity;

//var builder = WebApplication.CreateBuilder(args);

//Add services to the container
//builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
//builder.Services.AddSingleton<ItemService>();
//builder.Services.AddSingleton<UserService>();

//Register Identity services for role management
//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
//    .AddMongoDbStores<ApplicationUser, ApplicationRole, string>("mongodb://localhost:27017", "ItemDB")
//    .AddRoles<ApplicationRole>()
//    .AddDefaultTokenProviders();

//Add CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularApp", policy =>
//    {
//        policy.WithOrigins("*") // URL of frontend
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

//Add JWT Authentication

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//                                            {
//                                                ValidateIssuer = true,
//                                                ValidateAudience = true,
//                                                ValidateLifetime = true,
//                                                ValidateIssuerSigningKey = true,
//                                                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//                                                ValidAudience = builder.Configuration["JwtSettings:Audience"],
//                                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
//                                            };

//Register JWT hooks
//        options.Events = new JwtBearerEvents
//                         {
//                             OnMessageReceived = context =>
//                             {
//                                 Custom logic for extracting tokens if needed

//                                 return Task.CompletedTask;
//                             },
//                             OnTokenValidated = context =>
//                             {
//                                 Logic after successful token validation

//                                 return Task.CompletedTask;
//                             },
//                             OnChallenge = context =>
//                             {
//                                 Custom challenge response

//                                 context.HandleResponse();
//                                 context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                                 context.Response.ContentType = "application/json";
//                                 return context.Response.WriteAsync("{\"error\": \"Unauthorized access\"}");
//                             },
//                             OnAuthenticationFailed = context =>
//                             {
//                                 Handle authentication failure

//                                 Console.WriteLine($"Authentication failed: {context.Exception.Message}");
//                                 return Task.CompletedTask;
//                             },
//                             OnForbidden = context =>
//                             {
//                                 Custom forbidden response

//                                 context.Response.StatusCode = StatusCodes.Status403Forbidden;
//                                 context.Response.ContentType = "application/json";
//                                 return context.Response.WriteAsync("{\"error\": \"Access forbidden\"}");
//                             }
//                         };
//    });


//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
//});


///*
//builder.Services.AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
//        };
//    });
//*/
///*builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy =>
//    {
//        policy.RequireRole("Admin");
//    });
//    options.AddPolicy("UserPolicy", policy =>
//    {
//        policy.RequireRole("User");
//    });
//});
//*/
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//app.UseCors("AllowAngularApp");

//Enable Swagger
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//app.UseSwaggerUI();
//}

//app.UseAuthentication();
//app.UseAuthorization();

//app.UseHttpsRedirection();



//app.MapControllers();

//Use a service scope to resolve RoleManager
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
//    await RoleSeeder.SeedRolesAsync(roleManager);
//}

//app.Run();



using ItemManagementAPI;

///*
///*builder.Services.AddAuthorization(options =>
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}