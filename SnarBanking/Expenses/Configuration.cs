using System;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using SnarBanking.Expenses.GettingExpenses;

namespace SnarBanking.Expenses
{
    internal static class Configuration
    {
        public static IServiceCollection AddExpensesServices(this IServiceCollection services)
        {
            return services;
        }
        public static IEndpointRouteBuilder UseExpensesEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                //.UseGetExpenseDetailsEndpoint()
                //.UseAddExpenseEndpoint()
                .UseGetExpensesEndpoint(); // points to endpoint folder

            return endpoints;
        }
    }
}

