﻿using System;

using MongoDB.Driver;

using SnarBanking.Expenses;

namespace SnarBanking
{
    public static class Specifications
    {
        public interface ISpecification<T>
        {
            FilterDefinition<T> IsSatisfiedBy();
        }

        public abstract class Specification<T> : ISpecification<T>
        {
            public virtual FilterDefinition<T> IsSatisfiedBy()
            {
                return FilterDefinition<T>.Empty;
            }
        }

        public class MatchAll : Specification<Expense> { }

        public class MatchAnyStore : Specification<Expense>
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

        public class MatchById : Specification<Expense>
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
