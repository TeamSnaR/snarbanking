using System;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using SnarBanking.Expenses.GettingExpenseDetails;
using SnarBanking.Expenses.GettingExpenses;

using static SnarBanking.Specifications;
using static SnarBanking.Storage.Service;

namespace SnarBanking.Expenses
{
    internal static class Configuration
    {
        public static IServiceCollection AddExpensesServices(this IServiceCollection services)
        {
            services
                .AddTransient<Func<FilterDefinition<Expense>, Task<List<Expense>>>>(sp =>
                    sp.GetRequiredService<SnarBankingMongoDbService>().GetExpensesAsync
                )
                .AddTransient<Func<ISpecification<Expense>, Task<Expense>>>(sp =>
                    sp.GetRequiredService<SnarBankingMongoDbService>().GetOneExpenseAsync
                );


            return services;
        }
        public static IEndpointRouteBuilder UseExpensesEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                //.UseAddExpenseEndpoint()
                .UseGettingExpenseDetailsEndpoint()
                .UseGetExpensesEndpoint(); // points to endpoint folder

            return endpoints;
        }
    }
}

