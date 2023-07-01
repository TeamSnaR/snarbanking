using System;

using MediatR;

using MongoDB.Driver;

using static SnarBanking.Expenses.Specifications;
using static SnarBanking.Storage.Specifications;

namespace SnarBanking.Expenses.GettingExpenseDetails
{
    internal static class GetExpenseItems
    {

        internal record ExpenseItemProjection(string Id, string? Description, ICollection<ExpenseItem> Items);

        internal class GetExpenseItemsProjector : IProjector<Expense, ExpenseItemProjection>
        {
            public ProjectionDefinition<Expense, ExpenseItemProjection> ProjectAs() =>
                Builders<Expense>
                    .Projection
                        .Expression(expense => new ExpenseItemProjection(expense.Id, expense.Description, expense.Items));
        }
        internal class ById : IRequest<ExpenseItemProjection>
        {
            public ById(string payload)
            {
                Payload = payload;
            }

            public string Payload { get; }
        }

        internal class ByIdHandler : IRequestHandler<ById, ExpenseItemProjection>
        {
            private readonly IGenericService<Expense> _expenseService;

            public ByIdHandler(IGenericService<Expense> expenseService)
            {
                _expenseService = expenseService;
            }

            public async Task<ExpenseItemProjection> Handle(ById request, CancellationToken cancellationToken)
            {
                return await _expenseService.GetOneAsync(new MatchByIdSpecification(request.Payload), new GetExpenseItemsProjector()) ?? throw new NullReferenceException("Expense not found");
            }
        }
    }
}

