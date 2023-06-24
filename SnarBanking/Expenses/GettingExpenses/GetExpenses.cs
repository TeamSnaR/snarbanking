using System;

using MediatR;

using MongoDB.Driver;

namespace SnarBanking.Expenses.GettingExpenses
{
    internal static class GetExpenses
    {
        internal class Query : IRequest<IReadOnlyList<Expense>>
        { }

        internal class QueryHandler : IRequestHandler<Query, IReadOnlyList<Expense>>
        {
            private readonly Func<FilterDefinition<Expense>?, Task<List<Expense>>> _getExpenses;

            public QueryHandler(Func<FilterDefinition<Expense>?, Task<List<Expense>>> GetExpenses)
            {
                _getExpenses = GetExpenses;
            }
            public async Task<IReadOnlyList<Expense>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _getExpenses(null);
            }
        }
    }
}

