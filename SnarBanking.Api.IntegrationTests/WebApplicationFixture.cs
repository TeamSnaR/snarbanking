
using System.Reflection;

using MediatR;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using SnarBanking.Expenses;

using static SnarBanking.Storage.Service;

namespace SnarBanking.Api.IntegrationTests
{
    public class WebApplicationFixture : IAsyncLifetime, IDisposable
    {
        public readonly SnarBankingTestWebApplicationFactory _factory;
        public readonly IServiceProvider _serviceProvider;

        public WebApplicationFixture()
        {
            _factory = new SnarBankingTestWebApplicationFactory();
            _serviceProvider = _factory.Services;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public async Task SendASync(IRequest request)
        {
            using var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            await mediator.Send(request);
        }

        public async Task InitializeAsync()
        {
            var pathToSeedTestData = Path.Combine(Path.GetDirectoryName(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location))!, "Data", "TestData.json");

            using var stream = new StreamReader(pathToSeedTestData);
            var seedTestData = stream.ReadToEnd();
            var seedTestDataAsBson = BsonSerializer.Deserialize<IEnumerable<Expense>>(seedTestData);
            using var scope = _factory.Services.CreateScope();
            var snarBankingMongoDbService = scope.ServiceProvider.GetRequiredService<SnarBankingMongoDbService>();
            await snarBankingMongoDbService.ExpensesCollection.DeleteManyAsync(_ => true);
            await snarBankingMongoDbService.ExpensesCollection.InsertManyAsync(seedTestDataAsBson);
        }

        public async Task DisposeAsync()
        {
            //using var scope = _factory.Services.CreateScope();
            //var snarBankingMongoDbService = scope.ServiceProvider.GetRequiredService<SnarBankingMongoDbService>();

            //return snarBankingMongoDbService.ExpensesCollection.DeleteManyAsync(_ => true);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }
    }
}

