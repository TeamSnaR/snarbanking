using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using SnarBanking.Expenses;

using static SnarBanking.Storage.Settings;
using static SnarBanking.Storage.Specifications;

namespace SnarBanking.Storage
{
    public static class Service
    {
        public class SnarBankingMongoDbService
        {
            private readonly IMongoCollection<Expense> _expensesCollection;
            private readonly FilterDefinition<Expense> _matchAll;

            public SnarBankingMongoDbService(SnarBankingDbSettings snarBankingDbSettings)
            {
                var settings = MongoClientSettings.FromConnectionString(snarBankingDbSettings.ConnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(snarBankingDbSettings.DatabaseName);

                _expensesCollection = database.GetCollection<Expense>(snarBankingDbSettings.DefaultCollectionName);
                _matchAll = FilterDefinition<Expense>.Empty;
            }

            public Task<List<Expense>> GetExpensesAsync(FilterDefinition<Expense>? match) => _expensesCollection.Find(match ?? _matchAll).ToListAsync();
            public Task<Expense> GetOneExpenseAsync(FilterDefinitionSpecification<Expense> specification) =>
                _expensesCollection.Find(specification.IsSatisfiedBy()).FirstOrDefaultAsync();

            public Task CreateOneExpenseAsync(Expense expense) => _expensesCollection.InsertOneAsync(expense);
            public Task CreateManyExpensesAsync(IEnumerable<Expense> expenses) => _expensesCollection.InsertManyAsync(expenses);
            public Task DeleteManyExpensesAsync(FilterDefinition<Expense>? match) => _expensesCollection.DeleteManyAsync(match ?? _matchAll);
        }

        public static IServiceCollection AddSnarBankingMongoDbService(this IServiceCollection services) =>
            services.AddSingleton<SnarBankingMongoDbService>();

        public static IApplicationBuilder ConfigureSnarBankingMongoDbSeed(this IApplicationBuilder app)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            if (environment == "Development")
            {
                var snarBankingMongoDbService = app.ApplicationServices.CreateScope().ServiceProvider
                    .GetRequiredService<SnarBankingMongoDbService>();

                snarBankingMongoDbService.EnsureCreated();
            }
            return app;
        }

        internal static SnarBankingMongoDbService EnsureCreated(this SnarBankingMongoDbService s)
        {
            IEnumerable<Expense> seedExpenses = Enumerable.Range(1, 5).Select(item =>
            {
                return new Expense($"Description {item}", new Money(Currency.GBP, 10.50M + item), "Grocery", "Lidl", DateTimeOffset.UtcNow);
            });

            s.DeleteManyExpensesAsync(null).GetAwaiter().GetResult();
            s.CreateManyExpensesAsync(seedExpenses).GetAwaiter().GetResult();

            return s;
        }
    }
}

