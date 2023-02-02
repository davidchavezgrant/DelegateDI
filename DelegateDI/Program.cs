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
															      return innerId;
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
														    return innerId;
													    };
											 });

builder.Services.AddTransient<MyTransientDelegate>(serviceProvider =>
												   {
													   var outerId = Guid.NewGuid();
													   var outerSingleton   = serviceProvider.GetRequiredService<MySingletonDelegate>();
													   var outerSingletonId = outerSingleton();
													   Console.WriteLine($"Located Singleton: {outerSingletonId} while CONSTRUCTING {nameof(MyTransientDelegate)}: {outerId}");
													   return () =>
														      {
															      var innerSingleton   = serviceProvider.GetRequiredService<MySingletonDelegate>();
															      var innerSingletonId = innerSingleton();
															      var innerId          = Guid.NewGuid();
																  Console.WriteLine($"Located Singleton: {innerSingletonId} while INVOKING {nameof(MyTransientDelegate)}: {innerId}");
																  var outerInvokedFromInner = outerSingleton();
																  Console.WriteLine("Invoking the outer singleton from the inner transient: " + outerInvokedFromInner);
															      return innerId;
														      };
												   });
var app = builder.Build();

app.MapGet("/singleton-service",
		   (MySingletonService service) =>
		   {
			   service.Log();
			   return $"{nameof(MySingletonService)} {service.Id}";
		   });
app.MapGet("/singleton-delegate",
		   (MySingletonDelegate handle) =>
		   {
			   var handlerId = handle();
			   return $"{nameof(MySingletonDelegate)} {handlerId}";
		   });
app.MapGet("/scoped-service",
		   (MyScopedService service) =>
		   {
			   service.Log();
			   return $"{nameof(MyScopedService)} {service.Id}";
		   });
app.MapGet("/scoped-delegate",
		   (MyScopedDelegate handle) =>
		   {
			   var handlerId = handle();
			   return $"{nameof(MyScopedDelegate)} {handlerId}";
		   });
app.MapGet("/transient-service",
		   (MyTransientService service) =>
		   {
			   service.Log();
			   return $"{nameof(MyTransientService)} {service.Id}";
		   });
app.MapGet("/transient-delegate",
		   (MyTransientDelegate handle) =>
		   {
			   var handlerId = handle();
			   return $"{nameof(MyTransientDelegate)} {handlerId}";
		   });

app.UseSwagger();
app.UseSwaggerUI();
app.Run();