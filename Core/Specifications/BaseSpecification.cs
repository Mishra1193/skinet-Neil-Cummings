using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    // MUST be generic
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification() { }

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
            => Criteria = criteria;

        // Filtering
        public Expression<Func<T, bool>>? Criteria { get; private set; }
        protected void AddCriteria(Expression<Func<T, bool>> criteria) => Criteria = criteria;

        // Includes
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        protected void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);

        // Ordering
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        /// <summary>Neil-style helper used in the course code.</summary>
        protected void AddOrderBy(Expression<Func<T, object>> expr) => OrderBy = expr;

        /// <summary>Neil-style helper used in the course code.</summary>
        protected void AddOrderByDescending(Expression<Func<T, object>> expr) => OrderByDescending = expr;

        /// <summary>Your existing names kept for backward compatibility.</summary>
        protected void ApplyOrderBy(Expression<Func<T, object>> expr) => OrderBy = expr;

        /// <summary>Your existing names kept for backward compatibility.</summary>
        protected void ApplyOrderByDescending(Expression<Func<T, object>> expr) => OrderByDescending = expr;

        // Distinct
        public bool IsDistinct { get; private set; }
        protected void ApplyDistinct() => IsDistinct = true;

        // Paging (Step-1 fix: NON-nullable ints to match ISpecification)
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }
            return query;
        }
    }

    // Projection base (used by ProductBrands/Types specs, etc.)
    public abstract class BaseProjectionSpecification<T, TResult>
        : BaseSpecification<T>, IProjectionSpecification<T, TResult>
    {
        protected BaseProjectionSpecification() { }

        protected BaseProjectionSpecification(Expression<Func<T, bool>> criteria)
            : base(criteria) { }

        public Expression<Func<T, TResult>>? Selector { get; private set; }

        protected void ApplySelect(Expression<Func<T, TResult>> selector) => Selector = selector;
    }
}
