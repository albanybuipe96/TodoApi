internal class Program {
	private static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);
		var app = builder.Build();

		app.MapGet("/", () => "Hello from DotNet Core!");

		app.Run();
	}
}