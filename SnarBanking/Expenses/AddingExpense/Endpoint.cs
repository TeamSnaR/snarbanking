using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace SnarBanking.Expenses.AddingExpense;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder UseAddingExpenseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost("/api/expenses", async (IMediator mediator, ExpenseDto expenseDto, CancellationToken ct) =>
                {
                    var expenseId = await mediator.Send(new AddExpense.Command(expenseDto), ct);
                    return TypedResults.Created($"/api/expenses/{expenseId}", expenseId);
                });

        return endpoints;
    }
}