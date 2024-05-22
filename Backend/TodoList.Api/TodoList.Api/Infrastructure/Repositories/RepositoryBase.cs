using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoList.Api.Application.Abstractions;
using TodoList.Api.Infrastructure.Context;

namespace TodoList.Api.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
	{
		private readonly TodoContext _context;

		public RepositoryBase(TodoContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

        public virtual async Task<T> GetById(Guid id, CancellationToken cancellationToken = default) => await _context
            .Set<T>()
            .FindAsync(new object[] { id }, cancellationToken);

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default) => await _context
            .Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) => await _context
            .Set<T>()
            .Where(expression)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        public async Task Create(T entity, CancellationToken cancellationToken = default) => await _context.AddAsync(entity, cancellationToken);

        public void Update(T entity, CancellationToken cancellationToken = default) => _context.Update(entity);

        public void Delete(T entity, CancellationToken cancellationToken = default) => _context.Remove(entity);
    }
}