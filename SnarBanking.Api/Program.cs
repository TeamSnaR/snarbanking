
using SnarBanking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

builder.Services
    .AddRouting()
    .ConfigureSnarBankingDbSettings(builder.Configuration.GetSection(SnarBanking.Storage.Settings.SnarBankingDbSettings.SectionName))
    .AddSnarBankingServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app
    .UseRouting()
    .UseEndpoints(
        endpoints => endpoints.UseSnarBankingEndpoints()
     )
    .ConfigureSnarBankingServices()
    .UseHttpsRedirection()
    .UseSwagger()
    .UseSwaggerUI();


app.Run();
