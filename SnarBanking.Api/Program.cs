
using SnarBanking;

var builder = WebApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

builder.Services
    .ConfigureSnarBankingDbSettings(builder.Configuration.GetSection(SnarBanking.Storage.Settings.SnarBankingDbSettings.SectionName))
    .AddRouting()
    .AddSnarBankingServices()
    .AddThirdPartyServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app
    .UseRouting()
    .UseEndpoints(
        endpoints => endpoints.UseSnarBankingEndpoints()
     )
    .ConfigureSnarBankingServices(environmentName)
    .UseHttpsRedirection()
    .UseSwagger()
    .UseSwaggerUI();

app.Run();
