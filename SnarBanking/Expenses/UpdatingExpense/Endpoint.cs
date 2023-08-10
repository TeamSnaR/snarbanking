using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using SnarBanking.Expenses;

namespace SnarBanking;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder UseUpdatingExpenseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPut("/api/expenses/{expenseId}", async (IMediator mediator, string expenseId, CancellationToken ct) =>
                {
                    // await mediator.Send(new UpdateExpense.Command(expenseId, expenseToUpdate), ct);
                    return TypedResults.NoContent();
                });
        return endpoints;
    }
}
