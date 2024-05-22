using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Api.Application.Abstractions;
using TodoList.Api.Application.Dtos;
using TodoList.Api.Domain.Models;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsTest : IClassFixture<ServicesFixture>
    {
        private readonly ServicesFixture _servicesFixture;

        public TodoItemsTest()
        {
            _servicesFixture = new ServicesFixture();
        }

        [Fact]
        public async Task TodoItemsRepositoryTestAsync()
        {
            var repo = _servicesFixture.ServiceProvider.GetService<ITodoItemsRepository>();
            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                IsCompleted = false,
                Description = "Test",
            };
            await repo.CreateTodoItem(todoItem, CancellationToken.None);
            Assert.True(await repo.IsIdExits(todoItem.Id));
            Assert.True(await repo.IsDescriptionExists(todoItem.Description));
            var resItem = await repo.GetTodoItem(todoItem.Id, CancellationToken.None);
            Assert.Equal(todoItem, resItem);
            var resItems = await repo.GetTodoItems(CancellationToken.None);
            Assert.Contains(todoItem, resItems);
            todoItem.IsCompleted = true;
            var setResult = await repo.SetTodoItem(todoItem.Id, todoItem);
            Assert.Equal(1, setResult);
            resItems = await repo.GetTodoItems(CancellationToken.None);
            Assert.DoesNotContain(todoItem, resItems);
            resItem = await repo.GetTodoItem(todoItem.Id, CancellationToken.None);
            Assert.True(resItem.IsCompleted);
        }

        [Fact]
        public async Task TodoItemsServiceTestAsync()
        {
            var service = _servicesFixture.ServiceProvider.GetService<ITodoItemsService>();
            var todoItem = new TodoItemDto
            {
                Id = Guid.NewGuid(),
                IsCompleted = false,
                Description = "Test",
            };
            await service.CreateTodoItem(todoItem, CancellationToken.None);
            Assert.True(await service.IsIdExists(todoItem.Id));
            Assert.True(await service.IsDescriptionExists(todoItem.Description));
            var resItem = await service.GetTodoItem(todoItem.Id, CancellationToken.None);
            Assert.Equal(todoItem.Id, resItem.Id);
            Assert.Equal(todoItem.IsCompleted, resItem.IsCompleted);
            Assert.Equal(todoItem.Description, resItem.Description);
            var resItems = await service.GetTodoItems(CancellationToken.None);
            Assert.True(resItems.Any(i => i.Id == todoItem.Id && i.IsCompleted == todoItem.IsCompleted && i.Description == todoItem.Description));
            todoItem.IsCompleted = true;
            var setResult = await service.SetTodoItem(todoItem.Id, todoItem);
            Assert.Equal(1, setResult);
            resItems = await service.GetTodoItems(CancellationToken.None);
            Assert.False(resItems.Any(i => i.Id == todoItem.Id && i.IsCompleted == todoItem.IsCompleted && i.Description == todoItem.Description));
            resItem = await service.GetTodoItem(todoItem.Id, CancellationToken.None);
            Assert.True(resItem.IsCompleted);
        }
    }
}
