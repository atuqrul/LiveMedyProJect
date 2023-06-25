using Livemedy.Core.Base.Entities;
using Livemedy.Core.Base.Helpers.GenericExpressions;
using System.Linq.Expressions;

namespace Livemedy.Core.Base.Repositories;

public interface IRepository<T> where T : EntityBase
{
    Task<T> AddAsync(T entity);
    Task<int> AddAsync(ICollection<T> entity);
    Task UpdateAsync(T entity, Expression<Func<T, object>>[] includeProperties);
    Task<bool> DeleteAsync(T entity);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
                                  IList<string> includes = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  int? TopCount = null,
                                  string QueryTag = null,
                                  bool disableTracking = true);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate = null,
                                  IList<string> includes = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  string QueryTag = null,
                                  bool disableTracking = true);

   

    Task<T> GetAsync(GenericExpression<T> parameters = null);

    Task<IEnumerable<T>> GetAllAsync(GenericExpression<T> parameters = null);
    Task<int> GetCountAsync(GenericExpression<T> parameters = null);
    Task<IEnumerable<R>> GetGroupedAsync<R>(GenericGroupExpression<T, R> parameters);

}
