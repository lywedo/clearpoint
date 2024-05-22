using System;

namespace TodoList.Api.Application.Dtos
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
