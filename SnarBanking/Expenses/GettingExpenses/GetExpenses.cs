using System;

using MediatR;

namespace SnarBanking.Expenses.GettingExpenses
{
    internal static class GetExpenses
    {
        internal class Query : IRequest<IReadOnlyList<string>>
        { }

        internal class QueryHandler : IRequestHandler<Query, IReadOnlyList<string>>
        {
            public Task<IReadOnlyList<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new List<string> { "a" } as IReadOnlyList<string>);
            }
        }
    }
}

