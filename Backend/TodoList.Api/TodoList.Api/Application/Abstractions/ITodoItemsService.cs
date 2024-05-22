using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Application.Dtos;

namespace TodoList.Api.Application.Abstractions
{
    public interface ITodoItemsService
    {
        Task<IEnumerable<TodoItemDto>> GetTodoItems(CancellationToken cancellationToken = default);
        Task<TodoItemDto> GetTodoItem(Guid id, CancellationToken cancellationToken = default);
        Task<int> SetTodoItem(Guid id, TodoItemDto item, CancellationToken cancellationToken = default);
        Task<TodoItemDto> CreateTodoItem(TodoItemDto item, CancellationToken cancellationToken = default);
        Task<bool> IsIdExists(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsDescriptionExists(string description, CancellationToken cancellationToken = default);
    }
}
