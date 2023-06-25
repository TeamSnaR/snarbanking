using System;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using static SnarBanking.Expenses.GettingExpenseDetails.GetExpenseItems;

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

            endpoints
                .MapGet(
                    "/api/expenses/{id}/items",
                    async (
                        IMediator mediator,
                        string id,
                        CancellationToken ct
                        ) =>
                    {
                        var expenseItems = await mediator.Send(new GetExpenseItems.ById(id), ct);
                        return Results.Ok(expenseItems);
                    }
                )
                .Produces<IReadOnlyList<ExpenseItemProjection>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}

