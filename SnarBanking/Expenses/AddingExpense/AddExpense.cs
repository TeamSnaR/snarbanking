using FluentValidation;

using MediatR;

using SnarBanking.Core;

namespace SnarBanking.Expenses.AddingExpense;

internal static class AddExpense
{
    internal record Command(ExpenseDto ExpenseDto) : IRequest<string>;

    internal class Handler : IRequestHandler<Command, string>
    {
        private readonly Core.IGenericWriteService<Expense> _genericWriteService;
        private readonly IValidator<ExpenseDto> _validator;

        public Handler(IGenericWriteService<Expense> genericWriteService, IValidator<ExpenseDto> validator)
        {
            _genericWriteService = genericWriteService;
            _validator = validator;
        }
        public async Task<string> Handle(
            Command request,
            CancellationToken ct
        )
        {
            var expense = new Expense(request.ExpenseDto.Description, request.ExpenseDto.Amount, request.ExpenseDto.Category, request.ExpenseDto.Store, request.ExpenseDto.PurchaseDate);

            await _validator.ValidateAndThrowAsync(request.ExpenseDto, ct);

            var expenseId = await _genericWriteService.AddOneAsync(expense);
            return expenseId;
        }
    }
}
