using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SnarBanking.Expenses.GettingExpenses
{
    internal static class GetExpensesEndpoint
    {
        internal static IEndpointRouteBuilder UseGetExpensesEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapGet(
                    "/api/expenses",
                    async (
                        IMediator mediator,
                        CancellationToken ct
                    ) =>
                    {
                        var results = await mediator.Send(new GetExpenses.Query(), ct);
                        return Results.Ok(results);
                    })
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}

