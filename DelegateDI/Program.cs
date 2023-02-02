using DelegateDI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MySingletonService>();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddTransient<MyTransientService>();
builder.Services.AddSingleton<MySingletonDelegate>(serviceProvider =>
												   {
													   var outerId = Guid.NewGuid();
													   Console.WriteLine($"Construct {nameof(MySingletonDelegate)} with outerId: {outerId}");
													   return () =>
														      {
															      var innerId = Guid.NewGuid();
															      Console.WriteLine($"Invoke {nameof(MySingletonDelegate)} with outerId: {outerId} and innerId: {innerId}");
														      };
												   });
builder.Services.AddScoped<MyScopedDelegate>(serviceProvider =>
											 {
												 var outerId = Guid.NewGuid();
												 Console.WriteLine($"Construct {nameof(MyScopedDelegate)} with outerId: {outerId}");
												 return () =>
													    {
														    var innerId = Guid.NewGuid();
														    Console.WriteLine($"Invoke {nameof(MyScopedDelegate)} with outerId: {outerId} and innerId: {innerId}");
													    };
											 });
builder.Services.AddTransient<MyTransientDelegate>(serviceProvider =>
												   {
													   var outerId = Guid.NewGuid();
													   Console.WriteLine($"Construct {nameof(MyTransientDelegate)} with outerId: {outerId}");
													   return () =>
														      {
															      var innerId = Guid.NewGuid();
															      Console.WriteLine($"Invoke {nameof(MyTransientDelegate)} with outerId: {outerId} and innerId: {innerId}");
														      };
												   });
var app = builder.Build();

app.MapGet("/singleton-service",
		   (MySingletonService service) =>
		   {
			   var requestId = Guid.NewGuid();
			   Console.WriteLine($"{nameof(MySingletonService)} endpoint with requestId {requestId}");
			   service.Log();
			   return $"{nameof(MySingletonService)} {service.Id}";
		   });
app.MapGet("/singleton-delegate",
		   (MySingletonDelegate handle) =>
		   {
			   var requestId = Guid.NewGuid();
			   Console.WriteLine($"{nameof(MySingletonDelegate)} endpoint with requestId {requestId}");
			   handle();
			   return nameof(MySingletonDelegate);
		   });
app.MapGet("/scoped-service",
		   (MyScopedService service) =>
		   {
			   var requestId = Guid.NewGuid();
			   Console.WriteLine($"{nameof(MyScopedService)} endpoint with requestId {requestId}");
			   service.Log();
			   return nameof(MyScopedService);
		   });
app.MapGet("/scoped-delegate",
		   (MyScopedDelegate handle) =>
		   {
			   var requestId = Guid.NewGuid();
			   Console.WriteLine($"{nameof(MyScopedDelegate)} endpoint with requestId {requestId}");
			   handle();
			   return nameof(MyScopedDelegate);
		   });
app.MapGet("/transient-service",
		   (MyTransientService service) =>
		   {
			   var requestId = Guid.NewGuid();
			   Console.WriteLine($"{nameof(MyTransientService)} endpoint with requestId {requestId}");
			   service.Log();
			   return nameof(MyTransientService);
		   });
app.MapGet("/transient-delegate",
		   (MyTransientDelegate handle) =>
		   {
			   var requestId = Guid.NewGuid();
			   Console.WriteLine($"{nameof(MyTransientDelegate)} endpoint with requestId {requestId}");
			   handle();
			   return nameof(MyTransientDelegate);
		   });

app.UseSwagger();
app.UseSwaggerUI();
app.Run();