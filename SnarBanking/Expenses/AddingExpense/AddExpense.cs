using MediatR;

using SnarBanking.Core;

namespace SnarBanking.Expenses.AddingExpense;

internal static class AddExpense
{
    internal record Command(ExpenseDto ExpenseDto) : IRequest<string>;

    internal class Handler : IRequestHandler<Command, string>
    {
        private readonly Core.IGenericWriteService<Expense> _genericWriteService;

        public Handler(IGenericWriteService<Expense> genericWriteService)
        {
            _genericWriteService = genericWriteService;
        }
        public async Task<string> Handle(
            Command request,
            CancellationToken ct
        )
        {
            var expense = new Expense(request.ExpenseDto.Description, request.ExpenseDto.Amount, request.ExpenseDto.Category, request.ExpenseDto.Store, request.ExpenseDto.PurchaseDate);

            var expenseId = await _genericWriteService.AddOneAsync(expense);
            return expenseId;
        }
    }
}
