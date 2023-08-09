using System;

using FluentValidation;

namespace SnarBanking.Expenses.AddingExpense
{
    public class ExpenseValidator : AbstractValidator<ExpenseDto>
    {
        public ExpenseValidator()
        {
            RuleFor(m => m.Description)
                .NotEmpty();
            RuleFor(m => m.Amount)
                .NotNull()
                .Must(BeAValidAmount);
            RuleFor(m => m.Store)
                .NotEmpty();
            RuleFor(m => m.Category)
                .NotEmpty();
            RuleFor(m => m.PurchaseDate)
                .Must(BeAValidDate);

        }

        private bool BeAValidDate(DateTimeOffset offset)
        {
            return !offset.Equals(DateTimeOffset.MinValue);
        }

        private bool BeAValidAmount(Money money)
        {
            return money.Value > 0;
        }
    }
}

