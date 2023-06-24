using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using SnarBanking.Expenses;

namespace SnarBanking;

public static class Configuration
{
    public static IServiceCollection AddSnarBankingServices(this IServiceCollection services)
    {
        // add db context
        // add services
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddExpensesServices();
        return services;
    }

    public static IEndpointRouteBuilder UseSnarBankingEndpoints(this IEndpointRouteBuilder endpoints) =>
        endpoints.UseExpensesEndpoints();

    public static IApplicationBuilder ConfigureSnarBankingServices(this IApplicationBuilder app)
    {
        // local db migration
        return app;
    }
}
