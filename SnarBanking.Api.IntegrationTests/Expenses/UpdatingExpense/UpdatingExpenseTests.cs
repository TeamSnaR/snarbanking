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
        public async Task Return_No_Content_When_Expense_Is_Update()
        {
            var expenseId = "64986a2e6512f7e2d0ccb621";
            var updateExpenseRequest = new UpdateExpense.Command(expenseId, new ExpenseDto("Updated Expense description 1", new Money(Currency.GBP, 11.50M), "Store 1", "Category 1", DateTimeOffset.Now));

            await Should.NotThrowAsync(() => _waf.SendASync(updateExpenseRequest));

            var getExpenseRequest = new GetExpenseDetails.ById(expenseId);
            var result = await _waf.SendAsync(getExpenseRequest);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<Expense>();
            result.Description.ShouldBe("Updated Expense description 1");
        }
    }
}
