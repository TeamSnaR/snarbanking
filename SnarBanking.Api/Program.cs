using SnarBanking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddRouting()
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
