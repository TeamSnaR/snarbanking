using SnarBanking.Api.IntegrationTests;
using Shouldly;
using SnarBanking.Expenses.GettingExpenses;

namespace SnarBanking.Api.Tests.Expenses.GettingExpenses
{
    public static class GettingExpensesTests
    {
        [Collection("Web application collection")]
        public class GetExpensesShould
        {
            private readonly WebApplicationFixture _waf;

            public GetExpensesShould(WebApplicationFixture waf)
            {
                _waf = waf;
            }

            [Fact]
            public async Task ReturnListOfExpenses()
            {
                var request = new GetExpenses.Query();

                var result = await _waf.SendAsync(request);

                result.Count.ShouldNotBe(0);
                result.Count.ShouldBeGreaterThanOrEqualTo(10);
            }
        }
    }
}

