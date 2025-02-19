using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
namespace ExchangeRatesApp.Api.StartupConfigs
{
    public static class AuthenticationConfig
    {
        public static void AddAppAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.AuthSettings.Key)),
                    ClockSkew = TimeSpan.FromMinutes(appSettings.AuthSettings.ClockSkewMinutes)
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[appSettings.AuthSettings.CookieOptionsSettings.Key];
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
