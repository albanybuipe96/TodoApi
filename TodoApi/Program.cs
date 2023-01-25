using Microsoft.EntityFrameworkCore;
using TodoApi;

internal class Program {
	private static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);
		builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
		builder.Services.AddDatabaseDeveloperPageExceptionFilter();
		var app = builder.Build();

		var todoItems = app.MapGroup("/todoitems");

		todoItems.MapGet("/", GetAllTodos);

		todoItems.MapGet("/complete", GetCompletedTodos);

		todoItems.MapGet("/{id}", GetTodo);

		todoItems.MapPost("/", CreateTodo);

		todoItems.MapPut("/{id}", UpdateTodo);

		todoItems.MapDelete("/{id}", DeleteTodo);

		app.Run();

		static async Task<IResult> GetAllTodos(TodoDb db) {
			return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
		}

		static async Task<IResult> GetCompletedTodos(TodoDb db) {
			return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
		}

		static async Task<IResult> GetTodo(int id, TodoDb db) {
			return await db.Todos.FindAsync(id) 
				is Todo todo 
					? TypedResults.Ok(new TodoItemDTO(todo))
					: TypedResults.NotFound();
		}

		static async Task<IResult> CreateTodo(TodoItemDTO todoItemDto, TodoDb db) {

			var todoItem = new Todo { 
				IsComplete = todoItemDto.IsComplete,
				Name = todoItemDto.Name,
			};

			db.Todos.Add(todoItem);
			await db.SaveChangesAsync();

			return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItem);
		}

		static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db) {
			
			var todo = await db.Todos.FindAsync(id);

			if (todo is null) return TypedResults.NotFound();

			todo.Name = todoItemDTO.Name;
			todo.IsComplete = todoItemDTO.IsComplete;

			await db.SaveChangesAsync();

			return TypedResults.NoContent();
		}

		static async Task<IResult> DeleteTodo(int id, TodoDb db) {
			if (await db.Todos.FindAsync(id) is Todo todo) {
				db.Todos.Remove(todo);
				await db.SaveChangesAsync();
				
				return TypedResults.Ok(todo);
			}

			return TypedResults.NotFound();
		}

	}
}