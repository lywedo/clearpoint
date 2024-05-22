using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TodoList.Api.Application.Abstractions
{
    public interface IRepository<T>
    {
        Task<T> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        Task Create(T entity, CancellationToken cancellationToken = default);
        void Update(T entity, CancellationToken cancellationToken = default);
        void Delete(T entity, CancellationToken cancellationToken = default);
    }
}