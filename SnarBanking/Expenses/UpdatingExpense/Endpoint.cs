using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using SnarBanking.Expenses;
using SnarBanking.Expenses.UpdatingExpense;

namespace SnarBanking;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder UseUpdatingExpenseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/expenses/{expenseId}", async (string expenseId, ExpenseDto expenseToUpdate, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new UpdateExpense.Command(expenseId, expenseToUpdate), ct);
            return TypedResults.NoContent();
        });

        return endpoints;
    }

}
