using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Api.Application.Abstractions;
using TodoList.Api.Application.Dtos;
using TodoList.Api.Domain.Models;
using TodoList.Api.Infrastructure.Context;
using TodoList.Api.Infrastructure.Repositories;

namespace TodoList.Api.Application.Services
{
    public class TodoItemsService : ITodoItemsService
    {
        private readonly ITodoItemsRepository _repository;
        private readonly ILogger<TodoItemsService> _logger;

        public TodoItemsService(ITodoItemsRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = loggerFactory.CreateLogger<TodoItemsService>();
        }

        public async Task<TodoItemDto> CreateTodoItem(TodoItemDto item, CancellationToken cancellationToken = default)
        {
           
            await _repository.CreateTodoItem(new TodoItem
            {
                Id = item.Id,
                Description = item.Description,
                IsCompleted = item.IsCompleted
            }, cancellationToken);
            return item;
        }

        public async Task<TodoItemDto> GetTodoItem(Guid id, CancellationToken cancellationToken = default)
        {
            var todoItem = await _repository.GetTodoItem(id);
            return new TodoItemDto { Description = todoItem.Description, IsCompleted = todoItem.IsCompleted, Id = todoItem.Id };
        }

        public async Task<IEnumerable<TodoItemDto>> GetTodoItems(CancellationToken cancellationToken = default)
        {
            var todoItems = await _repository.GetTodoItems(cancellationToken);
            return todoItems.Select(x => new TodoItemDto
            {
                Id = x.Id,
                Description = x.Description,
                IsCompleted = x.IsCompleted
            });
        }

        public async Task<bool> IsDescriptionExists(string description, CancellationToken cancellationToken = default)
        {
            return await _repository.IsDescriptionExists(description, cancellationToken);
        }

        public async Task<bool> IsIdExists(Guid id, CancellationToken cancellationToken = default)
        {
            return await _repository.IsIdExits(id, cancellationToken);
        }

        public async Task<int> SetTodoItem(Guid id, TodoItemDto item, CancellationToken cancellationToken = default)
        {
            return await _repository.SetTodoItem(id, new TodoItem { IsCompleted = item.IsCompleted, Description = item.Description}, cancellationToken);
        }
    }
}
