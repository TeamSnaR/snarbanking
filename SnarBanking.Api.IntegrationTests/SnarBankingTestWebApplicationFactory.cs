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
        builder
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.Sources.Clear();
                cfg
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Test.json");
            })
            .ConfigureLogging(cfg =>
            {
                cfg.ClearProviders();
            })
            .ConfigureServices(services =>
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

        return host;
    }
}
