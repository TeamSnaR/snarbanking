using System;

using MongoDB.Driver;

using SnarBanking.Core;
using SnarBanking.Expenses;

namespace SnarBanking.Storage
{
    public static class Specifications
    {
        public abstract class FilterDefinitionSpecification<T> : ISpecification<FilterDefinition<T>>
        {
            public virtual FilterDefinition<T> IsSatisfiedBy()
            {
                return FilterDefinition<T>.Empty;
            }
        }

        public class MatchAll : FilterDefinitionSpecification<Expense> { }

        public class MatchAnyStore : FilterDefinitionSpecification<Expense>
        {
            private readonly IEnumerable<StringOrRegularExpression> _values;

            public MatchAnyStore(params string[] values)
            {
                _values = values.Select(s => new StringOrRegularExpression(s));
            }

            public override FilterDefinition<Expense> IsSatisfiedBy() =>
                Builders<Expense>
                    .Filter
                        .StringIn(expense => expense.Store, _values);
        }

        public class MatchById : FilterDefinitionSpecification<Expense>
        {
            private readonly string _id;

            public MatchById(string id)
            {
                _id = id;
            }

            public override FilterDefinition<Expense> IsSatisfiedBy() =>
                Builders<Expense>.Filter
                    .Eq(expense => expense.Id, _id);
        }
    }
}

