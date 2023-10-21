using FluentValidation;

using Shouldly;

using SnarBanking.Expenses;
using SnarBanking.Expenses.GettingExpenseDetails;
using SnarBanking.Expenses.UpdatingExpense;

namespace SnarBanking.Api.IntegrationTests;

public static class UpdatingExpenseTests
{
    [Collection("Web application collection")]
    public class UpdatingExpenseShould
    {
        private readonly WebApplicationFixture _waf;

        public UpdatingExpenseShould(WebApplicationFixture waf)
        {
            _waf = waf;
        }

        [Fact]
        public async Task Return_One_When_A_Single_Expense_Is_Updated()
        {
            var expenseId = "64986a2e6512f7e2d0ccb621";
            var updateExpenseRequest = new UpdateExpense.Command(expenseId, new ExpenseDto("Updated Expense description 1", new Money(Currency.GBP, 11.50M), "Store 1", "Category 1", DateTimeOffset.Now));

            var result = await _waf.SendAsync(updateExpenseRequest);
            result.ShouldBe(1);

            var getExpenseRequest = new GetExpenseDetails.ById(expenseId);
            var expense = await _waf.SendAsync(getExpenseRequest);

            expense.ShouldNotBeNull();
            expense.ShouldBeOfType<Expense>();
            expense.Description.ShouldBe("Updated Expense description 1");
        }

        [Theory]
        [InlineData(null, 11.50, "Store 1", "Category 1")]
        [InlineData("Expense description 1", 0, "Store 1", "Category 1")]
        [InlineData("Expense description 1", 11.50, null, "Category 1")]
        [InlineData("Expense description 1", 11.50, "Store 1", null)]
        public async Task Throw_Validation_Exception_When_Expense_Dto_Is_Invalid(string description, decimal amount, string store, string category)
        {
            var updateExpenseRequest = new UpdateExpense.Command("64986a2e6512f7e2d0ccb621", new ExpenseDto(description, new Money(Currency.GBP, amount), store, category, DateTimeOffset.Now));

            await Should.ThrowAsync<ValidationException>(() => _waf.SendAsync(updateExpenseRequest));
        }
    }
}
