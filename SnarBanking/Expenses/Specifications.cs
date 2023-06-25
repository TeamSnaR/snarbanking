using MongoDB.Driver;

using static SnarBanking.Storage.Specifications;

namespace SnarBanking.Expenses;

internal static class Specifications
{
    public class MatchAllSpecification : IFilterDefinitionSpecification<Expense>
    {
        public FilterDefinition<Expense> IsSatisfiedBy() => IFilterDefinitionSpecification<Expense>.DefaultIsSatisfiedBy(this);
    }

    public class MatchAnyStoreSpecification : IFilterDefinitionSpecification<Expense>
    {
        private readonly IEnumerable<StringOrRegularExpression> _values;

        public MatchAnyStoreSpecification(params string[] values)
        {
            _values = values.Select(s => new StringOrRegularExpression(s));
        }

        public FilterDefinition<Expense> IsSatisfiedBy() =>
            Builders<Expense>
                .Filter
                    .StringIn(expense => expense.Store, _values);
    }

    public class MatchByIdSpecification : IFilterDefinitionSpecification<Expense>
    {
        private readonly string _id;

        public MatchByIdSpecification(string id)
        {
            _id = id;
        }

        public FilterDefinition<Expense> IsSatisfiedBy() =>
            Builders<Expense>.Filter
                .Eq(expense => expense.Id, _id);
    }
}
