using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Api.Application.Abstractions;
using TodoList.Api.Application.Services;
using TodoList.Api.Infrastructure.Context;
using TodoList.Api.Infrastructure.Repositories;

namespace TodoList.Api.UnitTests
{
    public class ServicesFixture
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ServicesFixture()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            ServiceProvider = new ServiceCollection()
            .AddHttpClient()
                .AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoItemsDB"))
                .AddScoped<ITodoItemsRepository, TodoItemsRepository>()
                .AddScoped<ITodoItemsService, TodoItemsService>()
                .BuildServiceProvider();
        }
    }
}
