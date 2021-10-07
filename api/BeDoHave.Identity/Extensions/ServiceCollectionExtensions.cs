using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using BeDoHave.Identity.Contexts;
using BeDoHave.Identity.Models;
using BeDoHave.Identity.Options;
using BeDoHave.Shared.Interfaces;
using BeDoHave.Identity.Services;
using System.IO;

namespace BeDoHave.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                //.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BeDoHave.Identity"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            services.AddDbContext<IdentityAppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DocumentDatabaseConnection"),
                    assembly => assembly.MigrationsAssembly(typeof(IdentityAppDbContext).Assembly.FullName));
            //x => x.MigrationsAssembly("BeDoHave.Identity"));
            });
            services.AddIdentity<IdentityAppUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityAppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddSingleton(typeof(JwtSecurityTokenHandler));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));

            services.Configure<JwtOption>(option =>
            {
                option.Audience = configuration["JWT:Audience"];
                option.Issuer = configuration["JWT:Issuer"];
                option.SigningKey = configuration["JWT:SigningKey"];
                option.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(config =>
            {
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = signingKey,
                };
            });

            

            return services;
        }
    }
}
