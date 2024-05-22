using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Api.Application.Abstractions;
using TodoList.Api.Domain.Models;
using TodoList.Api.Infrastructure.Context;

namespace TodoList.Api.Infrastructure.Repositories
{
    public class TodoItemsRepository : RepositoryBase<TodoItem>, ITodoItemsRepository
    {
        private readonly ILogger<TodoItemsRepository> _logger;
        private readonly TodoContext _todoContext;

        public TodoItemsRepository(ILoggerFactory loggerFactory, TodoContext context) : base(context)
        {
            _todoContext = context ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory.CreateLogger<TodoItemsRepository>();
        }

        public async Task CreateTodoItem(TodoItem todoItem, CancellationToken cancellationToken = default)
        {
            await Create(todoItem, cancellationToken);
            await _todoContext.SaveChangesAsync();
        }

        public async Task<TodoItem> GetTodoItem(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetById(id, cancellationToken);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems(CancellationToken cancellationToken = default)
        {
            return await _todoContext.TodoItems.Where(x => !x.IsCompleted).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsDescriptionExists(string description, CancellationToken cancellationToken = default)
        {
            return await _todoContext.TodoItems.AnyAsync(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted, cancellationToken);
        }

        public async Task<bool> IsIdExits(Guid id, CancellationToken cancellationToken = default)
        {
            return await _todoContext.TodoItems.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<int> SetTodoItem(Guid id, TodoItem item, CancellationToken cancellationToken = default)
        {
            var todoItem = await GetById(id, cancellationToken);
            if (todoItem != null)
            {
                todoItem.IsCompleted = item.IsCompleted;
                todoItem.Description = item.Description;
                return await _todoContext.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
    }
}
