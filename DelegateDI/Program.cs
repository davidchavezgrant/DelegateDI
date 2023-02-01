var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MySingletonService>();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddTransient<MyTransientService>();
builder.Services.AddSingleton<MySingletonDelegate>(serviceProvider =>
												   {
													   Console.WriteLine("Construct singleton delegate");
													   return () =>
														      {
															      Console.WriteLine("Invoke singleton delegate");
															      return Task.CompletedTask;
														      };
												   });
builder.Services.AddScoped<MyScopedDelegate>(serviceProvider =>
											 {
												 Console.WriteLine("Construct scoped delegate");
												 return () =>
													    {
														    Console.WriteLine("Invoke scoped delegate");
														    return Task.CompletedTask;
													    };
											 });
builder.Services.AddTransient<MyTransientDelegate>(serviceProvider =>
												   {
													   Console.WriteLine("Construct transient delegate");
													   return () =>
														      {
															      Console.WriteLine("Invoke transient delegate");
															      return Task.CompletedTask;
														      };
												   });
var app = builder.Build();

app.MapGet("/singleton-service",  (MySingletonService  service) => "Singleton Service");
app.MapGet("/singleton-delegate", (MySingletonDelegate service) => "Singleton Delegate");
app.MapGet("/scoped-service",     (MyScopedService     service) => "Scoped Service");
app.MapGet("/scoped-delegate",    (MyScopedDelegate    service) => "Scoped Delegate");
app.MapGet("/transient-service",  (MyTransientService  service) => "Transient Service");
app.MapGet("/transient-delegate", (MyTransientDelegate service) => "Transient Delegate");

app.Run();

class MySingletonService: IDisposable
{
	public MySingletonService() { Console.WriteLine($"Created {nameof(MySingletonService)}"); }

	/// <inheritdoc />
	public void Dispose() { Console.WriteLine($"Disposed {nameof(MySingletonService)}"); }
}

class MyTransientService: IDisposable
{
	public MyTransientService() { Console.WriteLine($"Created {nameof(MyTransientService)}"); }
	public void Dispose() { Console.WriteLine($"Disposed {nameof(MyTransientService)}"); }
}

class MyScopedService: IDisposable
{
	public MyScopedService() { Console.WriteLine($"Created {nameof(MyScopedService)}"); }
	public void Dispose() { Console.WriteLine($"Disposed {nameof(MyScopedService)}"); }
}

delegate Task MySingletonDelegate();

delegate Task MyScopedDelegate();

delegate Task MyTransientDelegate();