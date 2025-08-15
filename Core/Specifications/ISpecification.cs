using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Filtering
        Expression<Func<T, bool>>? Criteria { get; }

        // Includes
        List<Expression<Func<T, object>>> Includes { get; }

        // Ordering
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }

        // Distinct
        bool IsDistinct { get; }

        // Paging
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }

        // Used by CountAsync to apply only the filter (no paging)
        IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }

    public interface IProjectionSpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? Selector { get; }
    }
}
