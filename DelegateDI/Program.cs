using DelegateDI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MySingletonService>();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddTransient<MyTransientService>();
builder.Services.AddSingleton<MySingletonDelegate>(serviceProvider =>
												   {
													   Console.WriteLine($"Construct {nameof(MySingletonDelegate)}");
													   return () => Console.WriteLine($"Invoke {nameof(MySingletonDelegate)}");
												   });
builder.Services.AddScoped<MyScopedDelegate>(serviceProvider =>
											 {
												 Console.WriteLine($"Construct {nameof(MyScopedDelegate)}");
												 return () => Console.WriteLine($"Invoke {nameof(MyScopedDelegate)}");
											 });
builder.Services.AddTransient<MyTransientDelegate>(serviceProvider =>
												   {
													   Console.WriteLine($"Construct {nameof(MyTransientDelegate)}");
													   return () => Console.WriteLine($"Invoke {nameof(MyTransientDelegate)}");
												   });
var app = builder.Build();

app.MapGet("/singleton-service",
		   (MySingletonService service) =>
		   {
			   Console.WriteLine($"{nameof(MySingletonService)} endpoint");
			   service.Log();
			   return nameof(MySingletonService);
		   });
app.MapGet("/singleton-delegate",
		   (MySingletonDelegate handle) =>
		   {
			   Console.WriteLine($"{nameof(MySingletonDelegate)} endpoint");
			   handle();
			   return nameof(MySingletonDelegate);
		   });
app.MapGet("/scoped-service",
		   (MyScopedService service) =>
		   {
			   Console.WriteLine($"{nameof(MyScopedService)} endpoint");
			   service.Log();
			   return nameof(MyScopedService);
		   });
app.MapGet("/scoped-delegate",
		   (MyScopedDelegate handle) =>
		   {
			   Console.WriteLine($"{nameof(MyScopedDelegate)} endpoint");
			   handle();
			   return nameof(MyScopedDelegate);
		   });
app.MapGet("/transient-service",
		   (MyTransientService service) =>
		   {
			   Console.WriteLine($"{nameof(MyTransientService)} endpoint");
			   service.Log();
			   return nameof(MyTransientService);
		   });
app.MapGet("/transient-delegate",
		   (MyTransientDelegate handle) =>
		   {
			   Console.WriteLine($"{nameof(MyTransientDelegate)} endpoint");
			   handle();
			   return nameof(MyTransientDelegate);
		   });

app.UseSwagger();
app.UseSwaggerUI();
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