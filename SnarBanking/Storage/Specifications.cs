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
        public interface IFilterDefinitionSpecification<T> : ISpecification<FilterDefinition<T>>
        {
            public new FilterDefinition<T> IsSatisfiedBy() => DefaultIsSatisfiedBy(this);
            protected static FilterDefinition<T> DefaultIsSatisfiedBy(ISpecification<FilterDefinition<T>> _)
            {
                return FilterDefinition<T>.Empty;
            }
        }

        public interface IProjector<TSource, IProjectionResult>
        {
            ProjectionDefinition<TSource, IProjectionResult> ProjectAs();
        }
    }
}

