using DelegateDI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MySingletonService>();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddTransient<MyTransientService>();
builder.Services.AddSingleton<MySingletonDelegate>(serviceProvider =>
												   {
													   Console.WriteLine("Construct singleton delegate");
													   return () => Console.WriteLine("Invoke singleton delegate");
												   });
builder.Services.AddScoped<MyScopedDelegate>(serviceProvider =>
											 {
												 Console.WriteLine("Construct scoped delegate");
												 return () => Console.WriteLine("Invoke scoped delegate");
											 });
builder.Services.AddTransient<MyTransientDelegate>(serviceProvider =>
												   {
													   Console.WriteLine("Construct transient delegate");
													   return () => Console.WriteLine("Invoke transient delegate");
												   });
var app = builder.Build();

app.MapGet("/singleton-service",
		   (MySingletonService service) =>
		   {
			   Console.WriteLine("Singleton service endpoint");
			   service.Log();
			   return "Singleton Service";
		   });
app.MapGet("/singleton-delegate",
		   (MySingletonDelegate handler) =>
		   {
			   Console.WriteLine("Singleton delegate endpoint");
			   handler();
			   return "Singleton Delegate";
		   });
app.MapGet("/scoped-service",
		   (MyScopedService service) =>
		   {
			   Console.WriteLine("Scoped service endpoint");
			   service.Log();
			   return "Scoped Service";
		   });
app.MapGet("/scoped-delegate",
		   (MyScopedDelegate handler) =>
		   {
			   Console.WriteLine("Scoped delegate endpoint");
			   handler();
			   return "Scoped Delegate";
		   });
app.MapGet("/transient-service",
		   (MyTransientService service) =>
		   {
			   Console.WriteLine("Transient service endpoint");
			   service.Log();
			   return "Transient Service";
		   });
app.MapGet("/transient-delegate",
		   (MyTransientDelegate handler) =>
		   {
			   Console.WriteLine("Transient delegate endpoint");
			   handler();
			   return "Transient Delegate";
		   });

app.Run();

interface IService
{
	Guid Id { get; }
	Task LogAsync();
	void Log();
}

delegate void MySingletonDelegate();

delegate void MyScopedDelegate();

delegate void MyTransientDelegate();

delegate Task MySingletonDelegateAsync();

delegate Task MyScopedDelegateAsync();

delegate Task MyTransientDelegateAsync();