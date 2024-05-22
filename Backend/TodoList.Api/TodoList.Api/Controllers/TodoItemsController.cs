using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Application.Abstractions;
using TodoList.Api.Application.Dtos;
using TodoList.Api.Application.Services;
using TodoList.Api.Domain.Models;
using TodoList.Api.Infrastructure.Context;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoItemsService _todoItemsService;

        public TodoItemsController(TodoContext context,ITodoItemsService todoItemsService, ILoggerFactory loggerFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _todoItemsService = todoItemsService ?? throw new ArgumentNullException(nameof(todoItemsService));
            _logger = loggerFactory.CreateLogger<TodoItemsController>();
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems(CancellationToken cancellationToken)
        {
            var results = await _todoItemsService.GetTodoItems();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id, CancellationToken cancellationToken)
        {
            var result = await _todoItemsService.GetTodoItem(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItemDto todoItem, CancellationToken cancellationToken)
        {
            try
            {
                await _todoItemsService.SetTodoItem(id, todoItem, cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TodoItemIdExists(id, cancellationToken))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        } 

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItemDto todoItem, CancellationToken cancellationToken)
        {
            if (await TodoItemIdExists(todoItem.Id, cancellationToken))
            {
                return BadRequest("Duplicated Id");
            }
            else if (string.IsNullOrEmpty(todoItem?.Description))
            {
                return BadRequest("Description is required");
            }
            else if (await TodoItemDescriptionExists(todoItem.Description, cancellationToken))
            {
                return BadRequest("Description already exists");
            }

            await _todoItemsService.CreateTodoItem(todoItem, cancellationToken);
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        } 

        private async Task<bool> TodoItemIdExists(Guid id, CancellationToken cancellationToken)
        {
            return await _todoItemsService.IsIdExists(id, cancellationToken);
        }

        private async Task<bool> TodoItemDescriptionExists(string description, CancellationToken cancellationToken)
        {
            return await _todoItemsService.IsDescriptionExists(description, cancellationToken);
        }
    }
}
