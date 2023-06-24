using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SnarBanking.Expenses;
using SnarBanking.Storage;

using static SnarBanking.Storage.Settings;

namespace SnarBanking;

public static class Configuration
{
    public static IServiceCollection ConfigureSnarBankingDbSettings(this IServiceCollection services, IConfigurationSection configurationSection) =>
        services
            .Configure<SnarBankingDbSettings>(configurationSection);

    public static IServiceCollection AddSnarBankingServices(this IServiceCollection services) =>
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddSnarBankingDbSettings()
            .AddSnarBankingMongoDbService()
            .AddExpensesServices();

    public static IEndpointRouteBuilder UseSnarBankingEndpoints(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .UseExpensesEndpoints();

    public static IApplicationBuilder ConfigureSnarBankingServices(this IApplicationBuilder app) =>
        app
            .ConfigureSnarBankingMongoDbSeed();
}
