using System;
using System.Linq.Expressions;

using MongoDB.Driver;

using SnarBanking.Core;
using SnarBanking.Expenses;

using static SnarBanking.Storage.Specifications;

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

        public interface IFilterDefinitionSpecification<T> : ISpecification<FilterDefinition<T>>
        {
            public new FilterDefinition<T> IsSatisfiedBy() => DefaultIsSatisfiedBy(this);
            protected static FilterDefinition<T> DefaultIsSatisfiedBy(ISpecification<FilterDefinition<T>> _)
            {
                return FilterDefinition<T>.Empty;
            }
        }

        public class MatchAllSpecification : IFilterDefinitionSpecification<Expense>
        {
            public FilterDefinition<Expense> IsSatisfiedBy() => IFilterDefinitionSpecification<Expense>.DefaultIsSatisfiedBy(this);
        }

        public class MatchAnyStoreSpecification : FilterDefinitionSpecification<Expense>
        {
            private readonly IEnumerable<StringOrRegularExpression> _values;

            public MatchAnyStoreSpecification(params string[] values)
            {
                _values = values.Select(s => new StringOrRegularExpression(s));
            }

            public override FilterDefinition<Expense> IsSatisfiedBy() =>
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

        public interface IProjector<TSource, IProjectionResult>
        {
            ProjectionDefinition<TSource, IProjectionResult> ProjectAs();
        }

    }
}

