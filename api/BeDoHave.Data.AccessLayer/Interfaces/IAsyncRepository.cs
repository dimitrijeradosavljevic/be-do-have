using System;
using System.Collections.Generic;
using BeDoHave.Data.Core.Entities;
using System.Threading.Tasks;
using BeDoHave.Shared.Interfaces;

namespace BeDoHave.Data.AccessLayer.Interfaces
{
    public interface IAsyncRepository<T> : IAsyncDisposable
            where T : BaseEntity
    {
        Task<IList<T>> GetAsync();
        Task<T> GetByIdAsync(int id);
        Task<IList<T>> GetBySpecAsync(ISpecification<T> specification);
        Task<T> GetSingleBySpecAsync(ISpecification<T> specification);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

    }
}
