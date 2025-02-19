using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ExchangeRatesApp.Domain;
using ExchangeRatesApp.Infrastructure.CqrsDispatcherPipelineBehaviors;
using ExchangeRatesApp.Infrastructure.Database;
using ExchangeRatesApp.Infrastructure.Database.Repositories;
using ExchangeRatesApp.Infrastructure.NbpServiceIntegration;
using ExchangeRatesApp.Application.TechnicalInterfaces;

namespace ExchangeRatesApp.Infrastructure
{
    public static class StartupInfrastructure
    {
        public static void Execute(IServiceCollection services, IAppSettings appSettings)
        {
            var assembly = Assembly.GetExecutingAssembly();

            AddDatabaseStack(services, appSettings, assembly);
            AddMediatRBehaviors(services);
            services.AddScopedServices(assembly);
            AddOtherItems(services);
        }

        private static void AddMediatRBehaviors(IServiceCollection services)
        {
            //attention: here the order matters
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LockedHandlerPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorsExecutionPipelineBehavior<,>));
        }

        private static void AddDatabaseStack(IServiceCollection services, IAppSettings appSettings, Assembly infraAssembly)
        {
            services.AddDbContext<ExchangeRatesAppDbContext>(options => options
                                            //.UseSqlServer(appSettings.MainConnectionString));
                                            .UseInMemoryDatabase("InMemoryDb"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddScopedRepositories(infraAssembly);
            services.AddScoped(typeof(IBaseRepository<>), typeof(Repository<>));
        }

        public static void AddOtherItems(IServiceCollection services)
        {
            services.AddSingleton<ApplicationContext>();
            services.AddScoped<IHandlerContext, HandlerContext>();
            services.AddHttpClient<ExchangeRateNbpRepository>();
        }
    }
}
