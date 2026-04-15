using System.Linq.Expressions;
using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Domain.Interfaces
{


    public interface IGenericeRepository<T>
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        T? Get(Expression<Func<T, bool>> expression);
        IQueryable<T> GetMany(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions);
        IQueryable<T> Include(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeExpressions);
        Task<IEnumerable<T>> ExcuteQuerys(string query, params object[] parameters);
        public Task<dynamic> ExcuteQuery(string query, params object[] parameters);
        IQueryable<T> GetMany();
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeExpressions);
        Task DeleteByIdAsync(int id);
    }
}
