
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

using SnarBanking;
using SnarBanking.Api.ExceptionHandling;

var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .Build();
var seqUrl = configuration.GetValue<string>("Seq:Url")!;
var apiKey = configuration.GetValue<string>("Seq:ApiKey")!;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Seq(seqUrl, apiKey: apiKey)
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Seq(seqUrl, apiKey: apiKey)
    );
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
        .UseSerilogRequestLogging()
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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }