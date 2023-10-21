using Microsoft.AspNetCore.Mvc;

using Shouldly;

using SnarBanking.Expenses;
using SnarBanking.Expenses.AddingExpense;

namespace SnarBanking.Api.IntegrationTests;

public static class DeletingExpenseTests
{
    [Collection("Web application collection")]
    public class DeletingExpenseShould
    {
        private readonly WebApplicationFixture _waf;

        public DeletingExpenseShould(WebApplicationFixture waf)
        {
            _waf = waf;
        }

        [Fact]
        public async Task Not_Throw_Exception_When_Deleting_Existing_Expense()
        {
            var addExpenseRequest = new AddExpense.Command(new ExpenseDto("Expense description 1", new Money(Currency.GBP, 11.50M), "Store 1", "Category 1", DateTimeOffset.Now));

            var expenseId = await _waf.SendAsync(addExpenseRequest);

            var deleteExpenseRequest = new DeleteExpense.Command(expenseId);

            await _waf.SendASync(deleteExpenseRequest);

            await Should.NotThrowAsync(() => _waf.SendASync(deleteExpenseRequest));
        }

        [Fact]
        public async Task Throw_Exception_When_Deleting_Non_Existing_Expense()
        {
            var deleteExpenseRequest = new DeleteExpense.Command("Non existing expense id");

            await Should.ThrowAsync<Exception>(() => _waf.SendASync(deleteExpenseRequest));
        }
    }
}
