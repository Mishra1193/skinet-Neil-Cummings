using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Core.Specifications;

namespace Infrastructure.Data
{
    public static class SpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// Non-projection path (returns IQueryable<T>).
        /// Order: Where -> OrderBy/OrderByDescending -> Distinct -> Includes -> Paging.
        /// </summary>
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            // Where / filter
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            // Ordering
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            // Distinct (entity path, if requested)
            if (spec.IsDistinct)
                query = query.Distinct();

            // Includes (EF Core navigation properties)
            foreach (var include in spec.Includes)
                query = query.Include(include);

            // Paging (entity path)
            if (spec.IsPagingEnabled)
            {
                if (spec.Skip > 0) query = query.Skip(spec.Skip);
                if (spec.Take > 0) query = query.Take(spec.Take);
            }

            return query;
        }

        /// <summary>
        /// Projection path (returns IQueryable<TResult>).
        /// Important: Apply paging AFTER projecting (Neil's approach).
        /// Order: Where -> OrderBy/OrderByDescending -> Includes -> Select -> Distinct -> Paging.
        /// </summary>
        public static IQueryable<TResult> GetQuery<TResult>(
            IQueryable<T> inputQuery,
            IProjectionSpecification<T, TResult> spec)
        {
            var query = inputQuery;

            // Where / filter
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            // Ordering
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            // Includes (before projection so nav props can be used)
            foreach (var include in spec.Includes)
                query = query.Include(include);

            // Selector is required for projection
            if (spec.Selector is null)
                throw new InvalidOperationException("Projection specification requires a non-null Selector.");

            // Project
            var selectQuery = query.Select(spec.Selector);

            // Distinct AFTER projection (especially for value projections like strings)
            if (spec.IsDistinct)
                selectQuery = selectQuery.Distinct();

            // Paging (projection path)
            if (spec.IsPagingEnabled)
            {
                if (spec.Skip > 0) selectQuery = selectQuery.Skip(spec.Skip);
                if (spec.Take > 0) selectQuery = selectQuery.Take(spec.Take);
            }

            return selectQuery;
        }
    }
}
