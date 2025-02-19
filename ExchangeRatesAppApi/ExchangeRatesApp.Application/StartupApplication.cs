using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExchangeRatesApp.Application
{
    public static class StartupApplication
    {
        public static void Execute(IServiceCollection services)
        {
            var applicationAssembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(applicationAssembly);
            services.AddScopedServices(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);
        }
    }
}
