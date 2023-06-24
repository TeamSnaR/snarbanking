using System;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace SnarBanking.Expenses.GettingExpenseDetails
{
    internal static class GettingExpenseDetailsEndpoint
    {
        internal static IEndpointRouteBuilder UseGettingExpenseDetailsEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapGet(
                    "/api/expenses/{id}",
                    async (
                        IMediator mediator,
                        string id,
                        CancellationToken ct) =>
                    {
                        var expense = await mediator.Send(new GetExpenseDetails.ById(id), ct);
                        return Results.Ok(expense);
                    }
                )
                .Produces<Expense>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);
            return endpoints;
        }
    }
}

