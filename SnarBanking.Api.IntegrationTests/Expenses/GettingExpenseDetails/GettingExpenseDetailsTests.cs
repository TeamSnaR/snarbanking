using System;

using MongoDB.Bson;

using Shouldly;

using SnarBanking.Expenses.GettingExpenseDetails;

namespace SnarBanking.Api.IntegrationTests.Expenses.GettingExpenseDetails
{
    public static class GettingExpenseDetailsTests
    {
        public class GetExpenseDetailsShould : IClassFixture<WebApplicationFixture>
        {
            private readonly WebApplicationFixture _waf;

            public GetExpenseDetailsShould(WebApplicationFixture waf)
            {
                _waf = waf;
            }

            [Fact]
            public async Task Return_Expense_Details_When_Expense_Exists()
            {
                var getExpenseDetailsRequest = new GetExpenseDetails.ById("64986a2e6512f7e2d0ccb621");

                var result = await _waf.SendAsync(getExpenseDetailsRequest);

                result.ShouldNotBeNull();
                result.Description.ShouldBe("Expense description 1");
                result.Amount.Value.ShouldBe(11.50M);
            }

            [Fact]
            public async Task Throw_Null_Reference_Exception_When_Expense_Does_Not_Exist()
            {

                var getExpenseDetailsRequest = new GetExpenseDetails.ById(ObjectId.GenerateNewId().ToString());

                await Should.ThrowAsync<NullReferenceException>(async () =>
                {
                    await _waf.SendAsync(getExpenseDetailsRequest);
                });
            }
        }

        public class GetExpenseItemsShould : IClassFixture<WebApplicationFixture>
        {
            private readonly WebApplicationFixture _waf;

            public GetExpenseItemsShould(WebApplicationFixture waf)
            {
                _waf = waf;
            }

            [Fact]
            public async Task Return_Expense_Items_When_Expense_Exists_And_It_Has_Items()
            {
                var getExpenseItemsRequest = new GetExpenseItems.ById("64986a2e6512f7e2d0ccb626");

                var result = await _waf.SendAsync(getExpenseItemsRequest);

                result.ShouldNotBeNull();
                result.Description.ShouldBe("Expense description 6");
                result.Items.ShouldNotBeEmpty();
            }
            [Fact]
            public async Task Throw_Exception_When_Expense_Does_Not_Exist()
            {
                var getExpenseItemsRequest = new GetExpenseItems.ById(ObjectId.GenerateNewId().ToString());

                var exception = await Should.ThrowAsync<NullReferenceException>(async () => await _waf.SendAsync(getExpenseItemsRequest));
                exception.Message.ShouldBe("Expense not found");
            }
        }
    }
}

