
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Data.AccessLayer.Context;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.AccessLayer.Extensions;
using BeDoHave.Shared.Interfaces;

namespace BeDoHave.Data.AccessLayer.Repositories
{
    public class AsyncRepository<T> : IAsyncRepository<T>
            where T : BaseEntity
    {
        protected DocumentDbContext _dbContext;

        public AsyncRepository(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IList<T>> GetBySpecAsync(ISpecification<T> specification)
        {
            return await GetQueryable(specification).ToListAsync();
        }

        public Task<T> GetSingleBySpecAsync(ISpecification<T> specification)
        {
            return Task.FromResult(GetQueryable(specification).FirstOrDefault());
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
        }

        public async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                await _dbContext.DisposeAsync()
                    .ConfigureAwait(false);

                _dbContext = null;
            }
        }

        private IQueryable<T> GetQueryable(ISpecification<T> specification)
        {
            var queryable = specification.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            queryable = specification.IncludeStrings
                .Aggregate(queryable, (current, include) => current.Include(include));

            queryable = specification.OrderByStrings
                .Aggregate(queryable, (current, orderBy) => specification.Direction == "ASC"
                    ? current.OrderBy(orderBy)
                    : current.OrderByDescending(orderBy));

            queryable = queryable.Where(specification.Criteria);

            if (specification.Skip.HasValue)
            {
                queryable = queryable.Skip(specification.Skip.Value);
            }

            if (specification.Take.HasValue)
            {
                queryable = queryable.Take(specification.Take.Value);
            }

            return queryable;
        }
    }
}
