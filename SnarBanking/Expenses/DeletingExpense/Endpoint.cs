using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder UseDeletingExpenseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete("/api/expenses/{id}", async (IMediator mediator, string id, CancellationToken ct) =>
                {
                    await mediator.Send(new DeleteExpense.Command(id), ct);
                    return TypedResults.NoContent();
                });

        return endpoints;
    }
}