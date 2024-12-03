
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AuthJwt.Services
{
    public static class JwtTokenServiceExtension
    {
        public static void AddJwtToken(this IServiceCollection services, string secretKey, Action<AuthorizationOptions> authOption = null)

        {
          //  services.AddTransient<IGenerateJwtToken, GenerateJwtToken>();

            var key = Encoding.ASCII.GetBytes(secretKey);
            services
                .AddAuthorization(x => authOption?.Invoke(x))
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    /////////////////
                    // Adding JWT event hooks
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Example: Extract token from query string if needed
                            if (string.IsNullOrEmpty(context.Token) && context.Request.Query.ContainsKey("authorization"))
                            {
                                context.Token = context.Request.Query["authorization"];
                            }
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            // Example: Additional logic after token validation
                            var userPrincipal = context.Principal;
                            // Custom logic, such as logging or additional claim checks
                            Console.WriteLine("Token successfully validated.");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            // Custom response for authentication challenges
                            context.HandleResponse(); // Prevents default redirect behavior
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"Unauthorized access\"}");
                        },
                        OnAuthenticationFailed = context =>
                        {
                            // Logging or handling authentication failures
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"Authentication failed\"}");
                        },
                        OnForbidden = context =>
                        {
                            // Custom response for forbidden access
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"Access forbidden\"}");
                        }
                    };
                    /// ////////////
                });
        }
    }
}

