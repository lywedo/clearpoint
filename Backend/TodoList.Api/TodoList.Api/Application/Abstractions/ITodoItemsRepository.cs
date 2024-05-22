using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Domain.Models;

namespace TodoList.Api.Application.Abstractions
{
    public interface ITodoItemsRepository : IRepository<TodoItem>
    {
        Task<IEnumerable<TodoItem>> GetTodoItems(CancellationToken cancellationToken = default);
        Task CreateTodoItem(TodoItem todoItem, CancellationToken cancellationToken = default);
        Task<TodoItem> GetTodoItem(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsIdExits(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsDescriptionExists(string description, CancellationToken cancellationToken = default);
        Task<int> SetTodoItem(Guid id, TodoItem item, CancellationToken cancellationToken = default);
    }
}
