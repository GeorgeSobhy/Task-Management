

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericeRepository<T>
        where T : class, IDeletedEntity, IAuditEntity
    {
        protected ApplicationDbContext context { get; }
        protected DbSet<T> DbSet { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericRepository(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            _httpContextAccessor = httpContextAccessor;
            DbSet = context.Set<T>();
        }

        public async System.Threading.Tasks.Task AddAsync(T entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.CreationUser = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "N/A";

            await DbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await this.DbSet.AddRangeAsync(entities, cancellationToken);
            await context.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task UpdateAsync(T entity)
        {

            DbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            entity.ModificationDate = DateTime.UtcNow;
            entity.ModificationUser = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "N/A";
            await context.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task RemoveAsync(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.UtcNow;
            entity.DeletionUser = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "N/A";
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await this.DbSet.Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }
        public T? Get(Expression<Func<T, bool>> expression)
        {
            return this.DbSet.Where(expression).AsNoTracking().FirstOrDefault();
        }
        public IQueryable<T> GetMany(Expression<Func<T, bool>> expression)
        {
            return this.DbSet.Where(expression).AsQueryable();
        }
        public IQueryable<T> GetMany()
        {
            return this.DbSet.AsQueryable();
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = DbSet;

            if (includeExpressions != null)
            {
                foreach (var include in includeExpressions)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async System.Threading.Tasks.Task DeleteByIdAsync(int id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity != null)
            {
                await this.RemoveAsync(entity);
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = DbSet;
            if (includeExpressions != null)
            {
                foreach (var include in includeExpressions)
                {
                    query = query.Include(include);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public IQueryable<T> Include(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeExpressions)
        {
            DbSet<T> dbSet = DbSet;
            includeExpressions.ToList().ForEach(x => DbSet.Include(x).Load());
            return DbSet;
        }
        public async Task<IEnumerable<T>> ExcuteQuerys(string query, params object[] parameters)
        {
            return await DbSet.FromSqlRaw(query, parameters).ToListAsync();
        }

        public async Task<dynamic> ExcuteQuery(string query, params object[] parameters)
        {
            return await DbSet.FromSqlRaw(query, parameters).ToListAsync();
        }
    }
}
