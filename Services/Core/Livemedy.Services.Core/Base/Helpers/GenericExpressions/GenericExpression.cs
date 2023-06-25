using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Livemedy.Core.Base.Helpers.GenericExpressions;

public class GenericExpression<T>
{
    public GenericExpression(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includePaths = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Expression<Func<T, T>> selector = null,
        bool disableTracking = true)
    {
        this.predicate = predicate;
        this.includePaths = includePaths;
        this.orderBy = orderBy;
        this.disableTracking = disableTracking;
        this.selector = selector;
    }
    //Expression<Func<T, int, TResult>>
    public Expression<Func<T, T>> selector { get; set; }
    public Expression<Func<T, bool>> predicate { get; set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>> orderBy { get; set; }
    public bool disableTracking { get; set; }
    public Func<IQueryable<T>, IIncludableQueryable<T, object>> includePaths { get; set; }
}
public class GenericGroupExpression<T, R>
{
    public GenericGroupExpression(
        Expression<Func<T, object>> groupBy,
        Expression<Func<IGrouping<object, T>, R>> selector,
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includePaths = null,
        Func<IQueryable<IGrouping<object, T>>, IOrderedQueryable<IGrouping<object, T>>> orderBy = null,
        int? topCount = null
        )
    {
        this.predicate = predicate;
        this.groupBy = groupBy;
        this.selector = selector;
        this.includePaths = includePaths;
        this.orderBy = orderBy;
        this.topCount = topCount;
    }
    public Expression<Func<T, bool>> predicate { get; set; }
    public Expression<Func<T, object>> groupBy { get; set; }
    public Expression<Func<IGrouping<object, T>, R>> selector { get; set; }
    public Func<IQueryable<IGrouping<object, T>>, IOrderedQueryable<IGrouping<object, T>>> orderBy { get; set; }
    public Func<IQueryable<T>, IIncludableQueryable<T, object>> includePaths { get; set; }
    public int? topCount { get; set; }
}
