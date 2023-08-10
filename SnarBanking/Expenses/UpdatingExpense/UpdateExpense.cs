using MediatR;

using SnarBanking.Expenses;
using SnarBanking.Core;
using FluentValidation;

namespace SnarBanking;

internal static class UpdateExpense
{
    internal record Command(string ExpenseId, ExpenseDto ExpenseToUpdate) : IRequest;

    internal class Handler : IRequestHandler<Command>
    {
        private readonly IGenericWriteService<Expense> _expenseWriteService;
        private readonly IValidator<ExpenseDto> _validator;

        public Handler(IGenericWriteService<Expense> expenseWriteService, IValidator<ExpenseDto> validator)
        {
            _expenseWriteService = expenseWriteService;
            _validator = validator;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.ExpenseToUpdate, cancellationToken);

            await _expenseWriteService.UpdateOneAsync(request.ExpenseId, new Expense(request.ExpenseToUpdate.Description, request.ExpenseToUpdate.Amount, request.ExpenseToUpdate.Category, request.ExpenseToUpdate.Store, request.ExpenseToUpdate.PurchaseDate));
        }
    }
}
