using Microsoft.AspNetCore.Mvc.Testing;

using MongoDB.Driver;

using SnarBanking.Storage;

using static SnarBanking.Storage.Service;
using static SnarBanking.Storage.Settings;

namespace SnarBanking.Api.IntegrationTests;

public class SnarBankingTestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(cfg =>
        {
            cfg.Sources.Clear();
            cfg
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json");
        });
        builder.ConfigureServices(services =>
        {
            services
                .Remove<IMongoDatabase>()
                .Remove<SnarBankingMongoDbService>()
                .Remove<SnarBankingDbSettings>();

            services
                .AddTransient(sp =>
                {
                    SnarBankingDbSettings snarBankingDbSettings = new();
                    sp
                        .GetRequiredService<IConfiguration>()
                        .GetRequiredSection(SnarBankingDbSettings.SectionName)
                        .Bind(snarBankingDbSettings);
                    return snarBankingDbSettings;
                });

            services
                .AddSnarBankingMongoDbService();
        });
        builder.UseEnvironment("Development");

        var host = base.CreateHost(builder);

        //using var scope = host.Services.CreateScope();
        //var database = scope.ServiceProvider.GetRequiredService<WarehouseDBContext>().Database;
        //database.ExecuteSqlRaw("TRUNCATE TABLE \"Product\"");

        return host;
    }
    //protected override void ConfigureWebHost(IWebHostBuilder builder)
    //{
    //    builder.ConfigureServices(services =>
    //    {
    //        services
    //            .Remove<IMongoDatabase>()
    //            .Remove<SnarBankingMongoDbService>()
    //            .Remove<SnarBankingDbSettings>();

    //        services
    //            .AddTransient(sp =>
    //            {
    //                SnarBankingDbSettings snarBankingDbSettings = new();
    //                sp
    //                    .GetRequiredService<IConfiguration>()
    //                    .GetRequiredSection(SnarBankingDbSettings.SectionName)
    //                    .Bind(snarBankingDbSettings);
    //                return snarBankingDbSettings;
    //            });

    //        services
    //            .AddSnarBankingMongoDbService();
    //    });
    //    builder.UseEnvironment("Development");
    //}
}
