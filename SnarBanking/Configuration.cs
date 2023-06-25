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
    public static IServiceCollection ConfigureSnarBankingDbSettings(this IServiceCollection services, IConfigurationSection dbSettingsConfigurationSection) =>
        services
            .Configure<SnarBankingDbSettings>(dbSettingsConfigurationSection);

    public static IServiceCollection AddSnarBankingServices(this IServiceCollection services) =>
        services
            .AddSnarBankingDbSettings()
            .AddSnarBankingMongoDbService()
            .AddExpensesServices();

    public static IEndpointRouteBuilder UseSnarBankingEndpoints(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .UseExpensesEndpoints();

    public static IApplicationBuilder ConfigureSnarBankingServices(this IApplicationBuilder app, string environmentName) =>
        app
            .ConfigureSnarBankingMongoDbDevelopmentSeed(environmentName);

    public static IServiceCollection AddThirdPartyServices(this IServiceCollection services) =>
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}
