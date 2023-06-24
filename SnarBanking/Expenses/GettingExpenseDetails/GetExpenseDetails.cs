using System;

using MediatR;

using static SnarBanking.Specifications;

namespace SnarBanking.Expenses.GettingExpenseDetails
{
    internal static class GetExpenseDetails
    {
        internal class ById : IRequest<Expense>
        {
            internal ById(string payload)
            {
                Payload = payload;
            }

            public string Payload { get; }
        }

        internal class ByIdHandler : IRequestHandler<ById, Expense>
        {
            private readonly Func<ISpecification<Expense>, Task<Expense>> _getExpenseDetails;

            public ByIdHandler(Func<ISpecification<Expense>, Task<Expense>> getExpenseDetails)
            {
                _getExpenseDetails = getExpenseDetails;
            }
            public Task<Expense> Handle(ById request, CancellationToken cancellationToken)
            {
                var expense = _getExpenseDetails(new MatchById(request.Payload)) ?? throw new NullReferenceException("Expense not found");
                return expense;
            }
        }
    }
}

