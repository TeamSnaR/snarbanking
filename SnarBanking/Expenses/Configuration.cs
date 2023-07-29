using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using SnarBanking.Expenses.GettingExpenseDetails;
using SnarBanking.Expenses.GettingExpenses;
using SnarBanking.Expenses.AddingExpense;

using static SnarBanking.Storage.Service;
using static SnarBanking.Storage.Specifications;
using SnarBanking.Core;

namespace SnarBanking.Expenses
{
    internal static class Configuration
    {
        public static IServiceCollection AddExpensesServices(this IServiceCollection services)
        {
            services
                .AddTransient<IGenericService<Expense>, ExpenseService>()
                .AddTransient<IGenericWriteService<Expense>, ExpenseService>();

            return services;
        }
        public static IEndpointRouteBuilder UseExpensesEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                .UseAddingExpenseEndpoints()
                .UseGettingExpenseDetailsEndpoint()
                .UseGetExpensesEndpoint(); // points to endpoint folder

            return endpoints;
        }
    }
}

