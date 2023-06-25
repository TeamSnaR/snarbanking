using MediatR;

using static SnarBanking.Expenses.Specifications;
using static SnarBanking.Storage.Specifications;

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
            private readonly IGenericService<Expense> _expenseService;

            public ByIdHandler(IGenericService<Expense> expenseService)
            {
                _expenseService = expenseService;
            }
            public Task<Expense> Handle(ById request, CancellationToken cancellationToken)
            {
                return _expenseService.GetOneAsync(new MatchByIdSpecification(request.Payload)) ?? throw new NullReferenceException("Expense not found");
            }
        }
    }
}

