using MongoDB.Bson.Serialization;
using System.Reflection;

using Ogooreck.API;
using static Ogooreck.API.ApiSpecification;
using MongoDB.Driver;
using SnarBanking.Api.IntegrationTests;
using SnarBanking.Api.IntegrationTests.Utilities;
using SnarBanking.Expenses;
using static SnarBanking.Storage.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;
using System.Text.Json;

namespace SnarBanking.Api.Tests.Expenses.GettingExpenses
{
    public class GetExpensesShould : ApiSpecification<Program>, IClassFixture<SnarBankingTestWebApplicationFactory>, IAsyncLifetime
    {
        private readonly SnarBankingTestWebApplicationFactory _factory;

        public GetExpensesShould(SnarBankingTestWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ValidRequest_With_NoParams_ShouldReturn_200()
        {
            var request = new SnarBanking.Expenses.GettingExpenses.GetExpenses.Query();
            var client = _factory.CreateClient();

            var result = await client.GetAsync("/api/expenses");
            var content = await JsonSerializer.DeserializeAsync<List<Expense>>(await result.Content.ReadAsStreamAsync());

            content.Count().ShouldBe(9);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public async Task InitializeAsync()
        {
            var pathToSeedTestData = Path.Combine(Path.GetDirectoryName(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location)), "Data", "TestData.json");
            using var stream = new StreamReader(pathToSeedTestData);
            var seedTestData = stream.ReadToEnd();
            var seedTestDataAsBson = BsonSerializer.Deserialize<IEnumerable<Expense>>(seedTestData);
            using var scope = _factory.Services.CreateScope();
            var snarBankingMongoDbService = scope.ServiceProvider.GetRequiredService<SnarBankingMongoDbService>();

            await snarBankingMongoDbService.ExpensesCollection.DeleteManyAsync(_ => true);
            await snarBankingMongoDbService.ExpensesCollection.InsertManyAsync(seedTestDataAsBson);
        }
    }
}

