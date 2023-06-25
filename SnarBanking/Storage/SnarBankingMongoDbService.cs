using System.Collections.Generic;

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
            public SnarBankingMongoDbService(IMongoDatabase mongoDatabase, SnarBankingDbSettings snarBankingDbSettings)
            {
                ExpensesCollection = mongoDatabase.GetCollection<Expense>(snarBankingDbSettings.DefaultCollectionName);
            }

            public IMongoCollection<Expense> ExpensesCollection { get; init; }
        }

        public static IServiceCollection AddSnarBankingMongoDbService(this IServiceCollection services) =>
            services
                .AddSingleton(sp =>
                {
                    var settings = sp.GetRequiredService<SnarBankingDbSettings>();
                    var client = new MongoClient(MongoClientSettings.FromConnectionString(settings.ConnectionString));

                    return client.GetDatabase(settings.DatabaseName);
                })
                .AddSingleton<SnarBankingMongoDbService>();

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

            s.ExpensesCollection.DeleteManyAsync(_ => true).GetAwaiter().GetResult();
            s.ExpensesCollection.InsertManyAsync(seedExpenses).GetAwaiter().GetResult();

            return s;
        }
    }
}

