using Livemedy.Core.Base.Entities;
using Livemedy.Core.Base.Helpers.GenericExpressions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Livemedy.Core.Base.Repositories;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    private readonly BaseContext Context;

    public Repository(BaseContext context)
    {
        Context = context;
    }

    public async Task<T> AddAsync(T entity)
    {
        using var transaction = Context.Database.BeginTransaction();

        try
        {
            Context.Set<T>().Add(entity);

            await Context.SaveChangesAsync();

            transaction.Commit();

            return entity;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return null;
        }
    }

    public async Task<int> AddAsync(ICollection<T> entity)
    {
        using var transaction = Context.Database.BeginTransaction();
        int count = 0;
        try
        {
            foreach (T item in entity)
            {
                Context.Set<T>().Add(item);
                count++;
            }

            await Context.SaveChangesAsync();

            transaction.Commit();

            return count;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return 0;
        }
    }


    public async Task<bool> DeleteAsync(T entity)
    {
        using var transaction = Context.Database.BeginTransaction();

        try
        {
            Context.Set<T>().Remove(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            transaction.Commit();
            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate = null,
        IList<string> includes = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
        int? TopCount = null,
        string QueryTag = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = Context.Set<T>();

        if (disableTracking) query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (TopCount != null) query = query.Take(TopCount.Value);

        if (QueryTag != null) query = query.TagWith(QueryTag);

        if (orderBy != null)
        {
            string sql1 = orderBy(query).ToQueryString();
            return await orderBy(query).ToListAsync();
        }
        string sql = query.ToQueryString();

        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(
        Expression<Func<T, bool>> predicate = null, 
        IList<string> includes = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string QueryTag = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = Context.Set<T>();

        if (disableTracking) query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (QueryTag != null) query = query.TagWith(QueryTag);

        if (orderBy != null)
            return await orderBy(query).FirstOrDefaultAsync();
        string sql = query.ToQueryString();
        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<R>> GetGroupedAsync<R>(GenericGroupExpression<T, R> parameters)
    {
        IQueryable<T> query = Context.Set<T>().Where(a => !a.IsDelete).AsNoTracking();

        if (parameters.includePaths != null)
            query = parameters.includePaths(query);

        if (parameters.predicate != null) query = query.Where(parameters.predicate);

        var grouped = query.GroupBy(parameters.groupBy);

        if (parameters.orderBy != null) grouped = parameters.orderBy(grouped);

        var result = grouped.Select(parameters.selector);

        if (parameters.topCount.HasValue)
            result = result.Take(parameters.topCount.Value);

        string sql = result.ToQueryString();

        return await result.ToListAsync();
    }



    public async Task UpdateAsync(T entity, Expression<Func<T, object>>[] includeProperties)
    {
        using var transaction = Context.Database.BeginTransaction();

        try
        {
            var dbEntry = Context.Entry(entity);
            foreach (var includeProperty in includeProperties)
            {
                dbEntry.Property(includeProperty).IsModified = true;
            }

            await Context.SaveChangesAsync();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
        }
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(GenericExpression<T> parameters = null)
    {
        IQueryable<T> query = Context.Set<T>().Where(a => !a.IsDelete);

        if (parameters == null) return query;

        if (parameters.disableTracking)
            query = query.AsNoTracking();

        if (parameters.includePaths != null)
            query = parameters.includePaths(query);

        if (parameters.predicate != null)
            query = query.Where(parameters.predicate);

        if (parameters.selector != null)
            query = query.Select(parameters.selector);

        if (parameters.orderBy != null)
            return await parameters.orderBy(query).ToListAsync();

        string sql = query.ToQueryString();

        return await query.ToListAsync();
    }
    public virtual async Task<int> GetCountAsync(GenericExpression<T> parameters = null)
    {
        IQueryable<T> query = Context.Set<T>().Where(a => !a.IsDelete);

        if (parameters == null)
            return 0;

        if (parameters.disableTracking)
            query = query.AsNoTracking();

        if (parameters.includePaths != null)
            query = parameters.includePaths(query);

        if (parameters.predicate != null)
            query = query.Where(parameters.predicate);

        string sql = query.ToQueryString();

        return await query.CountAsync();
    }

    public virtual async Task<T> GetAsync(GenericExpression<T> parameters = null)
    {
        IQueryable<T> query = Context.Set<T>().Where(a => !a.IsDelete);

        if (parameters == null)
            return await query.FirstOrDefaultAsync();

        if (parameters.disableTracking)
            query = query.AsNoTracking();

        if (parameters.includePaths != null)
            query = parameters.includePaths(query);

        if (parameters.predicate != null)
            query = query.Where(parameters.predicate);

        if (parameters.selector != null)
            query = query.Select(parameters.selector);
        
        if (parameters.orderBy != null)
            return await parameters.orderBy(query).FirstOrDefaultAsync();

        string sql = query.ToQueryString();
        return await query.FirstOrDefaultAsync();
    }

}
