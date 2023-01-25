﻿namespace TodoApi {
	public class TodoItemDTO {
		public int Id { get; set; }
		public string? Name { get; set; }
		public bool IsComplete { get; set; }

		public TodoItemDTO() { }
		// public TodoItemDTO(Todo todoItem) => (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);

		public TodoItemDTO(Todo todoItem) {
			Id = todoItem.Id;
			Name = todoItem.Name;
			IsComplete = todoItem.IsComplete;
		}
	}
}
