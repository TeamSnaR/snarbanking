using SnarBanking.Expenses;
using SnarBanking.Expenses.AddingExpense;


using FluentValidation;

using Shouldly;
namespace SnarBanking.Api.IntegrationTests;

public static class AddingExpenseTests
{
    [Collection("Web application collection")]
    public class AddingExpenseShould
    {
        private readonly WebApplicationFixture _waf;

        public AddingExpenseShould(WebApplicationFixture waf)
        {
            _waf = waf;
        }

        [Fact]
        public async Task Return_Expense_Id_When_Expense_Is_Added()
        {
            var addExpenseRequest = new AddExpense.Command(new ExpenseDto("Expense description 1", new Money(Currency.GBP, 11.50M), "Store 1", "Category 1", DateTimeOffset.Now));

            var result = await _waf.SendAsync(addExpenseRequest);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<string>();
        }

        [Theory]
        [InlineData(null, 11.50, "Store 1", "Category 1")]
        [InlineData("Expense description 1", 0, "Store 1", "Category 1")]
        [InlineData("Expense description 1", 11.50, null, "Category 1")]
        [InlineData("Expense description 1", 11.50, "Store 1", null)]
        public async Task Throw_Validation_Exception_When_Expense_Dto_Is_Invalid(string description, decimal amount, string store, string category)
        {
            var addExpenseRequest = new AddExpense.Command(new ExpenseDto(description, new Money(Currency.GBP, amount), store, category, DateTimeOffset.Now));

            await Should.ThrowAsync<ValidationException>(() => _waf.SendAsync(addExpenseRequest));
        }
    }

}
