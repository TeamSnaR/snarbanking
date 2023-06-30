using System;

using MediatR;

using MongoDB.Driver;

using static SnarBanking.Expenses.Specifications;
using static SnarBanking.Storage.Specifications;

namespace SnarBanking.Expenses.GettingExpenses
{
    internal static class GetExpenses
    {
        internal class Query : IRequest<IReadOnlyList<Expense>>
        { }

        internal class QueryHandler : IRequestHandler<Query, IReadOnlyList<Expense>>
        {
            private readonly IGenericService<Expense> _expenseService;
            public QueryHandler(IGenericService<Expense> expenseService)
            {
                _expenseService = expenseService;
            }
            public async Task<IReadOnlyList<Expense>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _expenseService.GetManyAsync(new MatchAllSpecification());
            }
        }
    }
}

