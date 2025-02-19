using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;
using ExchangeRatesApp.Api.StartupConfigs;
using ExchangeRatesApp.Domain;
using ExchangeRatesApp.Infrastructure;
using ExchangeRatesApp.Infrastructure.CqrsDispatcherPipelineBehaviors;
using ExchangeRatesApp.Infrastructure.Database;
using ExchangeRatesApp.Infrastructure.Database.Repositories;
using ExchangeRatesApp.Application.TechnicalInterfaces;

namespace ExchangeRatesApp.Tests.Common
{
    public abstract class BaseTestContainer
    {
        protected readonly ICurrentUser _currentUser;
        protected readonly ExchangeRatesAppDbContext _dbContext;
        protected readonly IMediator _mediator;
        protected readonly ServiceProvider _serviceProvider;

        protected BaseTestContainer()
        {
            _currentUser = this.MockCurrentUser(Guid.NewGuid().ToString());
            _dbContext = this.CreateInMemoryDbContext(this);

            var context = new DefaultHttpContext();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(hca => hca.HttpContext).Returns(context);

            var applicationAssembly = Assembly.GetAssembly(typeof(IHandlerContext))!;

            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped(sp => _currentUser)
                .AddScoped(sp => mockHttpContextAccessor.Object)
                .AddMediatR(applicationAssembly)
                .AddSingleton(sp => this.CreateConfiguration())
                .AddSingleton<IAppSettings, AppSettings>()
                .AddScoped(sp => _dbContext)
                .AddScoped(typeof(IBaseRepository<>), typeof(Repository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddValidatorsFromAssembly(applicationAssembly)
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionPipelineBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorsExecutionPipelineBehavior<,>))
                .AddSingleton<ApplicationContext>()
                .AddScoped<IHandlerContext, HandlerContext>()
                .BuildServiceProvider();
            _mediator = _serviceProvider.GetRequiredService<IMediator>();
        }
    }
}
