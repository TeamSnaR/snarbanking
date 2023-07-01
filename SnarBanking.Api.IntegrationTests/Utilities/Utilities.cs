using System;
using System.Reflection;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using SnarBanking.Expenses;

using static SnarBanking.Storage.Service;

namespace SnarBanking.Api.IntegrationTests.Utilities
{
    public static class Utilities
    {
        public static void InitializeDbForTests(SnarBankingMongoDbService snarBankingMongoDbService)
        {
            snarBankingMongoDbService.ExpensesCollection.InsertManyAsync(SeedExpenses()).GetAwaiter().GetResult();
        }

        public static void ReInitializeDbForTests(SnarBankingMongoDbService snarBankingMongoDbService)
        {
            snarBankingMongoDbService.ExpensesCollection.DeleteManyAsync(_ => true).GetAwaiter().GetResult();

            InitializeDbForTests(snarBankingMongoDbService);
        }

        static IEnumerable<Expense> SeedExpenses()
        {
            //static bool RandomNumberIsEven(int num) => num % 2 == 0;

            //IEnumerable<Expense> seedExpenses = Enumerable.Range(1, 5).Select(randomNumber =>
            //{
            //    var expense = new Expense($"Expense description {randomNumber}", new Money(Currency.GBP, 10.50M + randomNumber), "Grocery", "Lidl", DateTimeOffset.UtcNow);

            //    if (RandomNumberIsEven(randomNumber))
            //    {
            //        expense.AddExpenseItem(new ExpenseItem($"Expense item {randomNumber}", new Money(Currency.GBP, 7.50M + randomNumber), randomNumber, "Food", UnitOfMeasure.Piece));
            //    }

            //    return expense;


            //});
            var pathToSeedTestData = Path.Combine(Assembly.GetExecutingAssembly().Location, "Data", "TestData.json");
            using var stream = new StreamReader(pathToSeedTestData);
            var seedTestData = stream.ReadToEnd();
            var seedTestDataAsBson = BsonSerializer.Deserialize<IEnumerable<Expense>>(seedTestData);

            return seedTestDataAsBson;
        }
    }
}

