
using SnarBanking;
using SnarBanking.Api;
using SnarBanking.Api.ExceptionHandling;

using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

builder.Services
    .ConfigureSnarBankingDbSettings()
    .AddProblemDetails()
    .AddRouting()
    .AddSnarBankingServices()
    .AddThirdPartyServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app
    .UseSnarBankingGlobalExceptionHandler()
    .UseRouting()
    .UseEndpoints(
        endpoints => endpoints.UseSnarBankingEndpoints()
     )
    .ConfigureSnarBankingServices(environmentName)
    .UseHttpsRedirection()
    .UseSwagger()
    .UseSwaggerUI();

app.Run();

public partial class Program { }